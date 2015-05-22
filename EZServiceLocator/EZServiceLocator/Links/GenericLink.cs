using System;
using System.Collections.Generic;

namespace EZServiceLocation
{
    public abstract class GenericLink<TInterface> : BaseLink
    {
        protected TInterface _serviceInstance;
        protected Dictionary<int, TInterface> _threadInstances;

        internal abstract TInterface Invoke(object[] parameters = null);

        public bool HasInstance { get { return _serviceInstance != null; } }
    }
}
