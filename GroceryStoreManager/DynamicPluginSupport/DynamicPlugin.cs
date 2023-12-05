using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace DynamicPluginSupport;

public class DynamicPlugin
{
    private static readonly FileInfo[] _myAssFiles;
    private static readonly AppDomain _domain;

    static DynamicPlugin()
    {
        var exePath = Assembly.GetExecutingAssembly().Location;
        var baseFolder = Path.GetDirectoryName(exePath) ?? "";
        // My default dll folder: _dlls
        var myAssDirectory = Path.Combine(baseFolder, "_dlls");
        // Get list of DLL files
        _myAssFiles = new DirectoryInfo(myAssDirectory).GetFiles("*.dll", SearchOption.TopDirectoryOnly);
        _domain = AppDomain.CurrentDomain;
    }

    public static IEnumerable<Type> GetImplements<IContract>()
    {
        // List of Contract's inhertance
        var implements = new List<Type>();

        // On dependent assembly: even if we've listed dependent assembly in list of files, it's still going to search for the denpendent assembly on the event below
        _domain.AssemblyResolve += LoadReferenceLib;

        // Load all assemblies from current working directory
        foreach (var fileInfo in _myAssFiles)
        {
            // Get all of the types in the dll
            var fileFullName = fileInfo.FullName;
            var types = Array.Empty<Type>();
            // Check if assembly: https://learn.microsoft.com/en-us/dotnet/standard/assembly/identify (the second method), and get its name
            try
            {
                var assName = AssemblyName.GetAssemblyName(fileFullName);
                var assembly = _domain.Load(assName);
                //var assembly = Assembly.Load(assName);
                //var assembly = Assembly.LoadFrom(fileFullName);
                types = assembly.GetTypes();
            }
            catch (BadImageFormatException)
            {
                // Debug.WriteLine("The file is not an assembly");
            }
            foreach (var type in types)
            {
                if (type.IsClass && !type.IsAbstract)
                {
                    if (typeof(IContract).IsAssignableFrom(type))
                    {
                        implements.Add(type);
                    }
                }
            }
        }
        if (implements.Count == 0)
        {
            throw new Exception("DLL not found for: " + typeof(IContract).FullName);
        }
        return implements;
    }

    static Assembly LoadReferenceLib(object? sender, ResolveEventArgs args)
    {
        var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var assemblyPath = Path.Combine(folderPath, "_libs", new AssemblyName(args.Name).Name + ".dll");
        if (!File.Exists(assemblyPath))
        {
            assemblyPath = Path.Combine(folderPath, "_dlls", new AssemblyName(args.Name).Name + ".dll");
            if (!File.Exists(assemblyPath))
            {
                throw new Exception("Reference Assembly not found");
            }
        }
        var assembly = Assembly.LoadFrom(assemblyPath);
        return assembly;
    }
}
