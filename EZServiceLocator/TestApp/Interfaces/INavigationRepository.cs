using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TestApp.Entities;

namespace TestApp.Interfaces
{
    public interface INavigationRepository
    {
        void Save(Navigation nav);
        List<Navigation> Get(IPAddress clientIpAddress);
    }
}
