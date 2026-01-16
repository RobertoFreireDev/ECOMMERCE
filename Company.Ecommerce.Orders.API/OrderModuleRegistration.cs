namespace Company.Ecommerce.Orders.API;

public static class OrderModuleRegistration
{
    public static IMvcBuilder AddOrderControllers(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddApplicationPart(typeof(OrderModuleRegistration).Assembly);
    }

    public static void RegisterOrderServices(this IServiceCollection services)
    {
        services.AddScoped<IOrderService, OrderService>();
    }
}