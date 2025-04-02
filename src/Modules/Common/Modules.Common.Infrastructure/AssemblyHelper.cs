namespace Modules.Common.Infrastructure;

public static class AssemblyHelper<T1, T2>
{
    public static bool SameModule()
    {
        return typeof(T1).Assembly.GetName().Name!.Split(",")[1] == typeof(T1).Assembly.GetName().Name!.Split(",")[1];
    }
}
