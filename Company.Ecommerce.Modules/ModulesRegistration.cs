namespace Company.Ecommerce.Modules;

public static class ModulesRegistration
{
    public static void ConfigureApi(this IServiceCollection services)
    {
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

        services.AddScoped<IEventPublisher, InMemoryEventPublisher>();
    }

    public static IMvcBuilder AddApplicationParts(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder
            .AddOrderControllers()
            .AddBillingControllers();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterOrderServices();

        services.RegisterBillingSubscribers();

        services.AddScoped<IShoppingCartAccessPoint, ShoppingCartAccessPoint>();
    }
}