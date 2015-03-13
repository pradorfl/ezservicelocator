using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EZServiceLocation
{
    public class ConstructorDependency
    {
        private Type[] _constructorParameters;

        public Type[] ConstructorParameters { get { return _constructorParameters; } }

        public ConstructorDependency(params Type[] constructorParameters)
        {
            _constructorParameters = constructorParameters;
        }

        public TObject GetInstance<TObject>(object[] parameters) where TObject : class
        {
            if (parameters == null)
                return default(TObject);

            return typeof(TObject).GetConstructor(_constructorParameters).Invoke(parameters) as TObject;
        }
    }
}
