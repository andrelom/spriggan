using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Spriggan.Core;

public static class Development
{
    /// <summary>
    /// Will provide the the build time of the current assembly.
    /// </summary>
    public static readonly DateTime LinkerTime = GetLinkerTime();

    /// <summary>
    /// Will read the .env file if available and set all environment variables.
    /// The .env file must be in the same directory as the solution file.
    /// </summary>
    /// <param name="filename">The .env file name.</param>
    [Conditional("DEBUG")]
    public static void ReadEnvironmentVariables(string filename = ".env.local")
    {
        var root = TryGetSolutionPath();
        var path = Path.Combine(root, filename);

        if (!File.Exists(path))
        {
            return;
        }

        var items = File.ReadAllLines(path)
            .Select(line => line.Split('='))
            .Where(parts => parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]))
            .Select(values => new KeyValuePair<string, string>(values[0], values[1]));

        foreach (var (key, value) in items)
        {
            Environment.SetEnvironmentVariable(key, value);
        }
    }

    #region Private Methods

    private static DateTime GetLinkerTime()
    {
        const string prefix = "+build";

        var assembly = Assembly.GetExecutingAssembly();
        var attribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

        if (attribute?.InformationalVersion == null)
        {
            return default;
        }

        var value = attribute.InformationalVersion;
        var index = value.IndexOf(prefix, StringComparison.OrdinalIgnoreCase);

        if (index <= 0)
        {
            return default;
        }

        var datetime = value[(index + prefix.Length)..];

        return DateTime.ParseExact(datetime, "yyyy-MM-ddTHH:mm:ss:fffZ", CultureInfo.InvariantCulture);
    }

    private static string TryGetSolutionPath()
    {
        var directory = new DirectoryInfo(Directory.GetCurrentDirectory());

        while (directory != null && !directory.GetFiles("*.sln").Any())
        {
            directory = directory.Parent;
        }

        return directory?.FullName ?? string.Empty;
    }

    #endregion
}
