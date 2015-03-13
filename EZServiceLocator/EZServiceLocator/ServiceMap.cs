using System;
using System.Collections.Generic;

namespace EZServiceLocation
{
    public abstract class ServiceMap : ServiceContainer
    {
        public abstract void Load();

        internal IDictionary<Type, object> Links
        {
            get { return _links; }
        }

        internal IDictionary<Type, Dictionary<string, object>> NamedLinks
        {
            get { return _namedLinks; }
        }
    }
}
