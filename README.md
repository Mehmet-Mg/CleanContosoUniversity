# 🚀 Clean Contoso University - ASP.NET Core Razor Pages

This project is developed using **Clean Architecture** principles with ASP.NET Core Razor Pages. The goal is to provide a layered, testable, and maintainable structure.

## 🌟 Architecture and Technologies

The following core technologies and architectural patterns are used in this project:

* **Architecture:** Clean Architecture
* **Web Framework:** ASP.NET Core 7/8+ Razor Pages
* **Database:** [Database Used, e.g., SQL Server]
* **ORM:** Entity Framework Core
* **Other Libraries:** [Other important libraries used, e.g., MediatR, FluentValidation]

## 📋 Prerequisites

The following software must be installed to run the project locally:

* [.NET SDK] ([Used Version, e.g., 7.0 or 8.0])
* [Database Software, e.g., SQL Server LocalDB or Docker]
* [Preferred IDE, e.g., Visual Studio 2022 or VS Code]

## ⚙️ Project Setup

### 1. Cloning

Clone the project to your local machine:

```bash
git clone [REPOSITORY_URL]
cd [YOUR-PROJECT-FOLDER-NAME]
```

```bash
dotnet ef migrations add [MigrationAdı] --startup-project .\src\Web\ --project .\src\Infrastructure\
dotnet ef database update --startup-project .\src\Web\ --project .\src\Infrastructure\
```