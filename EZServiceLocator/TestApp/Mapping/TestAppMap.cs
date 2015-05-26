using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EZServiceLocation;
using TestApp.Interfaces;
using TestApp.Repository;
using TestApp.Services;

namespace TestApp.Mapping
{
    public class TestAppMap : ServiceMap
    {
        public override void Load()
        {
            //Fluent mappings with lazy load instance
            For<INavigationRepository>().Use<NavigationCassandraRepository>();
            //If the class has a constructor dependency send it here
            For<ITracker>().Use<Tracking>(new ConstructorDependency(typeof(INavigationRepository)));
        }
    }
}
