```mermaid
flowchart TD
    Host["Host"]
    Common["Common"]
    Events["Events"]
    EventsDb["EventsDb"]
    BillingAPI["Billing.API"]
    BillingApp["Billing.Application"]
    OrdersAPI["Orders.API"]
    OrdersApp["Orders.Application"]
    CartAPI["ShoppingCart.API"]

    Host --> BillingAPI
    Host --> OrdersAPI
    Host --> CartAPI
    Host --> Events

    BillingAPI --> BillingApp
    OrdersAPI --> OrdersApp

    Events --> EventsDb
```

# Project Structure

## Host

- Serves as the entry point of the application.
- Knows all module entry points (Module APIs) and invokes them to configure each module.
- Configures shared services and middleware common to all modules, such as InMemoryEventPublisher and GlobalExceptionHandler.

## Common

- Contains DTOs and event classes used for cross-module communication via asynchronous event messages or synchronous access points.

### Asynchronous event messages

- Fire-and-forget events that are published to an event bus and handled by other modules.

### Synchronous access points

- Synchronous calls to other modules via well-defined interfaces.
- APIs are not used to avoid unnecessary HTTP overhead for in-process calls.

### Modules

- Each module has its own API, Application, Domain, and Infrastructure layers/libraries.
- Each module has its own database.
- Modules communicate with each other via asynchronous event messages or synchronous access points.
- Modules do not bypass other bounded contexts to access databases or functionality directly.