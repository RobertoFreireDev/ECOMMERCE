namespace Company.Ecommerce.ShoppingCart.API;

public static class ShoppingCartModuleRegistration
{
    public static void RegisterShoppingCartServices(this IServiceCollection services)
    {
        services.AddScoped<IShoppingCartAccessPoint, ShoppingCartAccessPoint>();
    }
}