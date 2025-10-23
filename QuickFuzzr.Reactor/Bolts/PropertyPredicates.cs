using System.Reflection;

namespace QuickFuzzr.Reactor;

public static class PropertyPredicates
{
    public static bool PropertyNamed(this Type type, PropertyInfo propertyInfo, string propertyName)
        => propertyInfo.DeclaringType == type
            && string.Equals(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase);

    public static bool PropertyNamed(this PropertyInfo propertyInfo, string propertyName)
        => string.Equals(propertyInfo.Name, propertyName, StringComparison.OrdinalIgnoreCase);

    public static bool IsEntityId<T>(this PropertyInfo propertyInfo)
        => propertyInfo.PropertyType is T
        && (propertyInfo.PropertyNamed("id")
        || propertyInfo.PropertyNamed($"{propertyInfo.DeclaringType?.Name}id"));
}