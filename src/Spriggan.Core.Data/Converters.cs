using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Spriggan.Core.Data;

public static class Converters
{
    public static ValueConverter<string, string> ToUpperCase = new (value => value, value => value.ToUpper());
}
