<!-- ABOUT THE PROJECT -->
# Real Estate App

This real estate application is designed to manage different roles, each with distinct functionalities and interactions within the system. It incorporates a RESTful API to consume all available features.
The application is developed using the ONION architecture in C# ASP .NET Core and adheres to SOLID principles.

## Technologies Used

- **Backend**
  - C# ASP.NET Core MVC (6.0)
  - Microsoft Entity Framework Core (Code First approach)
      - Microsoft.EntityFrameworkCore.Relational
      - Microsoft.EntityFrameworkCore.SqlServer
      - Microsoft.EntityFrameworkCore.Tools
      - Microsoft.EntityFrameworkCore.Design
  - Microsoft Identity for user management
  - AutoMapper
  - Microsoft.AspNetCore.Identity.EntityFrameworkCore
  - Microsoft.Extensions.Options.ConfigurationExtensions
  - Microsoft.AspNetCore.Mvc.NewtonsoftJson
  - Microsoft.Extensions.DependencyInjection
  - Swashbuckle.AspNetCore.Swagger
  - Asp.Versioning.Mvc
  - CQRS Pattern
  - Mediator Pattern

- **Frontend**
  - HTML
  - CSS
  - Bootstrap
  - ASP.NET Razor

- **ORM**
  - Entity Framework

- **Database**
  - SQL Server

## Getting Started

### Prerequisites
To run this project, you'll need:
- Visual Studio with ASP.NET Core SDK (6 onwards)
- SQL Server

### Installation
1. Clone the repository or download the project.
2. Open the project in Visual Studio.
3. Update the database connection string in `appsettings.json` to match your SQL Server setup.
4. Update the mail settings in `appsettings.json` with your SMTP server details for email functionalities:
   ```json
   "MailSettings": {
    "EmailFrom": "your-email@example.com",
    "SmtpHost": "smtp.gmail.com",
    "SmtpPort": 587,
    "SmtpUser": "your-email@example.com",
    "SmtpPass": "your-smtp-password",
    "DisplayName": "Social Network mail"
   }
5. Open Package Manager Console in Visual Studio and run `Update-Database` to apply migrations.
6. Run the project and access it in your browser.

## Project Screenshots

* Home Page (Properties)

![Home Page](https://github.com/AleGxrcia/Real-Estate-App/blob/main/RealEstateApp.WebApp/wwwroot/ProjectImages/HomePage.png)

* Agents

![Friends Page](https://github.com/AleGxrcia/Real-Estate-App/blob/main/RealEstateApp.WebApp/wwwroot/ProjectImages/Agents.png)

* Agent Property Management

![Property Management](https://github.com/AleGxrcia/Real-Estate-App/blob/main/RealEstateApp.WebApp/wwwroot/ProjectImages/RegisterProperty.png)

* Admin Dashboard

![Admin Dashboard](https://github.com/AleGxrcia/Real-Estate-App/blob/main/RealEstateApp.WebApp/wwwroot/ProjectImages/AdminDashboard.png)
  
## Contributors
- [Alejandro](https://github.com/AleGxrcia)
- [Abner](https://github.com/Msnloco247)
- [Frank](https://github.com/frankjrx)

