using System;
using System.Collections.Generic;
using System.Threading;
using EZServiceLocation.Interfaces;

namespace EZServiceLocation
{
    public class ServiceLink<TInterface> : GenericLink<TInterface>, IServiceLink<TInterface>
    {
        private static readonly object _lock = new Object();
        protected Func<object[], TInterface> _getService;

        public ServiceLink() 
        {
            _theadInstances = new Dictionary<int, TInterface>();
        }

        public void Use<TObject>(bool lazyTnstance = true, bool threadScope = true) where TObject : TInterface, new()
        {
            _isThreadScope = threadScope;

            _getService = (parameters) => new TObject();

            if (!lazyTnstance)
            {
                if (_isThreadScope)
                    _theadInstances.Add(Thread.CurrentThread.ManagedThreadId, _getService(null));
                else
                    _serviceInstance = _getService(null);
            }
        }

        public void Use<TObject>(ConstructorDependency dependencies, bool threadScope = true) where TObject : class, TInterface
        {
            _isThreadScope = threadScope;
            _dependencies = dependencies;

            _getService = (parameters) => _dependencies.GetInstance<TObject>(parameters);
        }

        internal override TInterface Invoke(object[] parameters = null)
        {
            if (_getService != null)
            {
                if (_isThreadScope)
                {
                    lock (_lock)
                    {
                        if (!_theadInstances.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                            _theadInstances.Add(Thread.CurrentThread.ManagedThreadId, _getService(parameters));
                    }

                    return (TInterface)_theadInstances[Thread.CurrentThread.ManagedThreadId];
                }
                else
                {
                    lock (_lock)
                    {
                        if (_serviceInstance == null)
                            _serviceInstance = _getService(parameters);
                    }

                    return (TInterface)_serviceInstance;
                }
            }

            return default(TInterface);
        }

        internal override object InvokeObject(object[] parameters = null)
        {
            return Invoke(parameters);
        }
    }
}
