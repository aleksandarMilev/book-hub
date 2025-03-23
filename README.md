# BookHub

BookHub is a full-featured web application for book lovers. It allows users to manage books and authors, write and vote on reviews, create personalized reading lists, read articles, and participate in chats. 
> This project is my final course assignment for the SoftUni C# Web track. The entire application is fully Dockerized.

---

## ✨ Features

- **User Authentication**: Users must register with a username, email, and password. Only the homepage is accessible to unauthenticated visitors.
- **Profile Management**: Users can complete their profile with additional details. A completed profile unlocks most platform functionalities.
- **Books & Authors**: Authenticated users can create books and authors. All submissions require admin approval before becoming publicly available.
- **Reviews & Voting**: Users can write reviews for books and vote on others' reviews. Requires a completed profile.
- **Reading Lists**: Organize books into lists categorized as "Read," "To Read," and "Currently Reading."
- **Articles**: Read curated literary articles without the need for authentication. Article creation is limited to admins.
- **Chats**: Start or join discussions with other users. Requires a completed profile.
- **Notifications**: Get notified about key events like admin approvals, chat invites, and invitation responses.
- **Advanced Search**: Search across books, authors, articles, and more.

---

## 🔐 Admin Access

A seeded admin account is available for testing admin-specific features:

```
Username: Administrator
Password: admin1234
```

---

## 🚀 Getting Started

### Prerequisites
- Docker  
**Or manually:**
- Node.js
- .NET SDK
- MS SQL Server

### Installation & Setup

#### Option A: Docker (Recommended)
```bash
git clone https://github.com/aleksandarMilev/book-hub.git
cd book-hub
docker-compose up --build -d
```
- Client: [localhost:80](http://localhost:80)
- Server: [localhost:8080](http://localhost:8080)

#### Option B: Run Client Locally
```bash
git clone https://github.com/aleksandarMilev/book-hub.git
cd book-hub/client
npm i
npm run dev
```

#### Option C: Run Server Locally
```bash
git clone https://github.com/aleksandarMilev/book-hub.git
cd book-hub/server
dotnet restore
dotnet run
```

---

## 🛠 Technologies Used

- **Frontend**: ReactJS
- **Backend**: ASP.NET Core Web API
- **Database**: MS SQL Server
- **ORM**: Entity Framework Core
- **Other Tools & Libraries**:
  - AutoMapper
  - Swagger
  - XUnit
  - Moq
  - Bootstrap

---

## 📐 Project Structure and Architecture

### 📊 Database

![Database Schema](./screenshots/db.png)

---

### 🔧 Back-End Architecture

BookHub uses a classic monolithic 3-layer architecture: **Web**, **Service**, and **Data** layers.

![Server Structure](./screenshots/server.png)

#### **Areas**
![Areas](./screenshots/areas.png)  
Minimal logic; includes:
- `AdminApiController` base controller
- Admin ID retrieval service

#### **Common**
- Global constants
- Shared exception: `DbEntityNotFoundException`

#### **Data**
![Data](./screenshots/data.png)  
- `Base`: Abstract base entities with audit fields
- `Shared`: Entities like `BookGenre` for many-to-many relationships
- `BookHubDbContext.cs`:
  - `ApplyAuditInfo()` for automatic audit fields (created by/on, modified by/on)
  - **Global Query Filters**:
    - `IApprovableEntity`: filters unapproved entries
    - `IDeletableEntity`: filters soft-deleted entities

#### **Infrastructure**
![Infrastructure](./screenshots/infrastructure.png)

- **Extensions**: Modular startup setup via extension methods:
  - `AddDatabase`, `AddIdentity`, `AddJwtAuthentication`, `AddSwagger`
  - `AddServices`: Registers services by convention using marker interfaces (`IScopedService`, `ITransientService`, `ISingletonService`)
  - `AddApiControllers`, `AddAutoMapper`, `GetAppSettings`

- **Filters**:
  - `ModelOrNotFoundActionFilter`: Returns `BadRequest` if null, `Ok` otherwise

- **Services**:
  - `CurrentUserService`: Retrieves current user context
  - `Result` and `ResultWith<T>` for standardized success/failure responses

#### **Features**
![Features](./screenshots/features.png)

Each feature has its own directory containing:

- **Web**:
  - **Controllers**: User/Admin separation
  - **Models**: Input validation for client-side data

- **Service**:
  - **Models**: Internal service models decoupled from web
  - **Implementation**: Interfaces + logic

- **Mapper**:
  - AutoMapper profiles
  - Manual mapping where necessary

- **Data**:
  - Entity models, EF configurations, and feature-specific seeders

- **Shared**:
  - Validation constants, enums, etc.

#### **Tests**
![Tests](./screenshots/tests.png)

- Uses EF InMemory
- Dependencies mocked with Moq
- AutoMapper is not mocked
- FluentAssertions used for expressive tests

---

### 💻 Front-End Architecture

![Client Structure](./screenshots/client.png)

Logic is offloaded from components into custom hooks that call `fetch()` wrappers. Component → Hook → API file.

#### `assets`
Static images

#### `api`
![API](./screenshots/api.png)
- Each backend feature has a corresponding JS file
- Handles token auth, builds endpoints, and returns processed results

#### `common`
Constants, error messages, and utility functions

#### `components`
![Components](./screenshots/components.png)  
All JSX files per feature

#### `contexts`
- `MessageContext`: Displays global alerts
- `UserContext`: Tracks and persists user state, profile status, and admin rights

#### `hooks`
![Hooks](./screenshots/hooks.png)  
Feature logic in hooks like `useBookDetails()`

#### `App.jsx` & Custom Routing

Inside `src/components/common`:

- **AuthenticatedRoute**: Redirects unauthenticated users to login
- **AdminRoute**: Redirects non-admins to "Access Denied"
- **ProfileRoute**: Ensures user has a profile before accessing features
- **ChatRoute**: Ensures user is part of the chat before showing chat details

These routes improve UX but rely on back-end validation for security.

---

## 🏠 Home Page

![Screenshot 1](./screenshots/scr1.png)  
![Screenshot 2](./screenshots/scr2.png)  
![Screenshot 3](./screenshots/scr3.png)  
![Screenshot 4](./screenshots/scr4.png)  
![Screenshot 5](./screenshots/scr5.png)

---

## 📄 License

This project is licensed under the **MIT License**.
