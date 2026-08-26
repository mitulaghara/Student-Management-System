<div align="center">

# 🎓 Student Management System (SMS)
### *A Modern, Enterprise-Grade Academic ERP Portal*

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue?style=for-the-badge&logo=csharp&logoColor=white)](https://dotnet.microsoft.com/apps/aspnet/mvc)
[![MongoDB](https://img.shields.io/badge/MongoDB-Atlas%20Cloud-47A248?style=for-the-badge&logo=mongodb&logoColor=white)](https://www.mongodb.com/)
[![Bootstrap 5](https://img.shields.io/badge/Bootstrap-5.3-7952B3?style=for-the-badge&logo=bootstrap&logoColor=white)](https://getbootstrap.com/)
[![License](https://img.shields.io/badge/License-MIT-yellow?style=for-the-badge)](LICENSE)

<p align="center">
  <b>B.Tech Computer Science & Engineering — Semester 5 (.NET Technologies)</b><br>
  <i>Faculty of Engineering and Technology, Marwadi University</i>
</p>

---

</div>

## 📖 Table of Contents
- [📌 Overview & Problem Statement](#-overview--problem-statement)
- [✨ Key Features](#-key-features)
- [🛠️ System Architecture & Tech Stack](#️-system-architecture--tech-stack)
- [📂 Project Directory Structure](#-project-directory-structure)
- [🚀 Quick Start & Installation](#-quick-start--installation)
- [🗄️ Database Configuration](#️-database-configuration)
- [🔐 Access Control & Security](#-access-control--security)
- [📊 Modules Breakdown](#-modules-breakdown)
- [👥 Authors & Acknowledgments](#-authors--acknowledgments)

---

## 📌 Overview & Problem Statement

Academic institutions often face data redundancy, fragmented communication channels, and difficulty in real-time reporting due to legacy, manual record systems.

The **Student Management System (SMS)** is an end-to-end web portal engineered with **ASP.NET Core MVC** and **C#**. It streamlines institutional workflows by unifying Department administration, Course scheduling, Classroom logistics, Faculty rosters, Student admissions, and Advisor-Student Mentorship tracking within a single interactive dashboard.

---

## ✨ Key Features

| Feature | Description |
| :--- | :--- |
| 🛡️ **Session-Based Authentication** | Custom authorization filter (`[CheckAccess]`) protecting secured routes with session tokens and Anti-Forgery tokens. |
| 🏢 **Department Management** | Full CRUD capabilities for academic departments, intake capacity, and contact heads. |
| 👨‍🏫 **Faculty & Staff Directory** | Comprehensive staff directory tracking designations, contact information, and department affiliations. |
| 🏫 **Classroom & Lab Allocation** | Monitor facility capacity, room numbers, and laboratory designations. |
| 📚 **Course Catalog** | Manage syllabus codes, credit weights, and course outlines. |
| 🎓 **Student Lifecycle Tracking** | Manage roll numbers, personal details, date of birth, and enrollment statuses. |
| 🤝 **Faculty-Student Advising** | Dynamic mentor-mentee mapping module with active status indicators and progress remarks. |
| 📈 **Visual Analytics Dashboard** | Real-time counters, status badges, and interactive Google Charts for department-wise student distribution. |

---

## 🛠️ System Architecture & Tech Stack

```mermaid
graph TD
    User([🌐 Browser Client]) <--> UI[Bootstrap 5 / Razor Views]
    UI <--> Controller[ASP.NET Core MVC Controllers]
    Controller <--> Filter[🔐 CheckAccess Authorization Filter]
    Controller <--> Service[Services Layer / MongoDbService]
    Service <--> DB1[(🍃 MongoDB Atlas Cloud)]
    Service -.-> DB2[(🗄️ SQL Server / ADO.NET)]
```

### 💻 Technology Breakdown

- **Backend / Web Layer**: ASP.NET Core MVC (.NET 8.0), C# 12
- **Data Persistence**:
  - **MongoDB Atlas** (Cloud NoSQL via `MongoDB.Driver 2.28.0`)
  - **SQL Server** (`Microsoft.Data.SqlClient 5.2.2` with Stored Procedures)
- **Frontend / Presentation**:
  - HTML5, CSS3, JavaScript (ES6)
  - **Bootstrap 5.3**, **NiceAdmin UI**, **Bootstrap Icons**, **FontAwesome 6**
  - **ApexCharts**, **Chart.js**, and **Google Charts API**

---

## 📂 Project Directory Structure

```text
Student Management System Using dotNET/
├── 📁 Controllers/              # MVC Controllers (Auth, Student, Staff, Course, etc.)
├── 📁 Models/                   # Data Models & ViewModels (Department, Student, etc.)
├── 📁 Views/                    # Razor View Templates (.cshtml)
│   ├── 📁 Auth/                 # Login & Registration Pages
│   ├── 📁 Home/                 # Dashboard with Analytics & Charts
│   ├── 📁 Student/              # Student List & Add/Edit Forms
│   ├── 📁 Staff/                # Faculty Directory & Form
│   ├── 📁 Course/               # Course Management Views
│   ├── 📁 Department/           # Department Management Views
│   ├── 📁 Classroom/            # Classroom Allocation Views
│   ├── 📁 Enrollment/           # Student-Faculty Advising Views
│   └── 📁 Shared/               # _Layout, Navbar, Sidebar & Partials
├── 📁 Filters/                  # Custom Authorization Filters (CheckAccess.cs)
├── 📁 Services/                 # Business & Database Service Layer (MongoDbService.cs)
├── 📁 wwwroot/                  # Static Assets (CSS, JS, Vendor Libraries, Images)
├── 📄 appsettings.json          # Configuration & Connection Strings
├── 📄 Program.cs                # Application Startup & Middleware Pipeline
├── 📄 StudentManagementSystem.csproj
└── 📄 README.md
```

---

## 🚀 Quick Start & Installation

### 📋 Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher
- [Visual Studio 2022](https://visualstudio.microsoft.com/) / [VS Code](https://code.visualstudio.com/) / JetBrains Rider
- Active Internet Connection (for MongoDB Atlas cloud connection)

### ⚙️ Step-by-Step Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/mitulaghara/Student-Management-System.git
   cd "Student Management System Using dotNET"
   ```

2. **Restore Dependencies & Build**
   ```bash
   dotnet restore
   dotnet build
   ```

3. **Run the Application**
   ```bash
   dotnet run
   ```

4. **Open in Browser**
   Navigate to: `http://localhost:5062`

---

## 🗄️ Database Configuration

### Option A: MongoDB Atlas (Default & Cloud-Ready)
The application connects automatically to MongoDB Atlas using the configured connection string in `appsettings.json`. Database collections and seed records are automatically initialized on the first application launch.

```json
"MongoDbSettings": {
  "ConnectionString": "mongodb+srv://<username>:<password>@cluster.mongodb.net/?retryWrites=true&w=majority",
  "DatabaseName": "StudentManagementDB"
}
```

### Option B: Microsoft SQL Server (ADO.NET Stored Procedures)
1. Execute `StudentManagementSystem.sql` in SQL Server Management Studio (SSMS) to create the schema and stored procedures.
2. Update the `DefaultConnection` string in `appsettings.json`.

---

## 🔐 Access Control & Security

- **Default Administrator Credentials**:
  - **Username**: `admin`
  - **Password**: `admin123`
- **Route Guarding**: All operational modules are protected using the `[CheckAccess]` action filter.
- **CSRF Mitigation**: HTML forms utilize ASP.NET Core built-in Anti-Forgery Tokens (`@Html.AntiForgeryToken()`).

---

## 📊 Modules Breakdown

```
┌─────────────────────────────────────────────────────────────┐
│                 STUDENT MANAGEMENT PORTAL                   │
├───────────────┬─────────────────────────────┬───────────────┤
│ Administration│      Academic Operations    │   Analytics   │
├───────────────┼─────────────────────────────┼───────────────┤
│ • Departments │ • Student Admissions        │ • Enrollment  │
│ • Faculty     │ • Course Catalog            │   Statistics  │
│ • Classrooms  │ • Advisor Allocations       │ • Visual Pie  │
│ • Security    │ • Active/Inactive Tracking  │   Charts      │
└───────────────┴─────────────────────────────┴───────────────┘
```

---

## 👥 Team Members & Contributors

| Name | Role | GitHub Profile |
| :--- | :--- | :--- |
| **Mitul Aghara** | Lead Developer & Architect | [@mitulaghara](https://github.com/mitulaghara) |
| **Krisha** | Core Contributor | [@Krisha15607](https://github.com/Krisha15607) |
| **Harsh Zolapara** | Core Contributor | [@harshzolapara144295-dotcom](https://github.com/harshzolapara144295-dotcom) |

### 🏛️ Institution
- **Marwadi University** — *Faculty of Engineering and Technology (Department of Computer Engineering)*

---

<div align="center">
  <sub>© 2026 <b>Student Management System</b>. Built for .NET Technologies Academic Project.</sub>
</div>
