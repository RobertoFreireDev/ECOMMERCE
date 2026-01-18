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
            .AddOrderControllers();
    }

    public static void RegisterServices(this IServiceCollection services)
    {
        services.RegisterOrderServices();
    }
}