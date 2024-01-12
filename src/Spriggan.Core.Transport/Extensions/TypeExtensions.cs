namespace Spriggan.Core.Transport.Extensions;

public static class TypeExtensions
{
    internal static string ToQueueName(this Type type, string group)
    {
        var name = type.AssemblyQualifiedName;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        return $"{group}#{name}";
    }
}
