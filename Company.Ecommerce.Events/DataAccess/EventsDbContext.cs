namespace Company.Ecommerce.Events.DataAccess;

public class Entity
{
    public Guid Id { get; set; }
}

public class EventData : Entity
{
    public EventTypes EventType { get; set; } = default!;
    public string Payload { get; set; } = default!;
    public DateTime OccurredOnUtc { get; set; }
}

public class FailedEvent : Entity
{
    public Guid EventId { get; set; }
    public int RetryCount { get; set; } = 0;
    public DateTime? LastAttemptUtc { get; set; }
    public EventData Event { get; set; } = null!;
}

public class DeadLetterEvent : Entity
{
    public Guid EventId { get; set; }
    public DateTime FailedOnUtc { get; set; }
    public EventData Event { get; set; } = null!;
}

public sealed class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options)
        : base(options) { }

    public DbSet<EventData> Events => Set<EventData>();
    public DbSet<FailedEvent> FailedEvents => Set<FailedEvent>();
    public DbSet<DeadLetterEvent> DeadLetterEvents => Set<DeadLetterEvent>();

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

        modelBuilder.Entity<FailedEvent>(entity =>
        {
            entity.ToTable("FailedEvents");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EventId)
                  .IsRequired();

            entity.Property(x => x.RetryCount)
                  .IsRequired()
                  .HasDefaultValue(0);

            entity.Property(x => x.LastAttemptUtc);

            entity.HasOne(x => x.Event)
                  .WithMany()
                  .HasForeignKey(x => x.EventId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
        });

        modelBuilder.Entity<DeadLetterEvent>(entity =>
        {
            entity.ToTable("DeadLetterEvents");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.EventId)
                  .IsRequired();

            entity.Property(x => x.FailedOnUtc)
                  .IsRequired();

            entity.HasOne(x => x.Event)
                  .WithMany()
                  .HasForeignKey(x => x.EventId)
                  .OnDelete(DeleteBehavior.Cascade)
                  .IsRequired();
        });
    }
}