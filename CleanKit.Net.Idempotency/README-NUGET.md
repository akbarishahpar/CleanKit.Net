# CleanKit.Net

**CleanKit.Net** is a lightweight and modular framework for building scalable, maintainable, and testable applications using **Clean Architecture** principles in **ASP.NET Core** and **C#**.

It provides a well-structured starting point for enterprise-grade solutions by enforcing separation of concerns between layers such as Presentation, Application, Domain, and Infrastructure.

------

## ✨ Features

- Clean Architecture enforced by structure and contracts
- CQRS with **MediatR** (Queries & Commands)
- Built-in **idempotency** support for safe command execution
- Layered project separation (Domain, Application, Persistence, Presentation)
- Pluggable and extendable components
- Designed for testability and scalability

------

## 📦 Packages

| Package                                | Description                                              |
| -------------------------------------- | -------------------------------------------------------- |
| `CleanKit.Net.Application`             | Application layer containing commands, queries, handlers |
| `CleanKit.Net.Domain`                  | Core domain logic, entities, and domain services         |
| `CleanKit.Net.Idempotency`             | Interfaces and services for idempotent command execution |
| `CleanKit.Net.Idempotency.Persistence` | Implementation of idempotency persistence logic          |
| `CleanKit.Net.Persistence`             | Infrastructure & EF Core setup for data persistence      |
| `CleanKit.Net.Presentation`            | API layer, controllers, and configuration                |

------

### 📖 **Full documentation:**

📚 To access the complete documentation and stay up to date with the latest changes, please refer to the following link:

👉 [https://github.com/akbarishahpar/CleanKit.Net](https://github.com/akbarishahpar/CleanKit.Net)