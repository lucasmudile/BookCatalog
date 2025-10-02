# BookCatalog
Book Catalog is a simple project made in Angular and .Net Core 




# 📚 BookCatalog - Book Catalog System

A complete book catalog management system with backend in .NET 8 and frontend in Angular.

# 🚀 Running with Docker (Recommended)
Prerequisites

 * Docker Desktop installed and running

 * Git (to clone the repository)

# How to Run

# 1. Clone the repository

git clone https://github.com/helton-evambi/Desafio-Ajaxti.git
cd Desafio-Ajaxti


# 2. Run with Docker Compose

docker compose up --build


# 3. Access the applications

      Frontend (Angular): (http://localhost:4200)
      
      Backend API: (http://localhost:8080)
      
      Swagger UI: (http://localhost:8080/swagger)

      Database: PostgreSQL on port 5432

# 🔧 Useful Docker Commands
# Stop all services
docker compose down

# Stop and remove volumes (clean data)
docker compose down -v

# View logs for all services
docker compose logs -f

# View logs for specific services
docker compose logs -f catalog.api
docker compose logs -f angular-app
docker compose logs -f catalogdb

# Rebuild only one service
docker compose up --build angular-app
docker compose up --build catalog.api

# Run in background
docker compose up -d --build

# 🛠️ Manual Execution (Development)
# Prerequisites

Backend (.NET 8)

.NET 8 SDK

PostgreSQL 15+

Frontend (Angular)

Node.js 18+

Install Angular CLI:

npm install -g @angular/cli

Database Configuration

Install PostgreSQL

Create database: BookCatalogDb

User: postgres

Password: postgres

Port: 5432

# Configure connection string

Edit the file:
backend/src/BookCatalog.API/appsettings.Development.json

{
  "ConnectionStrings": {
    "Database": "Server=localhost;Port=5432;Database=BookCatalogDb;User Id=postgres;Password=postgres;Include Error Detail=true"
  }
}

# Run Backend (.NET API)
cd backend/src/BookCatalog.API
dotnet restore
dotnet run


API: https://localhost:7051
 or http://localhost:5051

Swagger: https://localhost:7051/swagger

# Run Frontend (Angular)
cd frontend
npm install


Edit API endpoint:

frontend/src/environments/environment.ts

export const environment = {
  production: false,
  apiUrl: 'https://localhost:7051/api/v1.0', // Adjust as needed
}


Then run:

npm start
# or
ng serve


Frontend: http://localhost:4200

🐛 Troubleshooting
Docker
Port already in use
# Check which processes are using the ports
netstat -ano | findstr :4200
netstat -ano | findstr :8080
netstat -ano | findstr :5432


Stop conflicting services or change ports.

PostgreSQL volume issues
# Clean volumes and restart
docker compose down -v
docker volume prune -f
docker compose up --build

SSL Certificate error

The certificate is managed automatically in Docker

If needed, force HTTP only by removing HTTPS_PORTS

Manual
Database connection error

Make sure PostgreSQL is running

Check credentials in the connection string

Test connection with:

psql -h localhost -U postgres -d BookCatalogDb

Angular issues
# Clear cache
npm cache clean --force

# Reinstall dependencies
rm -rf node_modules && npm install


Check Node version: node --version (should be 18+)

.NET issues

Check version: dotnet --version (should be 8.0+)

Clean and rebuild:

dotnet clean && dotnet build


Check if Entity Framework is installed:

dotnet ef --version

📝 Important Notes

Docker Compose automatically configures the full infrastructure

For development, Docker is recommended for consistency

Swagger is available only in Development mode

Detailed logs are enabled in Development

# 🏗️ Architecture and Technologies
# Tech Stack
Backend - .NET 8 (Required)

# Framework: ASP.NET Core Web API

# Why: Defined as mandatory technology

# Benefits:

Excellent performance and low memory usage

Robust ecosystem with Entity Framework Core

Native support for Docker and containerization

Automatic documentation with Swagger/OpenAPI

Implements Clean Architecture

# Frontend - Angular 18

# Framework: Angular with TypeScript

# Why Angular?

Enterprise structure: Ideal for medium/large apps

Native TypeScript: Strong typing reduces bugs

Modular architecture: Reusable components, scalable structure

Mature ecosystem: Angular Material, PrimeNG, etc.

Powerful CLI: Automatic code generation, optimized builds

Integrated testing: Jasmine and Karma

PWA-ready: Native support for Progressive Web Apps

# Database - PostgreSQL

# Why PostgreSQL?

Open Source: No licensing costs

High performance: Optimized for complex workloads

ACID-compliant: Reliable transactions and data consistency

Extensible: Supports JSON, arrays, custom types

Scalable: Great for growing applications

Compatible: Works seamlessly with EF Core

Docker-friendly: Official images are well-maintained

Considered Alternatives
Frontend

React: More flexible but needs more initial setup

Database

SQL Server: Native .NET integration, but expensive license

MySQL: Popular, but fewer advanced features than PostgreSQL

🧱 System Architecture
┌─────────────────┐    HTTP/REST    ┌─────────────────┐    Entity Framework    ┌─────────────────┐
│   Angular SPA   │ ◄──────────────► │   .NET Web API  │ ◄─────────────────────► │   PostgreSQL    │
│                 │                  │                 │                         │                 │
│ - Components    │                  │ - Controllers   │                         │ - Tables        │
│ - Services      │                  │ - Business Logic│                         │ - Relationships │
│ - Routing       │                  │ - Data Access   │                         │ - Constraints   │
└─────────────────┘                  └─────────────────┘                         └─────────────────┘

📁 Project Structure
Desafio-Ajaxti/
├── backend/
│   ├── src/
│   │   ├── BookCatalog.API/          # Main API
│   │   ├── BookCatalog.Application/  # Application logic
│   │   ├── BookCatalog.Domain/       # Domain entities
│   │   ├── BookCatalog.Infrastructure/ # Data access
│   │   └── BookCatalog.Shared/       # Shared utilities
│   └── Dockerfile
├── frontend/
│   ├── src/
│   │   ├── app/                      # Angular components
│   │   ├── environments/             # Environment configs
│   │   └── assets/                   # Static assets
│   ├── Dockerfile
│   └── package.json
├── docker-compose.yml               # Docker orchestration
└── README.md

# 💡 Design Decisions
# Why Angular?

1. Enterprise-ready: Robust structure for professional applications

2. Productivity: CLI and tools speed up development

3. Maintainability: TypeScript + modular architecture

4. Ecosystem: Ready-to-use UI components (Angular Material)

5. Future-proof: LTS support and regular updates

# Why PostgreSQL?

1. Cost-effective: Open source with enterprise-grade features

2. Performance: Optimized for complex queries

3. Reliability: ACID-compliant with robust backup

4. Flexibility: Supports JSON, arrays, custom types

5. Community: Excellent documentation and active support

# 🧱 Clean Architecture (.NET)
┌─────────────────────────────────────────────────────────────┐
│                    BookCatalog.API                          │
│                  (Controllers, Middleware)                  │
├─────────────────────────────────────────────────────────────┤
│                BookCatalog.Application                      │
│            (Use Cases, DTOs, Interfaces, ViewModels)        │
├─────────────────────────────────────────────────────────────┤
│                  BookCatalog.Domain                         │
│              (Entities, Abstracts)                          │
├─────────────────────────────────────────────────────────────┤
│               BookCatalog.Infrastructure                    │
│            (Data Access,  Migrations)                       │
└─────────────────────────────────────────────────────────────┘

# 🎯 Main Endpoints

# API (.NET)
GET /api/v1.0/authors – List authors
GET /api/v1.0/books – List books
GET /api/v1.0/genres – List genres
GET /health – Health checks for backing services
Full documentation: /swagger

# Frontend (Angular)
/ – Main dashboard
/authors – Author management
/books – Book management
/genres – Genre management
