# Task Manager - ASP.NET Core MVC Application

## 📚 Project Overview

This is a **Task Manager** web application built using **ASP.NET Core MVC** (.NET 8.0).  
The project follows a clean **3-layer architecture** (Controller → Interface → Service) and demonstrates CRUD operations, user authentication, task categorization, and task management.

---

## 🛠 Technologies Used

- **ASP.NET Core MVC (.NET 8.0)**
- **Entity Framework Core 9.0.4** (SQL Server database)
- **Microsoft.AspNetCore.Identity** (User authentication)
- **Microsoft.AspNetCore.Authentication.Cookies** (Session management)
- **Dependency Injection (DI)**
- **Bootstrap 5** (Frontend styling)

---

## 📦 Project Structure


---

## 🚀 Features

- ✅ User Registration and Login (Session-based Authentication)
- ✅ Create, Read, Update, Delete (CRUD) tasks
- ✅ Task priority selection (High, Medium, Low)
- ✅ Category management for tasks
- ✅ Task search functionality by Title
- ✅ View tasks by status (Pending / Completed)
- ✅ Secure APIs and Model Validation
- ✅ Proper Layered Architecture (Separation of Concerns)

---

## 🔧 Setup Instructions

### Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or any database)

### Steps

1. **Clone the repository**

```bash
git clone https://github.com/your-username/TaskManager.git
cd TaskManager

"ConnectionStrings": {
  "DefaultConnection": "Server=your_server_name;Database=TaskManagerDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

dotnet ef database update

dotnet run

https://localhost:5001

🧪 Challenges Faced
Binding Enums to Dropdowns: Required correct SelectList binding with null handling.

Handling Nullable Fields: Needed to align database nullable types with DTOs.

Authentication: Session-based login management with ASP.NET Core Identity took careful configuration.


API Documentation

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

This project is part of the Dotnet Developer Test Kit assignment, demonstrating full-stack web application development using ASP.NET Core MVC and Entity Framework.
