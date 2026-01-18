namespace Company.Ecommerce.Orders.Internal.Domain.Base;

internal class OrdersDbContext : DbContext
{
    public DbSet<Order> Orders => Set<Order>();

    public OrdersDbContext(DbContextOptions<OrdersDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(order =>
        {
            order.ToTable("Orders");

            order.HasKey(o => o.Id);

            order.Property(o => o.CustomerId).IsRequired();
            order.Property(o => o.ShippingAddressId).IsRequired();
            order.Property(o => o.BillingAddressId).IsRequired();
            order.Property(o => o.PaymentMethod).IsRequired();
            order.Property(o => o.CouponCode);

            order.OwnsMany(o => o.Items, item =>
            {
                item.ToTable("OrderItems");

                item.WithOwner()
                    .HasForeignKey("OrderId");

                item.HasKey("OrderId", "ProductId");

                item.Property(i => i.ProductId).IsRequired();
                item.Property(i => i.Name).IsRequired();
                item.Property(i => i.UnitPrice).IsRequired();
                item.Property(i => i.Quantity).IsRequired();
            });
        });
    }
}
