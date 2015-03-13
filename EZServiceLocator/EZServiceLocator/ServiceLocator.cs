using System;
using System.Collections.Generic;
using EZServiceLocation.Interfaces;

namespace EZServiceLocation
{
    public class ServiceLocator : ServiceContainer, IServiceResolver
    {
        private static readonly object _lock = new Object();
        private static ServiceLocator _current;

        public static ServiceLocator Current
        {
            get
            {
                lock (_lock)
                {
                    if (_current == null)
                        _current = new ServiceLocator();
                }

                return _current;
            }
        }

        private object ResolveDepedencies(BaseLink link)
        {
            object[] parameters = new object[link.Dependencies.Length];

            for (int i = 0; i < link.Dependencies.Length; i++)
            {
                if (_links.ContainsKey(link.Dependencies[i]))
                {
                    var dependency = _links[link.Dependencies[i]] as BaseLink;

                    parameters[i] = dependency.HasDependencies ? ResolveDepedencies(dependency) : dependency.InvokeObject();
                }
                else
                {
                    parameters[i] = null;
                }
            }

            return link.InvokeObject(parameters);
        }

        public void LoadServiceMap<TServiceMap>() where TServiceMap : ServiceMap, new()
        {
            var mapper = new TServiceMap();
            mapper.Load();

            foreach (Type interfaceKey in mapper.Links.Keys)
            {
                if (!_links.ContainsKey(interfaceKey))
                    _links.Add(interfaceKey, mapper.Links[interfaceKey]);
            }

            foreach (Type interfaceKey in mapper.NamedLinks.Keys)
            {
                if (!_namedLinks.ContainsKey(interfaceKey))
                    _namedLinks.Add(interfaceKey, mapper.NamedLinks[interfaceKey]);
            }
        }

        public TInterface GetService<TInterface>(bool throwError = true) where TInterface : class
        {
            try
            {
                var link = _links[typeof(TInterface)] as GenericLink<TInterface>;

                if (!link.HasInstance && link.HasDependencies)
                    return ResolveDepedencies(link) as TInterface;

                return link.Invoke();
            }
            catch (KeyNotFoundException)
            {
                if (!throwError)
                    return default(TInterface);

                throw new ApplicationException("The requested service is not registered");
            }
        }

        public TInterface GetService<TInterface>(string instanceName, bool throwError = true) where TInterface : class
        {
            try
            {
                var link = _namedLinks[typeof(TInterface)][instanceName] as GenericLink<TInterface>;

                if (!link.HasInstance && link.HasDependencies)
                    return ResolveDepedencies(link) as TInterface;

                return link.Invoke();
            }
            catch (KeyNotFoundException)
            {
                if (!throwError)
                    return default(TInterface);

                throw new ApplicationException("The requested service is not registered");
            }
        }
    }
}
