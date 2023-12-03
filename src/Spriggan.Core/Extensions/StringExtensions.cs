using System.Globalization;
using System.Text;

namespace Spriggan.Core.Extensions;

public static class StringExtensions
{
    public static Guid ToGuid(this string value)
    {
        return Guid.TryParse(value, out var guid) ? guid : default;
    }

    public static string Standardize(this string value)
    {
        // More at: https://stackoverflow.com/a/2393966/224810

        var decomposed = value.Normalize(NormalizationForm.FormD);
        var filtered = decomposed.Where(item => char.GetUnicodeCategory(item) != UnicodeCategory.NonSpacingMark);

        return new string(filtered.ToArray());
    }
}
