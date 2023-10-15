# CDN API

## Introduction
This project is an assessment to demonstrate my understanding and skills in C#, ASP.NET Core, RESTful APIs. The main functionality includes user management where users can be created, retrieved, updated, and deleted from a database.

## Prerequisites
- .NET Core SDK v7.0.402

## Database
- **SQL Server**: The relational database used.

## Clean Architecture
### Core Layer: User Entity
- the innermost circle in Clean Architecture
- independent of external influences
- represents a user within the system.
- Attributes: `Id, Username, Password, Email, PhoneNumber, Skillsets, Hobbies`.

### Infrastructure Layer: UserRepository (IUserRepository)
- outside of the Core layer.
- acts as a bridge between the core business logic and data access mechanisms.
- CRUD (Create, Read, Update, Delete) operations for the `User` entity
- implements the `IUserRepository` interface for abstract the operations

### Application Layer: UserService (IUserService)
- sits between the Infrastructure and Api layers
- execute business operations and apply specific business rules
- applies necessary transformations on user data before or after it's retreived
- implements the `IUserService` interface for abstract the operations
    
### Api Layer: UserController (IUserController)
- the outermost layer, handling HTTP requests
- translate HTTP request into data and pass to UserService (Application Layer) to proceed the next operations
- implements the `IUserController` interface for abstract the operations
