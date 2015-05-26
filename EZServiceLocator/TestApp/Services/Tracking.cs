using System.Collections.Generic;
using System.Net;
using TestApp.Entities;
using TestApp.Interfaces;

namespace TestApp.Services
{
    public class Tracking : ITracker
    {
        private INavigationRepository _rep;

        public INavigationRepository Repository { get { return _rep; } set { _rep = value; } }

        public Tracking(INavigationRepository rep)
        {
            _rep = rep;
        }

        public void SaveNavigation(Navigation nav)
        {
            if (nav.IsValid())
                _rep.Save(nav);
        }

        public List<Navigation> GetClientNavigations(string clientIpAddress)
        {
            IPAddress ip;

            if (!string.IsNullOrEmpty(clientIpAddress) && IPAddress.TryParse(clientIpAddress, out ip))
                return _rep.Get(ip);

            return new List<Navigation>(0);
        }
    }
}
