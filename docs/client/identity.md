# Identity Feature - **Client Documentation**

This document summarizes the **client-side** (React) implementation of the Identity feature in BookHub.

---

## Overview

The client identity layer provides:

- Register and login forms.
- Integration with the JWT‑based backend.
- Persistent auth state via Zustand.
- Localized success/error messages.
- Proper handling of server‑side validation errors.

---

## Types

**Location:** `client/src/features/identity/types/identity.ts`

```ts
export interface LoginResponse {
  token: string;
}

export interface RegisterRequest {
  username: string;
  email: string;
  password: string;
}

export interface LoginRequest {
  credentials: string;
  rememberMe: boolean;
  password: string;
}

export type DecodedToken = {
  nameid: string;
  unique_name: string;
  email: string;
  role?: string;
};

export interface RegisterFormValues extends RegisterRequest {
  confirmPassword: string;
}
```

- `LoginResponse` mirrors server `LoginResponseModel`.
- `DecodedToken` matches the JWT claims created by the server.

---

## HTTP & Error Handling

**Location:** `client/src/shared/api/http.ts`

- `http` and `httpAdmin` are `axios` instances using `baseUrl` and `baseAdminUrl`.
- Helper functions:
  - `getAuthConfig(token, signal)`
  - `getAuthConfigForFile(token, signal)`
  - `getPublicConfig(signal)`

### `processError`

```ts
export function processError(error: unknown, fallbackMessage: string): never {
  const isRequestCanceled = axios.isCancel?.(error) || IsCanceledError(error);

  if (isRequestCanceled) {
    throw error;
  }

  if (axios.isAxiosError(error) && error.response?.data) {
    const data = error.response.data;
    const serverMessage = data.errorMessage || data.message || data.title;

    if (serverMessage && typeof serverMessage === "string") {
      throw new Error(serverMessage);
    }
  }

  throw new Error(fallbackMessage);
}
```

- Prioritizes server error text (`errorMessage`) when present.
- Falls back to a localized generic message.
- Used by the Identity API client and other feature APIs.

---

## Identity API Client

**Location:** `client/src/features/identity/api/api.ts`

```ts
export const register = async (username, email, password, signal?) => {
  try {
    const url = `${routes.register}`;
    const payload: RegisterRequest = { username, email, password };
    const { data } = await http.post<LoginResponse>(
      url,
      payload,
      getConfig(signal)
    );
    return data;
  } catch (error) {
    processError(error, errors.identity.register);
  }
};

export const login = async (credentials, password, rememberMe, signal?) => {
  try {
    const url = `${routes.login}`;
    const payload: LoginRequest = { credentials, password, rememberMe };
    const { data } = await http.post<LoginResponse>(
      url,
      payload,
      getConfig(signal)
    );
    return data;
  } catch (error) {
    processError(error, errors.identity.login);
  }
};
```

- On success: returns `{ token }`.
- On error: throws `Error` with:
  - Server `errorMessage` (e.g. duplicate email/username), or
  - A generic localized text from `errors.identity.*`.

---

## Auth Store Integration

The identity hooks use the auth store in `client/src/shared/stores/auth`:

- The store holds:

  ```ts
  {
    userId: string;
    username: string;
    email: string;
    token: string;
    isAdmin: boolean;
    hasProfile: boolean;
    isAuthenticated: boolean;
  }
  ```

- State persists via `localStorage` using `zustand` + `persist`.

---

## Hooks

**Location:** `client/src/features/identity/hooks/useIdentity.ts`

### `useLogin`

```ts
export const useLogin = () => {
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();
  const { t } = useTranslation("identity");

  const onLogin = useCallback(
    async (credentials: string, password: string, rememberMe: boolean) => {
      try {
        const result = await identityApi.login(
          credentials,
          password,
          rememberMe
        );
        const decoded = jwtDecode<DecodedToken>(result.token);

        const user: User = {
          userId: decoded.nameid,
          username: decoded.unique_name,
          email: decoded.email,
          token: result.token,
          isAdmin: Boolean(decoded.role),
          hasProfile: await profileApi.hasProfile(result.token),
        };

        changeAuthenticationState(user);
        showMessage(t("messages.welcome", { username: user.username }), true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        const message = t("messages.loginFailed");
        throw new Error(message);
      }
    },
    [changeAuthenticationState, showMessage, t]
  );

  return onLogin;
};
```

- Calls login API, decodes JWT, and populates auth store.
- Sets `hasProfile` by calling `profileApi.hasProfile(token)`.
- Shows a localized welcome toast.
- On failure, throws a **generic** `loginFailed` message (server details are not surfaced here).

### `useRegister`

```ts
export const useRegister = () => {
  const { showMessage } = useMessage();
  const { changeAuthenticationState } = useAuth();
  const { t } = useTranslation("identity");

  const onRegister = useCallback(
    async (username: string, email: string, password: string) => {
      try {
        const result = await identityApi.register(username, email, password);
        const decoded = jwtDecode<DecodedToken>(result.token);

        const user: User = {
          userId: decoded.nameid,
          username: decoded.unique_name,
          email: decoded.email,
          token: result.token,
          isAdmin: Boolean(decoded.role),
          hasProfile: false,
        };

        changeAuthenticationState(user);
        showMessage(t("messages.welcome", { username: user.username }), true);
      } catch (error) {
        if (IsCanceledError(error)) {
          return;
        }

        // Re-throw so forms can display the server or fallback message
        throw error;
      }
    },
    [changeAuthenticationState, showMessage, t]
  );

  return onRegister;
};
```

- On failure, rethrows the error from `processError` so forms can show:
  - Server validation details (e.g. duplicate email/username), or
  - A localized fallback.

---

## Forms & Components

### Login

**Location:**

- `components/login/Login.tsx`
- `components/login/formik/useLoginFormik.ts`
- `components/login/validation/loginSchema.ts`
- `components/login/Login.css`

Key points:

- Formik manages `credentials`, `password`, `rememberMe`.
- Yup validation ensures required fields.
- On submit:
  - Calls `useLogin`.
  - Navigates to `routes.home` on success.
- On error:
  - Displays message under `credentials` (usually generic `loginFailed`).

### Register

**Location:**

- `components/register/Register.tsx`
- `components/register/formik/useRegisterFormik.ts`
- `components/register/validation/registerSchema.ts`
- `components/register/Register.css`

Key points:

- Formik manages `username`, `email`, `password`, `confirmPassword`.
- Yup validation:
  - Required username/email/password.
  - Email format.
  - `confirmPassword` must match `password`.
- On submit:
  - Calls `useRegister`.
  - Navigates to `routes.home` on success.
- On error:
  - Displays `error.message` (from server or fallback) under the `username` field.

### Logout

**Location:** `components/logout/Logout.tsx`

- Reads current `username` from auth store.
- Calls `logout()` to clear auth state.
- Shows localized goodbye toast (if username existed).
- Redirects to `routes.home`.

---

## Error Handling Behavior

- **Register**:

  - Server sends `{ errorMessage }` on validation failures (e.g. duplicate email/username).
  - `processError` converts that into `Error(errorMessage)`.
  - `useRegister` rethrows the error.
  - Register form shows this message in the UI.

- **Login**:
  - Errors are collapsed into a generic `loginFailed` message via `useLogin`.
  - Provides a consistent UX without exposing detailed login reasons.

---

## Summary

The client identity feature:

- Talks to the server using a small API wrapper and shared `processError`.
- Keeps auth state in a persisted Zustand store, based on JWT claims.
- Provides localized, styled login/register/logout screens.
- Surfaces detailed server errors on registration but uses generic messages on login for safety.
