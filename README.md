# Inventory Management System

A web-based inventory management system built with **.NET 6**, using **Entity Framework Core**, **SignalR**, and **NUnit** for integration testing. This system allows users to manage products, suppliers, transactions (sales and purchases), and generate CSV/PDF reports for better decision-making.

## Features

- ✅ Authentication and Authorization (with Administrator role)
- 📦 Manage Categories, Suppliers, Products
- 🔄 Track Transactions (Sales and Purchases)
- 📊 Real-time stock updates using SignalR
- 📁 Export reports in **CSV** and **PDF** formats
- 🧪 Unit tests using **NUnit**

## Tech Stack

- **Backend**: .NET 6, ASP.NET Core
- **Database**: Entity Framework Core (Code-First)
- **Testing**: NUnit
- **Real-time**: SignalR
- **Reports**: CSV and PDF generation (using CsvHelper and QuestPDF)

## Database Schema

- `Categories`
- `Suppliers`
- `Products`
- `Transactions`

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or your preferred DB

### Setup Instructions

1. **Clone the Repository**
2. **Run migrations for tables creation**
```bash
dotnet ef migrations add InitialMigration --context DataContext --project MariamApp.Data --startup-project MariamApp
dotnet ef database update  --project MariamApp.Data --startup-project MariamApp --context DataContext 

dotnet ef migrations add AddIdentityTables --context AppUsersDbContext --project MariamApp.Data --startup-project MariamApp
dotnet ef database update  --project MariamApp.Data --startup-project MariamApp --context AppUsersDbContext
```
3. **Build and run**

The AspNetUsers and AspNetRoles tables will be automatically created and initialized when the application starts.
Additionally, two default users — Administrator and Moderator — will be created with the following credentials:

<pre> 
Administrator
username: admin
password: Admin040325*

Moderator
username: moderator
password: Moderator040325*
</pre>

In the project, only Administrator users are allowed to use delete endpoints and download CSV files. All other actions require authorization for authenticated users.

### Usage of SignalR
For testing SignalR need to open this URLs.
1. **http://localhost:5210/src/reports/index.html**
In this page you can see messages when your file is ready for download.
2. **http://localhost:5210/src/stock/index.html**
In this page you can see stock state after any transaction.