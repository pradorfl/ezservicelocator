using System;
using System.Collections.Generic;
using EZServiceLocation.Interfaces;

namespace EZServiceLocation
{
    public abstract class ServiceContainer : IServiceLinker
    {
        protected IDictionary<Type, object> _links = new Dictionary<Type, object>();
        protected IDictionary<Type, Dictionary<string, object>> _namedLinks = new Dictionary<Type, Dictionary<string, object>>();

        public IServiceLink<TInterface> For<TInterface>()
        {
            if (!_links.ContainsKey(typeof(TInterface)))
                _links.Add(typeof(TInterface), new ServiceLink<TInterface>());

            return _links[typeof(TInterface)] as IServiceLink<TInterface>;
        }

        public IServiceLink<TInterface> For<TInterface>(string instanceName)
        {
            if (string.IsNullOrWhiteSpace(instanceName))
                return For<TInterface>();

            if (_namedLinks.ContainsKey(typeof(TInterface)))
            {
                var dic = _namedLinks[typeof(TInterface)];

                if (!dic.ContainsKey(instanceName))
                    dic.Add(instanceName, new ServiceLink<TInterface>());
                else
                    dic[instanceName] = new ServiceLink<TInterface>();
            }
            else
            {
                _namedLinks.Add(typeof(TInterface), new Dictionary<string, object>() { { instanceName, new ServiceLink<TInterface>() } });
            }

            return _namedLinks[typeof(TInterface)][instanceName] as IServiceLink<TInterface>;
        }

        public IObjectLink Use<TObject>(bool lazyTnstance = true, bool threadScope = true) where TObject : class, new()
        {
            if (!_links.ContainsKey(typeof(TObject)))
                _links.Add(typeof(TObject), new ObjectLink<TObject>());

            var link = _links[typeof(TObject)] as IObjectLink;

            link.Use(lazyTnstance, threadScope);

            return link;
        }

        public IInjectionLink Use<TObject>(ConstructorDependency dependencies, bool threadScope = true) where TObject : class
        {
            if (!_links.ContainsKey(typeof(TObject)))
                _links.Add(typeof(TObject), new InjectionLink<TObject>());

            var link = _links[typeof(TObject)] as IInjectionLink;

            link.Use(dependencies, threadScope);

            return link;
        }
    }
}
