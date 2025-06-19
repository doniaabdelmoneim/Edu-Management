# EDU- Student Management System with Admin Dashboard

![EDU Banner](./wwwroot/assets/home.png) 


<div align="center">
  <img src="./wwwroot/assets/department.png/" alt="Department Preview" width="70%">
  <p><em>Department Interface</em></p>
</div>

## 📝 Enhanced Overview

EDU is a comprehensive school management system featuring robust authentication/authorization and an admin dashboard, built with ASP.NET Core MVC.

## 🆕 Key Features

### 🔐 Authentication & Authorization
<div align="center">
  <img src="./wwwroot/assets/login.png/" alt="Login Screen" width="50%">
  <img src="./wwwroot/assets/register.png/" alt="Registration Screen" width="50%">

</div>

- Role-based access control (Admin, Staff, Student)
- Secure password hashing
- Password reset functionality
- Session management

## 👥 Roles & Permissions

| Role       | Permissions | 
|------------|-------------|
| Admin      | Full system access |
| Staff      | Manage students | 
| Student    | View profile |

## 🛠️ Technology Stack

<div align="center">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET">
  <img src="https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white" alt="SQL Server">
  <img src="https://img.shields.io/badge/Bootstrap-563D7C?style=for-the-badge&logo=bootstrap&logoColor=white" alt="Bootstrap">
</div>

## 🚀 Getting Started

### Prerequisites
- .NET 6.0 SDK
- SQL Server 2019+
- Node.js (for frontend dependencies)

### Installation
```bash
# Clone the repository
git clone https://github.com//ProjectX.git
cd ProjectX

# Install required packages
dotnet add package Microsoft.AspNetCore.Identity.EntityFrameworkCore
dotnet add package Microsoft.AspNetCore.Identity.UI

# Run migrations
dotnet ef database update

### 📄 License
MIT License - See LICENSE for details.

Developed with ❤️ by Donia