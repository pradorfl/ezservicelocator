using EZServiceLocation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace EZServiceLocation
{
    public class ObjectLink<TObject> : GenericLink<TObject>, IObjectLink where TObject : class, new()
    {
        private static readonly object _lock = new Object();

        public ObjectLink() 
        {
            _threadInstances = new Dictionary<int, TObject>();
        }

        public void Use(bool lazyTnstance = true, bool threadScope = true)
        {
            _isThreadScope = threadScope;

            if (!lazyTnstance)
            {
                if (_isThreadScope)
                    _threadInstances.Add(Thread.CurrentThread.ManagedThreadId, new TObject());
                else
                    _serviceInstance = new TObject();
            }
        }

        internal override TObject Invoke(bool requiresNew = false, object[] parameters = null)
        {
            if (_isThreadScope)
            {
                lock (_lock)
                {
                    if (requiresNew || !_threadInstances.ContainsKey(Thread.CurrentThread.ManagedThreadId))
                        _threadInstances[Thread.CurrentThread.ManagedThreadId] = new TObject();
                }

                return (TObject)_threadInstances[Thread.CurrentThread.ManagedThreadId];
            }
            else
            {
                lock (_lock)
                {
                    if (requiresNew || _serviceInstance == null)
                        _serviceInstance = new TObject();
                }

                return (TObject)_serviceInstance;
            }
        }

        internal override object InvokeObject(bool requiresNew = false, object[] parameters = null)
        {
            return Invoke(requiresNew, parameters);
        }
    }
}
