namespace Company.Ecommerce.Modules;

public static class ModulesRegistration
{
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