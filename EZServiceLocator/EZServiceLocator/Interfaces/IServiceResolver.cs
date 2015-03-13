
namespace EZServiceLocation.Interfaces
{
    public interface IServiceResolver
    {
        TInterface GetService<TInterface>(bool throwError = true) where TInterface : class;
        TInterface GetService<TInterface>(string instanceName, bool throwError = true) where TInterface : class;
    }
}
