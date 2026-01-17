namespace Company.Ecommerce.Events;

public static class EventsRegistration
{
    public static void ConfigureEvents(this IServiceCollection services)
    {
        services.AddDbContext<EventsDbContext>(options =>
            options.UseSqlServer(Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION")));

        services.AddScoped(typeof(IEventRepository<>), typeof(EventRepository<>));
        services.AddScoped<IEventPublisher, EventPublisher>();
    }
}
