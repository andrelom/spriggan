namespace Spriggan.Core.Transport.Extensions;

public static class TypeExtensions
{
    public static string ToQueueName(this Type type, string group)
    {
        var name = type.FullName;

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new Exception();
        }

        return $"{group}#{name}";
    }
}
