

# Vivarni.DDD

Fundamental building blocks for creating the domain layer in a Domain-Driven Design (DDD) software design approach.

## Introduction

Vivarni.DDD provides essential building blocks to help developers implement the domain layer of their applications following the principles of Domain-Driven Design (DDD). This library aims to streamline the creation of robust and maintainable domain models by offering reusable and extensible components. It contains interfaces for entities, aggregate roots, domain events and a generic repository for aggregate root entities using the specification pattern.

## Core Principles

This library is built upon the foundational principles of Domain-Driven Design:

 * Focus on the Core Domain: Emphasize the most critical aspects of yourapplication.
 * Ubiquitous Language: Foster collaboration between technical and businessteams by using a shared language.
 * Encapsulation: Maintain separation of concerns by encapsulating businesslogic within the domain.

## Features and Concepts
### Entities

Entities are objects that are defined by their unique identity rather than their attributes. In Vivarni.DDD, entities provide a base class that helps you define your domain-specific entities.

Example:
```csharp
public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; private set; }
    public DateTimeOffset OrderDate { get; private set; }

    // Constructor and methods here
}
```

### Aggregates

Aggregates are clusters of entities and value objects that are treated as a single unit. They enforce invariants and ensure consistency within the boundaries. Entities that are aggregate root should implement the `IAggregateRoot` marker interface.

Example:
```csharp
public class ShoppingCart : IAggregateRoot
{
    public Guid Id { get; set; }
    private readonly List<CartItem> _items = new();

    public void AddItem(Product product, int quantity)
    {
        // Business logic for adding an item
    }
}
```

### Repositories and Specifications

Repositories are responsible for managing the persistence and retrieval of aggregates. They abstract the database access layer. Vivarni offers the `IGenericRepository<T>` interface, with an implementation in `Vivarni.DDD.Infrastructure` that accepts aggregate root entities to operate on.

Specifications encapsulate business rules and allow for reusable and composable query logic. We build upon the wonderfull package provided by [ardalis/Specification](https://github.com/ardalis/Specification) to create these specifications. The `IGenericRepository<T>` interface accepts these specifications in order to construct the underlying database query.



Example usage:
```csharp
public class ApiController(IGenericRepository<ShoppingCart> repositoryBase) : ControllerBase
{
    private readonly IGenericRepository<ShoppingCart> _repository;

    [HttpGet("my-shopping-cart")]
    public async Task<IActionResult> GetShoppingCart()
    {
        // Create the specification
        var userId = User.Identity.Name;
        var spec = new ShoppingCartSpecification(userId);
        
        // Fetch the aggregate root entity from the repository
        var messages = await _repository.SingleAsync(spec);

        // Return the result
        // (this should be mapped to a API Model in stead of returning the Domain entity)
        return Ok(messages);
    }
}

public class ShoppingCartSpecification : Specification<Customer>
{
    public ShoppingCartSpecification(string userId)
    {
        // Filter shopping carts using the provided userId parameter
        Query.Where(x => x.UserId == userId);
    }
}
```

### Domain Events

Domain Events capture and communicate significant events within the domain. They allow decoupling and facilitate reactive systems. They typically have lighter payloads, containing only the necessary information for processing. Domain events can be used to communicate between different aggregates (keeping business logic separated in the domain layer), or between different architectural layers. Domain events are processed by event handlers, or sometimes also referred to as event listeners. A single domain event can be handled by zero or more listeners.

Example:
```csharp
public class OrderPlacedEvent : IDomainEvent
{
    public Guid OrderId { get; }
    public DateTime PlacedAt { get; }
}

public class OrderPlacedEventHandler : IDomainEventHandler<OrderPlacedEventHandler>
{
    public OrderPlacedEventHandler(/* use dependency injection */) { }

    public Task HandleAsync(OrderPlacedEventHandler event)
    {
        // Do some work
        return Task.CompletedTask;
    }
}
```

## Domain Types (aka: Type Safe Enums, Smart Enums)

*Type safe enums* are well known code construct which is found regularly in various projects, varying only slightly in implementation. The gist here is to have enums that can be extended with properties and methods. 

Note to future contributors: the name *domain type* is somewhat chosen poorly. We chose this name before knowing it was a design pattern that already had a name. Perhaps we should make it obsolete and replace it with another name?

```csharp
public class Shape : DomainType<OrderCategory>()
{
    public static readonly Shape Square = new("Square");
    public static readonly Shape Circle = new("Circle");
    public static readonly Shape Triangle = new("Triangle");

    private RgbColor(string name): base(name) { }
}

IEnumerable<Shape> shapes = Shape.GetAll();
Shape circle = Shape.FromName("Circle");
```


## Getting Started

**Installation:** Add the Vivarni.DDD package to your project using NuGet.

```bash
dotnet add package Vivarni.DDD
```

**Configuration:** Set up your project to integrate the library's components.  
**Usage:** Start defining your domain layer using the provided building blocks.

## Examples

Refer to the `sample` folder in this repository for in-depth usage scenarios, including:

 * bla
 * blabla

## Contributing

Contributions are welcome! To get started:

 * Fork the repository.
 * Create a new branch for your feature or bug fix.
 * Submit a pull request with a detailed description.

## License

This project is licensed under the MIT License. Feel free to use it in your projects.