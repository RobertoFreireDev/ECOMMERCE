namespace Company.Ecommerce.Billing.API;

public static class BillingModuleRegistration
{
    public static IMvcBuilder AddBillingControllers(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddApplicationPart(typeof(BillingModuleRegistration).Assembly);
    }

    public static void RegisterBillingSubscribers(this IServiceCollection services)
    {
        services.AddScoped<IDomainEventHandler<OrderPlacedEvent>, PlaceOrderSubscriber>();
    }
}