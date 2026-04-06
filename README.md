# Sales System DDD
This is a sample project that demonstrates how to implement a sales system using Domain-Driven Design (DDD) principles. The project is structured into different layers, including the domain layer, application layer, and infrastructure layer.

It is based on the Macoratti DDD course that can be found on his blog or youtube channel.

## Domain Driven Design (DDD)
Concentrate the complex business logic in the domain layer, which is the heart of the application. The domain layer contains entities, value objects, aggregates, and domain services that encapsulate the core business rules and logic.

## Clean Architecture
Organize the application into layers that separate concerns and dependencies. The application layer contains use cases and application services that orchestrate the interactions between the domain layer and the infrastructure layer. The infrastructure layer contains implementations of repositories, data access, and external services.

## CQRS (Command Query Responsibility Segregation)
Separate the read and write operations into different models. The command model is responsible for handling write operations, while the query model is responsible for handling read operations. This allows for better scalability and performance, as well as a clearer separation of concerns.
