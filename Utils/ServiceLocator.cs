namespace imago.Utils;

public static class ServiceLocator
{
    public static IServiceProvider? Instance { get; set; }

    public static T Resolve<T>() where T : class
    {
        return Instance?.GetService<T>()!;
    }
}