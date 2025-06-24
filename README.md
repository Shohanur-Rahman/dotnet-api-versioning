# Dotnet REST api versioning and documentation example

## Overview

This project is a RESTful CRUD API for managing users, built with **.NET 9**. It leverages **Entity Framework Core** for data access with **Microsoft SQL Server** as the database backend. The API follows best practices, including structured response wrapping via a custom `ApiResponse<T>` and integrates with a scalar API for specific data handling operations.

### Key Features

- **Create, Read, Update, Delete (CRUD)** operations for User entities.
- Strongly-typed requests and responses with models such as `UserCreateRequest`, `UserUpdateRequest`, and `UserDetailResponse`.
- File upload support for updating user profile photos using a dedicated `FileUploadService`.
- Detailed API response handling with HTTP status codes and meaningful error messages.
- Dependency injection for service management, including logging and user business logic separation (`IUserService`).
- Exception handling and logging integrated in controller actions.
- Uses MS SQL via EF Core for persistent storage.
- Designed with scalability and maintainability in mind using clean architecture principles.
  
### Technologies Used

- **.NET 9** (ASP.NET Core Web API)
- **Entity Framework Core** for ORM
- **Microsoft SQL Server** (MS SQL) as the database
- **Scalar API** for specialized data interactions (please specify the scalar API details here if needed)
- Built-in **Dependency Injection** and **Logging**

### API Endpoints

| Method | Route           | Description                         |
|--------|-----------------|-----------------------------------|
| GET    | `/api/users`    | Get all users                     |
| GET    | `/api/users/{id}` | Get a single user by ID          |
| POST   | `/api/users`    | Create a new user                 |
| PUT    | `/api/users`    | Update an existing user           |
| PATCH  | `/api/users/photo-update` | Update user profile photo |
| DELETE | `/api/users`    | Delete a user by ID               |

### Usage

- Send requests to the respective endpoints with appropriate payloads.
- Profile photos can be updated by uploading a file through multipart/form-data in the `photo-update` endpoint.
- The API responds with standardized `ApiResponse<T>` objects to simplify client-side error handling.

---

Feel free to extend this API by adding authentication, pagination, filtering, or other advanced features depending on your project needs.

---

**Note:** Make sure to configure your connection string for MS SQL Server in `appsettings.json` and apply EF Core migrations before running the API.

---

## Contact

For questions or contributions, please open an issue or pull request.

