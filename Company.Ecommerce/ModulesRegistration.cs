namespace Company.Ecommerce.Host;

public static class ModulesRegistration
{
    public static void ConfigureApi(this IServiceCollection services)
    {
        services
            .AddExceptionHandler<GlobalExceptionHandler>()
            .AddProblemDetails();
    }

    public static IMvcBuilder AddApplicationParts(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder
            .AddOrderControllers()
            .AddShoppingCartControllers();
    }

    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterOrderServices(configuration);
        services.RegisterShoppingCartServices(configuration);
    }
}