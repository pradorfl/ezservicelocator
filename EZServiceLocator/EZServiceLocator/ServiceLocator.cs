﻿using System;
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

        private bool HasNamedInstance(Type type, string instanceName)
        {
            return !string.IsNullOrWhiteSpace(instanceName)
                && _namedLinks.ContainsKey(type)
                && _namedLinks[type].ContainsKey(instanceName);
        }

        private object ResolveDepedencies(BaseLink link, bool requiresNew, string instanceName = null)
        {
            object[] parameters = new object[link.Dependencies.Length];

            for (int i = 0; i < link.Dependencies.Length; i++)
            {
                if (HasNamedInstance(link.Dependencies[i], instanceName))
                {
                    var dependency = _namedLinks[link.Dependencies[i]][instanceName] as BaseLink;

                    parameters[i] = dependency.HasDependencies ? ResolveDepedencies(dependency, requiresNew, instanceName) : dependency.InvokeObject(requiresNew);
                }
                else if (_links.ContainsKey(link.Dependencies[i]))
                {
                    if (_links.ContainsKey(link.Dependencies[i]))
                    {
                        var dependency = _links[link.Dependencies[i]] as BaseLink;

                        parameters[i] = dependency.HasDependencies ? ResolveDepedencies(dependency, requiresNew, instanceName) : dependency.InvokeObject(requiresNew);
                    }
                }
                else
                {
                    parameters[i] = null;
                }
            }

            return link.InvokeObject(requiresNew, parameters);
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
                foreach (string instanceName in mapper.NamedLinks[interfaceKey].Keys)
                {
                    if (_namedLinks.ContainsKey(interfaceKey))
                    {
                        if (!_namedLinks[interfaceKey].ContainsKey(instanceName))
                            _namedLinks[interfaceKey].Add(instanceName, mapper.NamedLinks[interfaceKey][instanceName]);
                    }
                    else
                    {
                        _namedLinks.Add(interfaceKey, mapper.NamedLinks[interfaceKey]);
                        break;
                    }
                }
            }
        }

        public TInterface GetService<TInterface>(bool throwError = true, bool requiresNew = false) where TInterface : class
        {
            try
            {
                var link = _links[typeof(TInterface)] as GenericLink<TInterface>;

                if (!link.HasInstance && link.HasDependencies)
                    return ResolveDepedencies(link, requiresNew) as TInterface;

                return link.Invoke(requiresNew);
            }
            catch (KeyNotFoundException)
            {
                if (!throwError)
                    return default(TInterface);

                throw new ApplicationException("The requested service is not registered");
            }
        }

        public TInterface GetService<TInterface>(string instanceName, bool throwError = true, bool requiresNew = false) where TInterface : class
        {
            try
            {
                var link = _namedLinks[typeof(TInterface)][instanceName] as GenericLink<TInterface>;

                if (!link.HasInstance && link.HasDependencies)
                    return ResolveDepedencies(link, requiresNew, instanceName) as TInterface;

                return link.Invoke(requiresNew);
            }
            catch (KeyNotFoundException)
            {
                if (!throwError)
                    return default(TInterface);

                throw new ApplicationException("The requested service is not registered");
            }
        }

        public object GetService(Type instance, bool throwError = true, bool requiresNew = false)
        {
            try
            {
                var link = _links[instance] as BaseLink;

                if (link.HasDependencies)
                    return ResolveDepedencies(link, requiresNew);

                return link.InvokeObject(requiresNew);
            }
            catch (KeyNotFoundException)
            {
                if (!throwError)
                    return default(object);

                throw new ApplicationException("The requested service is not registered");
            }
        }

        public object GetService(Type instance, string instanceName, bool throwError = true, bool requiresNew = false)
        {
            try
            {
                var link = _namedLinks[instance][instanceName] as BaseLink;

                if (link.HasDependencies)
                    return ResolveDepedencies(link, requiresNew, instanceName);

                return link.InvokeObject(requiresNew);
            }
            catch (KeyNotFoundException)
            {
                if (!throwError)
                    return default(object);

                throw new ApplicationException("The requested service is not registered");
            }
        }
    }
}
