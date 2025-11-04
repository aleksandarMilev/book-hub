import { useState } from 'react';

export default function usePersistedState<T>(
  localStorageKey: string,
  initialState: T,
): [T, (value: T) => void] {
  const [state, setState] = useState<T>(() => {
    try {
      const storedValue = localStorage.getItem(localStorageKey);
      return storedValue ? (JSON.parse(storedValue) as T) : initialState;
    } catch {
      return initialState;
    }
  });

  const setPersistedState = (value: T): void => {
    localStorage.setItem(localStorageKey, JSON.stringify(value));

    setState(value);
  };

  return [state, setPersistedState];
}
