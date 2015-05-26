using EZServiceLocation;
using TestApp.Entities;
using TestApp.Interfaces;
using TestApp.Mapping;
using TestApp.Repository;
using TestApp.Services;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Load fluent service mappings from mapping class
            ServiceLocator.Current.LoadServiceMap<TestAppMap>();

            //Get the instace of a service trough interface
            var trackingService = ServiceLocator.Current.GetService<ITracker>();

            Initialize(trackingService);
            Search(trackingService);

            //Specific mapping, with named instance
            ServiceLocator.Current.For<INavigationRepository>("sql").Use<NavigationMsSqlRepository>();

            //Get named instance of repository
            ((Tracking)trackingService).Repository = ServiceLocator.Current.GetService<INavigationRepository>("sql");

            Initialize(trackingService);
            Search(trackingService);
        }

        private static void Initialize(ITracker service)
        {
            //Debug-me
            service.SaveNavigation(new Navigation("192.168.0.1", "www.github.com", "www.google.com"));
            service.SaveNavigation(new Navigation("192.168.0.1", "www.google.com", "www.github.com"));
            service.SaveNavigation(new Navigation("192.168.0.2", "www.google.com", "www.github.com"));
        }

        private static void Search(ITracker service)
        {
            var clientNavs = service.GetClientNavigations("192.168.0.1");

            if (clientNavs != null)
                clientNavs.ForEach(n => { System.Console.WriteLine(n.ToString()); });
        }
    }
}
