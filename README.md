# Student Management System (SMS)

> **Course**: .NET Technologies (01CE1523) – B.Tech Semester 5 (CSE)  
> **Institution**: Marwadi University – Faculty of Engineering and Technology (Department of Computer Engineering)  

---

## 📌 Project Overview & Problem Statement

Traditional academic record-keeping in educational institutions often relies on fragmented manual processes, leading to data redundancy, delays in course enrollment tracking, and inefficient faculty-student mentorship monitoring. 

The **Student Management System (SMS)** is a modern, enterprise-grade web application built using **ASP.NET Core MVC** and **C#**. It provides a unified digital portal to manage Departments, Academic Courses, Classrooms, Faculty Members, Student Enrollment Profiles, and Advising Assignments with real-time graphical analytics.

---

## 🎯 Objectives

1. **Centralized Data Management**: Digitize and maintain records for Departments, Staff, Courses, Classrooms, Students, and Mentorship Enrollments.
2. **Secure Session-Based Authentication**: Enforce custom `[CheckAccess]` authorization filter for protected portal areas.
3. **Hybrid Database Architecture**: Support **SQL Server via ADO.NET Stored Procedures** as well as **MongoDB Atlas Cloud Database**, with automated mock fallback for offline development.
4. **Rich Interactive Analytics**: Provide an executive dashboard with key metric cards and interactive Google Pie Charts.
5. **Responsive Modern UI**: Built with Bootstrap 5, FontAwesome, Bootstrap Icons, and the NiceAdmin dashboard template.

---

## 💻 Technology Stack

- **Core Framework**: C# (.NET 10.0), ASP.NET Core MVC
- **Data Access & Storage**:
  - **ADO.NET** (`System.Data.SqlClient`) with SQL Server Stored Procedures (`StudentManagementSystem.sql`)
  - **MongoDB Atlas** NoSQL Cloud Database (`MongoDB.Driver`)
- **Frontend & UI**: HTML5, Vanilla CSS3, Bootstrap 5.3, FontAwesome 6, Bootstrap Icons, Google Charts API
- **Version Control**: Git & GitHub

---

## ⚙️ Key Features & Modules

1. **Authentication & Session Control**:
   - Secure login portal with Anti-Forgery Token protection.
   - Dynamic user profile header displaying active session details.

2. **Department Management**:
   - Complete CRUD operations for engineering/academic departments.

3. **Faculty / Staff Directory**:
   - Manage faculty profiles, designations, mobile numbers, emails, and department associations.

4. **Classroom & Lab Allocation**:
   - Track lab allocations, seminar halls, and lecture classrooms.

5. **Course & Academic Program Management**:
   - Manage offered courses, syllabus remarks, and course codes.

6. **Student Record Management**:
   - Maintain student enrollment records, roll numbers, contact details, birth dates, assigned classrooms, and active/dropped status.

7. **Student-Faculty Advising & Mentorship**:
   - Map students to dedicated faculty advisors with status tracking and remarks.

8. **Executive Dashboard**:
   - Summary counter widgets and a dynamic Google Chart for department-wise student distribution.

---

## 🚀 Installation & Execution Steps

### Prerequisites
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 / VS Code
- (Optional) SQL Server Management Studio (SSMS) or MongoDB Atlas Account

### Quick Start (Command Line)
1. **Clone the repository**:
   ```bash
   git clone <your-github-repo-url>
   cd "Student Management System Using dotNET"
   ```
2. **Restore dependencies & Build**:
   ```bash
   dotnet restore
   dotnet build
   ```
3. **Run the Application**:
   ```bash
   dotnet run
   ```
4. **Access in Browser**:
   Open [http://localhost:5062](http://localhost:5062) in your browser.

5. **Default Login Credentials**:
   - **Username**: `admin`
   - **Password**: `admin123`

---

## 🗄️ Database Setup Options

### Option A: MongoDB Atlas Cloud (Active Default)
The application is pre-configured in `appsettings.json` with a MongoDB Atlas cloud connection string. Collections and seed data are automatically initialized on startup.

### Option B: SQL Server (ADO.NET)
Open SQL Server Management Studio (SSMS) and execute the included [StudentManagementSystem.sql](file:///Users/mitulaghara/Desktop/Student%20Management%20System%20Using%20dotNET/StudentManagementSystem.sql) script to create the database and all stored procedures.

---

## 👥 Team Members

- **Mitul Aghara** (Lead Developer)
- **Team Member 2**
- **Team Member 3**

---

## 📜 License & Copyright

© 2026 **Student Management System**. All Rights Reserved.  
Designed & Developed by **Mitul Aghara**.
