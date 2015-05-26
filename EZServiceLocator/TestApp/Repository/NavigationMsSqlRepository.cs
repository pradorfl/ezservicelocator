using System.Collections.Generic;
using System.Linq;
using System.Net;
using TestApp.Entities;
using TestApp.Interfaces;

namespace TestApp.Repository
{
    public class NavigationMsSqlRepository : INavigationRepository
    {
        private List<Navigation> _reg;

        public NavigationMsSqlRepository()
        {
            _reg = new List<Navigation>();
        }

        public void Save(Navigation nav)
        {
            System.Console.WriteLine(string.Format("NavigationMsSqlRepository.Save({0})", nav));

            var current = _reg.FirstOrDefault(n => n.Code == nav.Code);

            if (current != null)
                _reg.Remove(current);

            _reg.Add(nav);
        }

        public List<Navigation> Get(IPAddress clientIpAddress)
        {
            System.Console.WriteLine(string.Format("NavigationMsSqlRepository.Get({0})", clientIpAddress));

            return _reg.Where(n => n.ClientIpAddres.ToString().Equals(clientIpAddress.ToString())).ToList();
        }
    }
}
