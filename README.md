# Dotnet REST api versioning and documentation example

## Overview

This project is a RESTful CRUD API for managing users, built with **.NET 9**. It leverages **Entity Framework Core** for data access with **Microsoft SQL Server** as the database backend. The API follows best practices, including structured response wrapping via a custom `ApiResponse<T>` and integrates with a scalar API for specific data handling operations.

### Key Features

- **API Versioning:** Supports multiple API versions (e.g., `v1`, `v2`), enabling backward compatibility and iterative improvements.
- **Clean Code & Standard Architecture:** Designed using clean code principles and a standard layered architecture for maintainability, testability, and scalability.
- **OpenAPI Documentation:** Fully integrated OpenAPI (Swagger) support to provide interactive API documentation and ease of exploration.
- **CRUD operations:** Create, Read, Update, and Delete users with well-defined request and response models.
- **File Upload Support:** Upload and update user profile photos using a dedicated `FileUploadService`.
- **Strongly-Typed Responses:** Uses generic `ApiResponse<T>` wrappers to standardize API responses with success, error messages, and HTTP status codes.
- **Dependency Injection & Logging:** Utilizes built-in .NET dependency injection for service management and integrated logging for monitoring and troubleshooting.
- **EF Core with MS SQL:** Entity Framework Core with Microsoft SQL Server backend for efficient and reliable data persistence.
- **Scalar API Integration:** Includes support for scalar API calls for specialized data operations (add details here if needed).

  
### Technologies Used

- **.NET 9** (ASP.NET Core Web API)
- **Entity Framework Core** (EF Core)
- **Microsoft SQL Server** (MS SQL)
- **API Versioning** (Microsoft.AspNetCore.Mvc.Versioning)
- **OpenAPI / Swagger** for API documentation
- **Dependency Injection & Logging** (built into .NET)
- **Scalar API** for custom data operations

### API Versioning

The API is versioned to allow multiple iterations to coexist:

- `api/v1/users` - Version 1 endpoints
- `api/v2/users` - Version 2 endpoints with potential enhancements

This enables smooth upgrades and client compatibility management.

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

- Use the versioned routes to access the API endpoints.
- Profile photos are updated by sending a multipart/form-data request to the photo update endpoint.
- All responses are wrapped in the `ApiResponse<T>` format for consistency.
- Explore the API via the integrated Scalar UI available at `/scalar` endpoint.

---

Feel free to extend this API by adding authentication, pagination, filtering, or other advanced features depending on your project needs.

---

**Note:** Make sure to configure your connection string for MS SQL Server in `appsettings.json` and apply EF Core migrations before running the API.

---

## Contact

For questions or contributions, please open an issue or pull request.

