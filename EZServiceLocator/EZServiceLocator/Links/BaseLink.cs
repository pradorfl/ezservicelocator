using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZServiceLocation
{
    public abstract class BaseLink
    {
        protected bool _isThreadScope;
        protected ConstructorDependency _dependencies;

        internal abstract object InvokeObject(bool requiresNew = false, object[] parameters = null);

        public bool HasDependencies { get { return _dependencies != null; } }
        public Type[] Dependencies { get { return _dependencies.ConstructorParameters; } }
    }
}
