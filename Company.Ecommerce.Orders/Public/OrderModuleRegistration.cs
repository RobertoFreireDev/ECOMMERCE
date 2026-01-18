namespace Company.Ecommerce.Orders.Public;

public static class OrderModuleRegistration
{
    public static async Task ApplyOrdersMigrationsAsync(
        this IHost app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

        dbContext.Database.EnsureCreated();
        await dbContext.Database.MigrateAsync();
    }

    public static IMvcBuilder AddOrderControllers(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddApplicationPart(typeof(OrderModuleRegistration).Assembly);
    }

    public static void RegisterOrderServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("ORDERS_DB_CONNECTION")
            ?? throw new InvalidOperationException(
                "Connection string 'ORDERS_DB_CONNECTION' not found.");

        services.AddDbContext<OrdersDbContext>(options =>
            options.UseSqlServer(connectionString));

        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IShoppingCartAccessPoint, ShoppingCartAccessPoint>();
    }
}