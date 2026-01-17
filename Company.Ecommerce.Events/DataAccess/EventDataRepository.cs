namespace Company.Ecommerce.Events.DataAccess;

public interface IEventDataRepository
{
    Task AddAsync(EventData eventData, CancellationToken cancellationToken);
}

public sealed class EventDataRepository : IEventDataRepository
{
    private readonly EventsDbContext _dbContext;

    public EventDataRepository(EventsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(EventData eventData, CancellationToken cancellationToken)
    {
        _dbContext.Events.Add(eventData);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}