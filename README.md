# eShopMicroservices

A comprehensive microservices-based e-commerce application built with .NET 8 and modern cloud-native practices. This project demonstrates how to build and connect multiple microservices using the latest technologies and architectural patterns.

## 🚀 Features

This project implements a complete e-commerce platform with the following microservices:

### Catalog Microservice
- ASP.NET Core Minimal APIs with the latest .NET 8 and C# 12 features
- Vertical Slice Architecture with Feature folders
- CQRS implementation using MediatR
- Validation Pipeline using MediatR and FluentValidation
- Marten as a .NET Transactional Document DB on PostgreSQL
- Carter for Minimal API endpoint definition
- Cross-cutting concerns: Logging, Exception Handling, Health Checks

### Basket Microservice
- ASP.NET 8 Web API following REST principles
- Redis for distributed caching
- Proxy, Decorator, and Cache-aside patterns
- gRPC communication with Discount service
- Message publishing with MassTransit and RabbitMQ

### Discount Microservice
- ASP.NET gRPC Server
- Protobuf message contracts
- Entity Framework Core with SQLite
- Database migrations and containerization

### Ordering Microservice
- Domain-Driven Design (DDD), CQRS, and Clean Architecture
- MediatR, FluentValidation, and Mapster
- Domain Events & Integration Events
- Entity Framework Core with SQL Server
- Message consumption with MassTransit-RabbitMQ

### Yarp API Gateway
- Gateway Routing Pattern implementation
- Route, Cluster, Path, Transform, and Destinations configuration
- Rate Limiting with FixedWindowLimiter

## 📊 Architecture

This project demonstrates several architectural patterns:

- **Microservices Architecture**: Decomposing an application into independent services
- **Domain-Driven Design**: Focusing on the core domain and domain logic
- **CQRS Pattern**: Separating read and update operations
- **Vertical Slice Architecture**: Organizing code by feature rather than technical concerns
- **Clean Architecture**: Separation of concerns with layered architecture
- **Event-Driven Architecture**: Using message queues for asynchronous communication

## 🔧 Technologies

- **ASP.NET Core 8**: Latest web framework
- **Entity Framework Core**: ORM for SQL Server and SQLite
- **Marten**: Document DB for PostgreSQL
- **Redis**: Distributed cache
- **RabbitMQ**: Message broker
- **MassTransit**: Message bus abstraction
- **gRPC**: High-performance RPC framework
- **Docker**: Containerization
- **Yarp**: Reverse proxy for API gateway
- **.NET Aspire**: Cloud-ready stack for distributed applications
- **MediatR**: Mediator pattern implementation
- **FluentValidation**: Validation library
- **Mapster**: Object-to-object mapping

## 🛠️ Communication Patterns

- **Sync Communication**: Direct API calls using gRPC between services
- **Async Communication**: Message-based using RabbitMQ with Publish/Subscribe pattern
- **API Gateway**: Centralizing service access through Yarp

## 🐳 Docker Support

The application is fully containerized with Docker, including:
- Microservices containers
- Database containers (PostgreSQL, SQL Server, Redis, SQLite)
- Message broker container (RabbitMQ)
- Network configuration
- Environment variable overrides

## 🌟 .NET Aspire Support

This project includes support for .NET Aspire, Microsoft's opinionated, cloud-ready stack for building distributed applications:
- Service discovery and orchestration
- Health checks integration
- Observability and telemetry
- Configuration management
- Resilience patterns

## 🏗️ Project Structure

```
eShopMicroservices/
├── src/
│   ├── ApiGateways/
│   │   └── YarpApiGateway/
│   ├── Aspire/                   # .NET Aspire integration
│   ├── BuildingBlocks/           # Shared libraries and components
│   │   ├── BuildingBlocks.Behaviors/
│   │   ├── BuildingBlocks.CQRS/
│   │   ├── BuildingBlocks.Exceptions/
│   │   ├── BuildingBlocks.Messaging/
│   │   └── BuildingBlocks.Pagination/
│   ├── Services/
│   │   ├── Basket/
│   │   │   └── Basket.API/
│   │   ├── Catalog/
│   │   │   └── Catalog.API/
│   │   ├── Discount/
│   │   │   └── Discount.Grpc/
│   │   └── Ordering/
│   │       ├── Ordering.API/
│   │       ├── Ordering.Application/
│   │       ├── Ordering.Domain/
│   │       └── Ordering.Infrastructure/
│   └── docker-compose.yml        # Docker orchestration
```

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Docker Desktop
- Visual Studio 2022 or VS Code

### Running the Application

1. Clone the repository
   ```
   git clone https://github.com/Devancy/eShopMicroservices.git
   ```

2. Navigate to the src directory
   ```
   cd eShopMicroservices/src
   ```

3. Run with Docker Compose
   ```
   docker-compose up
   ```

4. Access services:
   - Catalog API: http://localhost:6000/products
   - Basket API: http://localhost:6001/basket/{username}
   - Ordering API: http://localhost:6003/orders
   - API Gateway: http://localhost:6004

## 🧠 What You'll Explore

This project demonstrates how to:
- Build microservices using latest .NET features
- Implement DDD, CQRS, and Clean Architecture
- Set up inter-service communication (sync and async)
- Use document and relational databases effectively
- Deploy microservices with Docker
- Implement API Gateway pattern
- Apply SOLID principles and design patterns
- Handle cross-cutting concerns
- Work with message brokers and event-driven architecture
- Build resilient services with proper error handling
- Implement cloud-native applications with .NET Aspire

## 📚 Best Practices

This project follows industry best practices:
- Loosely-coupled, dependency-inverted architecture
- SOLID principles
- Clean code principles
- Proper exception handling
- Comprehensive logging
- Health checks for monitoring
- Docker containerization
- Message-based communication
- API Gateway pattern

## 📝 License

This project is licensed under the MIT License.