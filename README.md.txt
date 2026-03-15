# Overclocked

An online gaming store built with React, .NET 8 and SQL Server.

## Tech Stack

- Frontend: React + TypeScript (Vite)
- Backend: .NET 8 Web API
- Database: SQL Server LocalDB
- Auth: ASP.NET Identity + JWT
- Logging: Serilog
- Testing: xUnit, Playwright, NBomber

## Requirements

- Visual Studio 2022
- .NET 8 SDK
- Node.js
- SQL Server LocalDB (comes with Visual Studio)

## Setup

1. Clone the repo and open `Overclocked.sln` in Visual Studio
2. Right click the solution, go to Configure Startup Projects and set the following to Start:
   - Overclocked.API
   - Overclocked.Products.API
   - Overclocked.Orders.API
3. Hit Run — the frontend and databases are set up automatically

## URLs

- Frontend: http://localhost:5173
- Main API Swagger: http://localhost:5256/swagger
- Products API Swagger: http://localhost:5001/swagger
- Orders API Swagger: http://localhost:5002/swagger

## Running Tests

Make sure the app is running first, then in a separate terminal:
```bash
cd Overclocked.Tests
dotnet test --verbosity normal
```

## Notes

- Register an account to access the cart
- The homepage shows featured products only, all products are on the Products page
- Each microservice has its own database
- Logs are saved to the `logs/` folder inside each API project and are also printed to the console when running