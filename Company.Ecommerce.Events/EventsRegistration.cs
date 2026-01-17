namespace Company.Ecommerce.Events;

public static class EventsRegistration
{
    public static void ConfigureEvents(this IServiceCollection services)
    {
        services.AddDbContext<EventsDbContext>(options =>
            options.UseSqlServer(
                Environment.GetEnvironmentVariable("EVENTS_DB_CONNECTION")
                    ?? "Server=db;Database=EventsDb;User Id=sa;Password=YourStrong!Passw0rd;"
            ));


        services.AddScoped<IEventDataRepository, EventDataRepository>();
        services.AddScoped<IEventPublisher, EventPublisher>();
    }
}
