# Library Management System API

## Overview
This repository contains a RESTful Web API designed to model a foundational Library Management System. The project was built using **ASP.NET Core Web API** and demonstrates modern .NET development practices, specifically targeting Object-Oriented Programming (OOP) concepts, separation of concerns, and relational data modeling.

## Architecture & Design Patterns
To satisfy the OOP requirements and ensure maintainability, the application architecture relies on several core principles:

*   **Service Pattern (Encapsulation & Abstraction):** Business logic (borrowing/returning logic, availability checks) is entirely decoupled from the API Controllers. It is encapsulated within the `LibraryOperationsService`, which is injected via an interface (`ILibraryOperationsService`).
*   **Entity Framework Core (Code-First):** The database schema is generated directly from the C# Domain Models (`Book`, `Member`, `Loan`), ensuring a single source of truth for the data structure.
*   **Dependency Injection (DI):** Services and the Database Context are registered in the DI container within `Program.cs`, promoting loose coupling and testability.
*   **Data Transfer Objects (DTOs):** Client requests are mapped to DTOs (e.g., `BorrowRequestDto`) rather than exposing the core database entities directly to the presentation layer.

## Database Design
The relational model utilizes a junction table approach to track historical borrowing data:
*   `Books` (One-to-Many with Loans)
*   `Members` (One-to-Many with Loans)
*   `Loans` (Junction table linking Books and Members, tracking `BorrowDate` and `ReturnDate`)

*Note: The project is currently configured to use the EF Core `InMemory` database provider to allow for immediate, frictionless testing upon cloning.*

## API Endpoints
The API is documented and testable via Swagger UI. The primary operational endpoints include:

*   `POST /api/library/borrow` - Accepts a `BookId` and `MemberId`. Validates book availability and creates a loan transaction.
*   `POST /api/library/return/{loanId}` - Processes a book return and updates the asset's availability status.

## Prerequisites to Run
*   .NET 8.0 SDK (or higher)
*   Visual Studio 2022 (or VS Code with C# Dev Kit)

## How to Run the Project Locally
1. Clone the repository to your local machine.
2. Open the `.sln` file in Visual Studio.
3. Press **F5** (or run `dotnet run` via CLI) to build and launch the application.
4. The application will automatically launch the **Swagger UI** in your default web browser, allowing you to execute API requests and test the library operations directly.
