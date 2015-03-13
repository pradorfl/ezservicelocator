
namespace EZServiceLocation.Interfaces
{
    public interface IServiceLink<TInterface>
    {
        void Use<TObject>(bool lazyTnstance = true, bool threadScope = true) where TObject : TInterface, new();

        void Use<TObject>(ConstructorDependency dependencies, bool threadScope = true) where TObject : class, TInterface;
    }
}
