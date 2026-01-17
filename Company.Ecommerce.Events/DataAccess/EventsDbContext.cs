namespace Company.Ecommerce.Events.DataAccess;

public sealed class EventData
{
    public Guid Id { get; set; }
    public EventTypes EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime OccurredOnUtc { get; set; }
}

public sealed class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options)
        : base(options) { }

    public DbSet<EventData> Events => Set<EventData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventData>(entity =>
        {
            entity.ToTable("Events");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.EventType)
                  .IsRequired()
                  .HasMaxLength(300);

            entity.Property(x => x.Payload)
                  .IsRequired();

            entity.Property(x => x.OccurredOnUtc)
                  .IsRequired();
        });
    }
}