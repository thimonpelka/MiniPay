# MiniPay Platform

A comprehensive payment platform solution that enables users to manage multiple payment providers and their configurations through a modern web interface.

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Architecture](#architecture)
- [Technologies](#technologies)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Development](#development)
- [Future Improvements](#future-improvements)

## Overview

MiniPay Platform is a modular payment management system designed to streamline the integration and management of multiple payment providers. The platform consists of four main components:

- **Backend API**: RESTful API built with .NET 8 for payment provider management
- **Frontend Application**: Modern Angular-based user interface
- **SQL Database**: Persistent storage for payment configurations
- **Test Payment Service**: Simple Python service for testing payment integrations

## Features

- **Payment Provider Management**: Add, configure, and manage multiple payment providers
- **Real-time Configuration**: Update payment provider settings dynamically
- **Provider Status Control**: Activate/deactivate payment providers as needed
- **Transaction Simulation**: Test payment provider configurations using a simple Python service

_Disclaimer_: So far only a few currencies are supported, but it is very easy to add more. The current supported currencies are:

- USD (US Dollar)
- EUR (Euro)
- GBP (British Pound)
- JPY (Japanese Yen)
- AUD (Australian Dollar)
- CAD (Canadian Dollar)
- CNY (Chinese Yuan)
- INR (Indian Rupee)
- RUB (Russian Ruble)

## Architecture

The platform follows a microservices architecture with clear separation of concerns:

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Angular UI    │───▶│   .NET API      │───▶│  SQL Database   │
│   (Frontend)    │    │   (Backend)     │    │   (Storage)     │
└─────────────────┘    └─────────────────┘    └─────────────────┘
                                │
                                ▼
                       ┌─────────────────┐
                       │ Python Service  │
                       │ (Test Payments) │
                       └─────────────────┘
```

## Technologies

### Backend
- **.NET 8**: Modern C# framework for high-performance APIs
- **Entity Framework Core**: Object-relational mapping for database operations
- **ASP.NET Core**: Web API framework with built-in dependency injection
- **Swagger/OpenAPI**: API documentation and testing interface

### Frontend
- **Angular**: TypeScript-based web application framework
- **Tailwind CSS**: Utility-first CSS framework for styling

### Database
- **SQL Server**: Enterprise-grade relational database

### Testing Service
- **Python 3**: Simple and efficient scripting language
- **Flask**: Lightweight web framework for API endpoints

## Project Structure

```
MiniPayPlatform/
├── MiniPayPlatformBackend/              # Backend API (.NET 8)
│   ├── MiniPay.Application/             # Main application logic
│   │   ├── Controllers/                 # API controllers
│   │   ├── Models/                      # Data models
│   │   ├── Services/                    # Business logic
│   │   └── Data/                        # Database context
│   ├── MiniPay.Tests/                   # Unit test project
│   ├── Dockerfile                       # Dockerfile for the service
│   └── MiniPay.sln                      # Visual Studio solution
├── MiniPayPlatformFrontend/             # Angular frontend
│   ├── src/
│   │   ├── app/                         # Angular components
│   │   ├── assets/                      # Static assets
│   │   └── environments/                # Environment configs
│   ├── package.json                     # NPM dependencies
│   ├── Dockerfile                       # Dockerfile for the service
│   └── angular.json                     # Angular configuration
├── MiniPayPlatformSimplePaymentService/ # Test payment service
│   ├── src/
│   │   ├── app.py                       # Flask application
│   │   └── routes/                      # API routes
│   ├── Dockerfile                       # Dockerfile for the service
│   └── requirements.txt                 # Python dependencies
├── docker-compose.yml                   # Full stack deployment
├── docker-compose-minimal.yml           # Database only
└── README.md                            # This file
```

## Getting Started

### Prerequisites
- Docker and Docker Compose (for containerized deployment)
- .NET 8 SDK (for local backend development)
- Node.js 18+ and npm (for local frontend development)
- Python 3.8+ (for local payment service development)

### Quick Start with Docker

The easiest way to run the entire platform is using Docker Compose:

```bash
# Clone the repository
git clone <repository-url>
cd MiniPayPlatform

# Start all services
docker-compose up -d
```

This will start:
- Backend API on `http://localhost:8080`
- Frontend application on `http://localhost:4200`
- SQL Server database on `localhost:1433`
- Test payment service on `http://localhost:5000`

### Local Development Setup

If you prefer to run services individually for development:

#### 1. Database Setup
```bash
# Start only the SQL Server database
docker-compose -f docker-compose-minimal.yml up -d
```

#### 2. Backend API
```bash
# Navigate to backend directory
cd MiniPayPlatformBackend/MiniPay.Application/

# Restore dependencies and run
dotnet restore
dotnet run
```

The API will be available at `http://localhost:8080`

#### 3. Frontend Application
```bash
# Navigate to frontend directory
cd MiniPayPlatformFrontend/

# Install dependencies and start development server
npm install
npm start
```

The frontend will be available at `http://localhost:4200`

#### 4. Test Payment Service
```bash
# Navigate to payment service directory
cd MiniPayPlatformSimplePaymentService/

# Install dependencies and run
pip install -r requirements.txt
python src/app.py
```

The service will be available at `http://localhost:5000`

### Simulating Payments

To test payment provider configurations, you can use the simple payment service. It provides an endpoint to simulate a payment. In order to access it from the frontend, you need to pass the correct url of the payment provider test services.

If you run the test payment service in docker, you can use the following URL in the provider configuration:

```
http://host.docker.internal:5000/process-payment
```

If you run it locally, you can use:

```
http://localhost:5000/process-payment
```


## API Documentation

The backend API is fully documented using Swagger/OpenAPI. Once the backend is running, you can access the interactive API documentation at:

```
http://localhost:8080/swagger
```

This interface allows you to:
- Explore all available endpoints
- Test API calls directly from the browser
- View request/response schemas
- Download OpenAPI specification

## Development

### Running Tests
```bash
# Backend unit tests
cd MiniPayPlatformBackend/
dotnet test

# Frontend/Integration tests are not yet implemented
```

## Future Improvements

The following enhancements are recommend for future development

### Security & Authentication
- **User Authentication System**: Implement authentication with role-based access control (RBAC)
- **Admin vs User Roles**: Different permission levels for administrators and regular users
- **Rate Limiting**: Prevent API abuse with configurable rate limits

### Backend Enhancements
- **URL Validation**: Implement comprehensive URL validation for payment provider endpoints with automatic connectivity testing during provider creation
- **Integration Tests**: Add comprehensive integration tests covering API endpoints, database operations, and external service interactions
- **Uniform API Responses**: Create middleware to standardize all API responses with consistent error handling and response formats
- **Currency Support**: Expand the currency enum to include more international currencies and cryptocurrency support
- **Pagination**: Implement efficient pagination for list endpoints, especially for provider listings with configurable page sizes

### Frontend Improvements
- **User Confirmation Dialogs**: Add confirmation dialogs for critical actions (delete, activate/deactivate providers)
- **Frontend Testing**: Implement comprehensive unit and integration tests using Jasmine/Karma
- **Filtering**: Implement search and filtering capabilities for provider management
- **Accessibility**: Ensure WCAG 2.1 compliance for better accessibility

### Infrastructure & Deployment
- **Production-Ready Frontend**: Serve Angular application with Nginx in production with proper caching headers
- **Database Migrations**: Implement automated database migration scripts for production deployments
- **Monitoring & Logging**: Add comprehensive logging with structured logging and application monitoring
- **Health Checks**: Implement health check endpoints for better monitoring and deployment strategies
- **CI/CD Pipeline**: Set up automated testing and deployment pipelines

### Architecture Improvements
- **Microservices Refinement**: Move interfaces into dedicated namespaces for better code organization
- **API Versioning**: Add proper API versioning strategy for backward compatibility

---

**Note**: This is a development/demo platform. For production use, please implement proper security measures, monitoring, and follow your organization's deployment guidelines.
