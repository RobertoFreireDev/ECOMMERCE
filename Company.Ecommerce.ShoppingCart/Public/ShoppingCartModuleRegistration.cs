namespace Company.Ecommerce.ShoppingCart.Public;

public static class ShoppingCartModuleRegistration
{
    public static IMvcBuilder AddShoppingCartControllers(this IMvcBuilder mvcBuilder)
    {
        return mvcBuilder.AddApplicationPart(typeof(ShoppingCartModuleRegistration).Assembly);
    }

    public static void RegisterShoppingCartServices(this IServiceCollection services)
    {
    }
}