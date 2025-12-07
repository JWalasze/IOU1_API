using System.Reflection;

namespace IOU1.Application.Mappings;

public static class MappingConfig
{
    private const string _classNamePrefix = "MappingConfig";
    private const string _methodName = "Init";

    public static void Init()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            Type[] types;

            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                types = ex.Types.Where(t => t != null).ToArray()!;
            }

            foreach (var type in types.Where(t => 
                t is not null &&
                t.IsClass &&
                t.Name.EndsWith(_classNamePrefix, StringComparison.Ordinal) &&
                typeof(IMappingConfiguration).IsAssignableFrom(t)))
            {
                var method = type.GetMethod(_methodName);
                if (method is null)
                    continue;

                if (Activator.CreateInstance(type) is IMappingConfiguration mapping)
                    mapping.Init();
            }
        }
    }
    
}
