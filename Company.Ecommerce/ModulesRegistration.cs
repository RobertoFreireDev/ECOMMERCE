namespace Company.Ecommerce.Host;

public static class ModulesRegistration
{
    public static void ConfigureApi(this IServiceCollection services)
    {
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();

        services.ConfigureEvents();
    }

    public static IMvcBuilder AddApplicationParts(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder
            .AddOrderControllers()
            .AddBillingControllers();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterShoppingCartServices();

        services.RegisterOrderServices();

        services.RegisterBillingSubscribers();
    }
}