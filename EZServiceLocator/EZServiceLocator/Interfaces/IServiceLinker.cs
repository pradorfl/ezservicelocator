
namespace EZServiceLocation.Interfaces
{
    public interface IServiceLinker
    {
        IServiceLink<TInterface> For<TInterface>();
        IServiceLink<TInterface> For<TInterface>(string instanceName);
        IObjectLink Use<TObject>(bool lazyTnstance = true, bool threadScope = true) where TObject : class, new();
        IInjectionLink Use<TObject>(ConstructorDependency dependencies, bool threadScope = true) where TObject : class;
    }
}
