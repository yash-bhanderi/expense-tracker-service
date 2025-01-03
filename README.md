# expense-tracker-service

## Overview

The **Expenses Tracker** backend service is built using **C#** and **.NET Core**. It provides a RESTful API that allows users to track their personal expenses, categorize them, and retrieve detailed information on their spending habits. The service is responsible for managing all operations related to expenses, including adding, editing, deleting, listing, and categorizing expenses. It also supports full-text search and provides summary data by category.

This service is designed to serve as the backend for a web application that helps users keep track of their expenses in a secure and organized manner. The backend handles all business logic, database interactions, and API responses. It interacts with a SQL-based database (SQL Server) to store and retrieve data.


## Developing

### Built With 

* [net8.0.0](https://dotnet.microsoft.com/download/dotnet/8.0)
* [AutoMapper.Extensions.Microsoft.DependencyInjection] (https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/12.0.1)
* [BCrypt.Net-Next](https://www.nuget.org/packages/BCrypt.Net-Next/4.0.3)
* [Microsoft.AspNetCore.Authentication.Google](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.Google/8.0.7)
* [Microsoft.AspNetCore.Authentication.JwtBearer](https://www.nuget.org/packages/Microsoft.AspNetCore.Authentication.JwtBearer/8.0.4)
* [Microsoft.AspNetCore.OpenApi](https://www.nuget.org/packages/Microsoft.AspNetCore.OpenApi/8.0.11)
* [Microsoft.EntityFrameworkCore.SqlServer](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.SqlServer/9.0.0)
* [Microsoft.EntityFrameworkCore.Tools](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Tools/9.0.0)
* [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/6.6.2)

## Features

### 1. Add/Edit/Delete Expense Entries
- Users can add new expense entries, edit them, or delete them.
- Each expense will include details such as the amount, date, and description.

### 2. List Expense Entries
- A list of all expense entries will be displayed.
- The entries will include relevant details such as amount, date, category, and description.

### 3. Categorize Expenses
- Expenses will be categorized into predefined types such as:
  - Food
  - Travel
  - Utilities
  - Entertainment
  - Others

### 4. Summary by Category
- A summary view will show total amounts spent in each category.
- This will help users analyze their spending patterns across different categories.

### 5. Search Functionality
- Full-text search capabilities will be available, enabling users to search through expense descriptions, categories, and other details.
- Semantic search will be implemented to allow more natural language queries (e.g., "how much did I spend on food last month?").

### 6. Data Visualizations
- Data will be visualized using pie charts and bar charts to represent:
  - Expense distribution by category.
  - Total expenses over time.

## Technologies Used

- **C# (.NET Core)**: The back-end API is developed using C# and .NET Core for building RESTful services.
- **SQL Database (SQL Server)**: The application will store expense entries and user data in a relational database.
- **Entity Framework Core**: ORM for interacting with the database.

## Getting Started

### Prerequisites

- **.NET Core SDK**: Ensure you have .NET Core 8.0 or later installed on your machine.
- **SQL Database**: Install SQL Server for storing expense data.

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yash-bhanderi/expense-tracker-service.git
   cd expense-tracker-service