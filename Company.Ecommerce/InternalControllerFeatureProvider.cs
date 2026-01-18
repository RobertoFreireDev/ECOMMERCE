namespace Company.Ecommerce.Host;

public sealed class InternalControllerFeatureProvider
    : ControllerFeatureProvider
{
    protected override bool IsController(TypeInfo typeInfo)
    {
        if (!typeInfo.IsClass)
            return false;

        if (typeInfo.IsAbstract)
            return false;

        if (!typeof(ControllerBase).IsAssignableFrom(typeInfo))
            return false;

        // allow internal controllers
        return typeInfo.Name.EndsWith("Controller", StringComparison.Ordinal);
    }
}
