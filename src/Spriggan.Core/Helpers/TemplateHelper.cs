using Scriban;

namespace Spriggan.Core.Helpers;

public static class TemplateHelper
{
    public static async Task<string?> ParseText(string text, object data)
    {
        return await Template.Parse(text).RenderAsync(data);
    }

    public static async Task<string?> ParseFile(string path, object data, CancellationToken cancel = default)
    {
        if (!File.Exists(path))
        {
            return default;
        }

        var text = await File.ReadAllTextAsync(path, cancel);

        return await ParseText(text, data);
    }
}
