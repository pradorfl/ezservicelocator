using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EZServiceLocation.Interfaces;

namespace EZServiceLocation
{
    public class InjectionLink<TObject> : GenericLink<TObject>, IInjectionLink where TObject : class
    {
        private static readonly object _lock = new Object();
        protected Func<object[], TObject> _getService;

        public InjectionLink() 
        {
            _threadInstances = new Dictionary<int, TObject>();
        }

        public void Use(ConstructorDependency dependencies, bool threadScope = true)
        {
            _isThreadScope = threadScope;
            _dependencies = dependencies;

            _getService = (parameters) => _dependencies.GetInstance<TObject>(parameters);
        }

        internal override TObject Invoke(bool requiresNew = false, object[] parameters = null)
        {
            if (_getService != null)
            {
                if (_isThreadScope)
                {
                    lock (_lock)
                    {
                        if (requiresNew || !_threadInstances.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                            _threadInstances[Thread.CurrentThread.ManagedThreadId] = _getService(parameters);
                    }

                    return (TObject)_threadInstances[Thread.CurrentThread.ManagedThreadId];
                }
                else
                {
                    lock (_lock)
                    {
                        if (requiresNew || _serviceInstance == null)
                            _serviceInstance = _getService(parameters);
                    }

                    return (TObject)_serviceInstance;
                }
            }

            return default(TObject);
        }

        internal override object InvokeObject(bool requiresNew = false, object[] parameters = null)
        {
            return Invoke(requiresNew, parameters);
        }
    }
}
