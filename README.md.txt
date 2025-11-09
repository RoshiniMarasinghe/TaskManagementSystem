# Task Management System

A simple REST API built with **.NET 8** and **Entity Framework Core (SQL Server)** for managing tasks.  
The project demonstrates clean architecture with **Repository–Service–Controller** layers and includes **unit tests** using xUnit and Moq.

**Repository:** [https://github.com/RoshiniMarasinghe/TaskManagementSystem](https://github.com/RoshiniMarasinghe/TaskManagementSystem)

---

## Overview

This API allows users to:
- Create, read, update, and delete tasks  
- Assign task priority and status  
- Track creation and update timestamps  
- Validate inputs such as title, description, and due date  

The goal of this project is to show practical use of EF Core, async programming, DTOs, and clean separation of responsibilities.

---

## Technologies Used

- .NET 8 Web API  
- Entity Framework Core  
- SQL Server LocalDB  
- xUnit  
- Moq  
- Git / GitHub  

---

## Project Structure

TaskManagement/
├── Controllers/
│ └── TasksController.cs
├── Services/
│ ├── ITaskService.cs
│ └── TaskService.cs
├── Repositories/
│ ├── ITaskRepository.cs
│ └── TaskRepository.cs
├── Data/
│ └── AppDbContext.cs
├── Dtos/
│ └── TaskDtos.cs
├── Models/
│ ├── TaskItem.cs
│ └── Enum/
│ ├── StatusEnum.cs
│ └── PriorityEnum.cs
├── TaskManagement.Tests/
│ └── Unit/
│ └── TaskServiceTests.cs
└── Program.cs
---

## Getting Started

### Clone the repository

git clone https://github.com/RoshiniMarasinghe/TaskManagementSystem.git
cd TaskManagementSystem

Configure the database

The project uses SQL Server LocalDB by default.
appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=TaskyDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
}

Apply migrations
dotnet ef database update

Run the project
dotnet run

Then open Swagger at:
https://localhost:<port>/swagger

Running Tests
Unit tests are located in the TaskManagement.Tests project.

Run tests with:
dotnet test

These tests cover:
Validation of create logic (e.g., due date cannot be in the past)
GetById behaviours (success / not found)
Partial update logic
Delete behaviours
Tests use xUnit with Moq to mock the ITaskRepository.

Notes
DTOs are used to separate input models from database entities.
Enums are stored as readable strings in the database.
ServiceResult<T> standardizes API responses with Success, Error, and Data.

The project follows a clean and maintainable structure for real-world backend development.

Future Enhancements
Add authentication & authorization
Introduce integration tests with in-memory databases
