namespace Company.Ecommerce.Events;

public static class EventsRegistration
{
    public static void ConfigureEvents(this IServiceCollection services)
    {
        services.AddDbContext<EventsDbContext>(options =>
            options.UseSqlServer(
                "Server=(localdb)\\MSSQLLocalDB;Database=EventsDb;Trusted_Connection=True;"
            ));

        services.AddScoped<IEventDataRepository, EventDataRepository>();
        services.AddScoped<IEventPublisher, EventPublisher>();
    }
}
