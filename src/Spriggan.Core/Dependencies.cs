using System.Collections.Immutable;
using System.Reflection;

namespace Spriggan.Core;

public static class Dependencies
{
    private static bool _initialized;

    private static IImmutableList<Assembly> _assemblies = null!;

    private static IImmutableList<Type> _types = null!;

    /// <summary>
    /// It will return all assemblies available in the application domain.
    /// </summary>
    public static IImmutableList<Assembly> Assemblies
    {
        get
        {
            Ensure();

            return _assemblies;
        }
    }

    /// <summary>
    /// It will return all types available in the application domain.
    /// </summary>
    public static IImmutableList<Type> Types
    {
        get
        {
            Ensure();

            return _types;
        }
    }

    /// <summary>
    /// It will read all available DLLs and load them into the application domain.
    /// </summary>
    /// <param name="prefix">The assembly namespace prefix.</param>
    public static void Initialize(string prefix)
    {
        if (_initialized)
        {
            return;
        }

        var assemblies = GetReferencedAssemblies(prefix).ToList();
        var locations = GetFiles(prefix, assemblies.Select(assembly => assembly.Location).Distinct());

        assemblies.AddRange(locations.Select(GetAssembly));

        _initialized = true;
        _assemblies = assemblies.ToImmutableList();
        _types = assemblies.SelectMany(assembly => assembly.GetTypes()).ToImmutableList();
    }

    #region Private Methods

    private static void Ensure()
    {
        if (!_initialized)
        {
            throw new InvalidOperationException($"Assemblies have not been loaded, you must call '{nameof(Dependencies)}.{nameof(Initialize)}()', on the application startup");
        }
    }

    private static bool IsValidAssembly(string prefix, AssemblyName name)
    {
        return name?.FullName.StartsWith(prefix) ?? false;
    }

    private static bool IsValidAssembly(string prefix, Assembly assembly)
    {
        return IsValidAssembly(prefix, assembly.GetName());
    }

    private static IEnumerable<string> GetFiles(string prefix, IEnumerable<string> paths)
    {
        return Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, $"{prefix}.*.dll")
            .Where(path => !paths.Contains(path, StringComparer.InvariantCultureIgnoreCase));
    }

    private static Assembly GetAssembly(string location)
    {
        return AppDomain.CurrentDomain.Load(AssemblyName.GetAssemblyName(location));
    }

    private static IEnumerable<Assembly> GetReferencedAssemblies(string prefix)
    {
        var attendance = new List<string>();
        var stack = new Stack<Assembly>();

        stack.Push(Assembly.GetEntryAssembly() ?? throw new InvalidOperationException());

        do
        {
            var reference = stack.Pop();

            if (IsValidAssembly(prefix, reference))
            {
                yield return reference;
            }

            var referenced = reference.GetReferencedAssemblies().Where(name => IsValidAssembly(prefix, name));

            foreach (var name in referenced)
            {
                if (attendance.Contains(name.FullName) ||
                    Assembly.Load(name) is not { IsDynamic: false } assembly ||
                    string.IsNullOrWhiteSpace(assembly?.FullName))
                {
                    continue;
                }

                attendance.Add(assembly.FullName);

                stack.Push(assembly);
            }
        } while (stack.Count > 0);
    }

    #endregion
}
