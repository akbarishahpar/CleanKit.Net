<p align="center"><img src="https://raw.githubusercontent.com/akbarishahpar/CleanKit.Net/refs/heads/main/Logo.png" /></p>

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

## 🧱 Core Concepts

### CQRS with MediatR

**CleanKit.Net** abstracts **MediatR** to provide cleaner command and query handling:

```c#
// Queries
public interface IQuery {}
public interface IQuery<TResponse> : IQuery, IRequest<Result<TResponse>> {}

// Query Handler
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse> {}

// Commands
public interface ICommand : IRequest<Result> {}
public interface ICommand<TResponse> : IRequest<Result<TResponse>> {}
public interface INonTransactional {}

// Command Handlers
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand {}

public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse> {}
```

------

## 🧪 Idempotency

Prevent duplicate command executions in distributed systems with:

- `IIdempotencyService`
- `RequestManager` for tracking processed requests
- Backed by `CleanKit.Net.Idempotency.Persistence`

------

## 🚀 Getting Started With Recommended Project Structure

To fully leverage **CleanKit.Net**, structure your solution using the Clean Architecture layering approach:

```
MyCleanApp/
├── MyCleanApp.Application       → Business use cases (CQRS)
├── MyCleanApp.Domain            → Domain models, entities, events, and interfaces
├── MyCleanApp.Infrastructure    → Cross-cutting concerns (e.g., email, external services)
├── MyCleanApp.Persistence       → Database implementations (EF Core, etc.)
├── MyCleanApp.Presentation.Web  → Web API (controllers, endpoints, filters)
├── MyCleanApp.Web               → ASP.NET Core project (entry point)
└── MyCleanApp.sln
```

### 📦 NuGet Packages per Layer

| Project            | Purpose                                         | Install This Package(s)                                      |
| ------------------ | ----------------------------------------------- | ------------------------------------------------------------ |
| **Application**    | CQRS, handlers, use cases                       | `CleanKit.Net.Application`                                   |
| **Domain**         | Entities, aggregates, domain events & contracts | `CleanKit.Net.Domain`                                        |
| **Infrastructure** | External services (email, queues, etc.)         | *(Depends on use case – no required CleanKit package)*       |
| **Persistence**    | DB contexts, EF Core implementations            | `CleanKit.Net.Persistence` `CleanKit.Net.Idempotency.Persistence` `CleanKit.Net.Outbox.Persistence` |
| **Presentation**   | API layer, endpoints, middleware, filters       | `CleanKit.Net.Presentation`                                  |

> You can also add `CleanKit.Net.Idempotency`, `CleanKit.Net.Outbox`, and `CleanKit.Net.DependencyInjection` where needed.

------

## 💡 Example: Installing Packages

Install packages for each project:

```
# Application layer
dotnet add MyCleanApp.Application package CleanKit.Net.Application

# Domain layer
dotnet add MyCleanApp.Domain package CleanKit.Net.Domain

# Persistence layer
dotnet add MyCleanApp.Persistence package CleanKit.Net.Persistence
dotnet add MyCleanApp.Persistence package CleanKit.Net.Idempotency.Persistence
dotnet add MyCleanApp.Persistence package CleanKit.Net.Outbox.Persistence

# Presentation layer
dotnet add MyCleanApp.Presentation package CleanKit.Net.Presentation
```

------

This structure ensures **separation of concerns**, **testability**, and **modular design**, aligned with Clean Architecture principles.

## 📦 Use a Separate Entry Point Project

To adhere more closely to Clean Architecture principles and keep the `Presentation` layer **reusable and decoupled**, it is recommended to create a separate entry-point project—such as **MyCleanApp.Web**—which acts as the host and wires all application layers together.

### 🔍 Why This is Better

- **Loose coupling**: `Presentation` is just another layer, not tied to hosting concerns.
- **Testability**: Makes it easier to unit test `Presentation` logic without bootstrapping the web server.
- **Reusability**: You can reuse `Presentation` in different host types (e.g., console app, serverless).

## 🧩 Centralized Dependency Injection per Layer

To keep your architecture **modular**, **composable**, and **clean**, each layer should define its own static `DependencyInjection` class. This class exposes an extension method (e.g., `AddApplication`, `AddPersistence`, etc.) responsible for registering the services specific to that layer.

This pattern ensures that:

- Each layer manages its own dependencies
- The composition root (typically the `Program.cs` file of the entry-point project) remains clean and declarative
- Layers stay loosely coupled and testable

### 📁 Project Structure Example

```
MyCleanApp/
├── Application/
│   └── DependencyInjection.cs → AddApplication(IServiceCollection, IConfiguration)
├── Infrastructure/
│   └── DependencyInjection.cs → AddInfrastructure(IServiceCollection, IConfiguration)
├── Persistence/
│   └── DependencyInjection.cs → AddPersistence(IServiceCollection, IConfiguration)
├── Presentation.Website/
│   └── DependencyInjection.cs → AddPresentation(IServiceCollection, IConfiguration)
├── Web/
│   └── Program.cs → Calls the above methods to wire up all layers
```

### ✅ Example Usage in `Program.cs` (Web Entry Point)

```c#
var builder = WebApplication.CreateBuilder(args);

// Register services from all layers
MyCleanApp.Application.DependencyInjection.AddApplication(builder.Services, builder.Configuration);
MyCleanApp.Infrastructure.DependencyInjection.AddInfrastructure(builder.Services, builder.Configuration);
MyCleanApp.Persistence.DependencyInjection.AddPersistence(builder.Services, builder.Configuration);
MyCleanApp.Presentation.Website.DependencyInjection.AddPresentation(builder.Services, builder.Configuration);

var app = builder.Build();

// Register middleware and route handling
app.UsePresentation();

app.Run();
```

> 📌 This approach gives each layer full control over its registrations, and the entry point merely composes them — following the Dependency Inversion Principle and the Clean Architecture model.

## 🧠 Dependency Injection in the Application Layer

In the **Application layer**, it is crucial to register the MediatR service along with necessary **behaviors** to handle commands and queries in a clean, efficient, and modular way. This is done using the `DependencyInjection` class, which serves to wire up all the necessary services for the Application layer.

The **MediatR Behaviors** provide an elegant way to add cross-cutting concerns (e.g., logging, validation, transactions, idempotency) to your command and query handlers without cluttering the business logic.

### ✅ Example: `DependencyInjection` in the Application Layer

Here’s how the `DependencyInjection` class can be set up to register **MediatR** and the required behaviors:

```c#
namespace MyCleanApp.Application.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(options =>
		{
        	options.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            options.AddLoggingBehaviour();
            options.AddValidationBehaviour();
            options.AddTransactionBehaviour();
            options.AddIdempotencyBehaviour();
		});

        services.AddValidatorsFromAssembly(
            typeof(DependencyInjection).Assembly,
            includeInternalTypes: true
        );

        services.AddOptions<WalletCryptographyOptions>(configuration);

        return services;
    }
}
```

### 🔍 Explanation of Each Behavior

- **LoggingBehavior**: This behavior logs every executed command and query. It's useful for debugging and tracing the flow of operations within the system.

  ```c#
  options.AddLoggingBehaviour();
  ```

- **ValidationBehavior**: Using FluentValidation, this behavior validates commands before they are executed. If a command has a corresponding validator (i.e., a class that implements `IValidator<T>`), it will validate the command's data before it reaches the handler.

  ```c#
  options.AddValidationBehaviour();
  ```

- **TransactionBehavior**: Ensures that a transaction session is created for commands that interact with the database. This behavior makes sure that the database operations are atomic and consistent. It will skip commands that implement the `INonTransactional` interface, as these are not required to be wrapped in a transaction.

  ```c#
  options.AddTransactionBehaviour();
  ```

- **IdempotencyBehavior**: This behavior is only applied to commands that implement the `IIdempotentCommand` interface. It ensures that repeated executions of the same command have no unintended side effects (useful for avoiding duplication in systems like payment processing).

  ```c#
  options.AddIdempotencyBehaviour();
  ```

- **FluentValidation Registration**: Automatically registers all the validators from the assembly where `DependencyInjection` is located. This makes it easy to validate commands and queries by simply creating appropriate validator classes.

  ```c#
  services.AddValidatorsFromAssembly(
      typeof(DependencyInjection).Assembly,
      includeInternalTypes: true
  );
  ```

> 📌 These behaviors ensure that cross-cutting concerns such as logging, validation, transaction handling, and idempotency are applied consistently across your application without polluting your business logic.



## 🗄️ Dependency Injection in the Persistence Layer

The **Persistence layer** is responsible for data access and storage concerns in a clean architecture setup. To properly wire up this layer, you should define a `DependencyInjection` class that registers:

- The main `DbContext`
- Outbox pattern repositories
- Idempotency support
- Interfaces for `IUnitOfWork` and `IDatabaseContext`

### ✅ Example: `DependencyInjection` Class in Persistence Layer

```c#
public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(IServiceCollection services,
        IConfiguration configuration)
    {
        // Getting connection string for the main database from configuration
        var connectionString = configuration.GetConnectionString("MainDb");

        // Registering the main EF Core DbContext
        services.AddDbContext<MainDbContext>(options =>
            options.UseSqlServer(connectionString)
                .AddDefaultInterceptors()              // Adds built-in EF Core interceptors (e.g., soft deletes, timestamps)
                .AddOutboxMessagesInterceptor()       // Enables Outbox pattern for reliable message publishing
        );

        // Registering DbContext as interfaces for abstraction and testing
        services.AddScoped<IDatabaseContext>(sp => sp.GetRequiredService<MainDbContext>());
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<MainDbContext>());

        // Registering Outbox repositories to support eventual consistency and messaging
        services.AddOutboxRepositories();

        // Registering Idempotency repositories to avoid duplicate command handling
        services.AddIdempotencyRepositories();

        // Making domain event handlers idempotent (safe for re-execution)
        services.AddIdempotentDomainEventHandler();

        // TODO: Register project-specific repositories (e.g., for your entities)
                
        return services;
    }
}
```

------

### 🔍 Explanation of Key Registrations

| Code                                | Purpose                                                      |
| ----------------------------------- | ------------------------------------------------------------ |
| `AddDbContext<MainDbContext>`       | Registers the Entity Framework Core DbContext with SQL Server using the configured connection string. |
| `.AddDefaultInterceptors()`         | Adds helpful EF Core interceptors like auditing, soft delete, or custom behavior. |
| `.AddOutboxMessagesInterceptor()`   | Enables **Outbox Pattern** to ensure database operations and event publishing are atomic. |
| `IDatabaseContext`, `IUnitOfWork`   | Interfaces for decoupling the application from EF Core. Helps in testing and enforcing clean architecture boundaries. |
| `AddOutboxRepositories()`           | Adds repositories to support the outbox messaging mechanism. |
| `AddIdempotencyRepositories()`      | Supports idempotent command processing by storing previously handled command IDs. |
| `AddIdempotentDomainEventHandler()` | Ensures your domain event handlers are safe to retry without unintended side effects. |



## 🎯 Command & Query Result Handling

In **CleanKit.Net**, all **queries** and **commands** return results using a unified wrapper called `Result<TValue>`. This pattern ensures **consistent**, **predictable**, and **clean** handling of operation outcomes across the entire application.

------

### ✅ What is `Result<TValue>`?

The `Result<TValue>` class encapsulates the outcome of an operation. It provides a structured way to handle both **success** and **failure**, without relying on exceptions for control flow.

------

### 📦 Structure

Here's a breakdown of the `Result<TValue>` class:

| Property    | Description                                                  |
| ----------- | ------------------------------------------------------------ |
| `IsSuccess` | A `bool` indicating whether the operation was successful.    |
| `TValue`    | The actual return value (used in **queries** or **commands** that return data). |
| `Error`     | An object including error code and a message describing what went wrong, if `IsSuccess` is `false`. |



------

### 🧠 Usage in Queries

**Queries** are expected to return a result *with* a value:

```c#
public record GetUserByIdQuery(Guid UserId) : IQuery<UserDto>;
public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    public async Task<Result<UserDto>> Handle(GetUserByIdQuery query, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(query.UserId);

        if (user is null)
            return Result.Failure<UserDto>("User not found");

        return Result.Success(new UserDto(user));
    }
}
```

------

### 🛠️ Usage in Commands

**Commands** may or may not return a value. Those that don’t, return a non-generic `Result`:

```c#
public record UpdateUserCommand(Guid Id, string NewName) : ICommand;
public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByIdAsync(command.Id);

        if (user is null)
            return Result.Failure("User not found");

        user.UpdateName(command.NewName);
        return Result.Success();
    }
}
```

------

### 🔐 Benefits

- **No need for exceptions for flow control**
- **Consistent response shape for API and UI layers**
- **Easy to compose or chain operations**
- **Improved error handling**

------

This pattern makes it easy for the application and presentation layers to process results in a unified way:

```c#
var result = await _mediator.Send(new UpdateUserCommand(...));

if (!result.IsSuccess)
    return BadRequest(result.Error);
```

------

## ✅ Conclusion

**CleanKit.Net** empowers you to build modular, testable, and maintainable applications using **Clean Architecture principles** in ASP.NET and C#. By organizing your solution into clear layers—**Application**, **Domain**, **Infrastructure**, **Persistence**, and **Presentation**—and using structured patterns like `Result<T>`, MediatR pipelines, and centralized `DependencyInjection`, your projects remain scalable and developer-friendly.

Start by setting up your projects, installing the right NuGet packages, and wiring dependencies via the entry point. With **CleanKit.Net**, you can focus on delivering features while keeping your architecture clean and robust.