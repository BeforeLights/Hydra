# HYDRA

![z6839202535302_5ccedf89c6b52d7deafcb4309ca9a045](https://github.com/user-attachments/assets/97f48240-2b96-48ff-8ed6-81619ad5f118)

**HYDRA** is a desktop application for Windows, built with WPF, that serves as an all-in-one game store and personal library manager. Inspired by platforms like Steam, this project is built using a clean 3-Layer Architecture to demonstrate best practices in application design and development.

## 🌟 Features

* **Game Store**: Browse a catalog of games available for purchase.
    * Filter games by genre, platform, and other criteria.
    * View detailed game pages with descriptions, screenshots, pricing, and system requirements.
* **Shopping Cart**: Add games to a persistent shopping cart before purchasing.
* **User Accounts & Authentication**: Secure user registration and login system.
* **Role-Based Access Control**:
    * **Customer**: Can browse the store, purchase games, and manage their personal library.
    * **Manager**: Can manage the game catalog (add/edit/remove games) and view customer data.
    * **Admin**: Has full control over the entire application, including user and role management.
* **Personal Game Library**: After purchasing a game, it is automatically added to the user's private library.
    * Track game status (e.g., Playing, Backlog, Completed).
    * Add personal notes and ratings for each game.

## 🛠️ Tech Stack & Architecture

This project emphasizes a clean and scalable structure.

### **Technology**

* **Framework**: .NET 6 (or later)
* **Frontend**: Windows Presentation Foundation (WPF)
* **Database**: Microsoft SQL Server
* **Data Access**: Dapper (high-performance micro-ORM)
* **Language**: C#

### **Architecture**

The solution is built on a classic **3-Layer Architecture** to ensure separation of concerns:

1.  **HYDRA.GUI (Presentation Layer)**
    * The WPF application itself.
    * Responsible for all UI elements and user interaction.
    * Contains no business logic; it simply displays data and captures user input.

2.  **HYDRA.BLL (Business Logic Layer)**
    * The "brains" of the application.
    * Handles all business rules, data validation, and authorization logic (e.g., "Is this user an Admin?").
    * Acts as the intermediary between the GUI and the DAL.

3.  **HYDRA.DAL (Data Access Layer)**
    * The only layer that communicates directly with the database.
    * Responsible for executing SQL queries to perform CRUD (Create, Read, Update, Delete) operations.
    * Uses Dapper to map database results to C# model classes.

## 📦 Database Schema

The project uses a relational database schema designed to support all the features mentioned above. The core tables include `Users`, `Roles`, `Games`, `Orders`, `LibraryEntries`, and `ShoppingCarts`.

The complete `CREATE TABLE` script can be found in the `/database` folder of this repository.

## 🚀 Getting Started

To get a local copy up and running, follow these simple steps.

### **Prerequisites**

* Visual Studio 2022
* .NET 6 SDK (or later)
* SQL Server (Express, Developer, or full version)

### **Installation**

1.  **Clone the repository:**
    ```sh
    git clone [https://github.com/your_username/HYDRA.git](https://github.com/your_username/HYDRA.git)
    ```
2.  **Set up the database:**
    * Open SQL Server Management Studio (SSMS).
    * Run the `CREATE_HYDRA_DB.sql` script to create the `HYDRA` database and all its tables.
3.  **Configure the connection string:**
    * Open the solution in Visual Studio.
    * In the `HYDRA.GUI` project, open the `App.config` file.
    * Update the `connectionString` with your SQL Server instance name.
4.  **Build and run the application:**
    * Build the solution (this will restore the NuGet packages).
    * Set `HYDRA.GUI` as the startup project and press F5 to run.

## 🚧 Project Status

This project is currently **in development**. New features and refinements are being added.
