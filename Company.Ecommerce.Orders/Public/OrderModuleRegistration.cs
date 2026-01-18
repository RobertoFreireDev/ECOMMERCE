namespace Company.Ecommerce.Orders.Public;

public static class OrderModuleRegistration
{
    public static IMvcBuilder AddOrderControllers(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddApplicationPart(typeof(OrderModuleRegistration).Assembly);
    }

    public static void RegisterOrderServices(this IServiceCollection services)
    {
        //services.AddDbContext<OrderDbContext>(options =>
        //    options.UseSqlServer(Environment.GetEnvironmentVariable("ORDERS_DB_CONNECTION")));

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IShoppingCartAccessPoint, ShoppingCartAccessPoint>();
    }
}