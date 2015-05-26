using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Entities;

namespace TestApp.Interfaces
{
    public interface INavigationSearcher
    {
        List<Navigation> GetClientNavigations(string clientIpAddress);
    }
}
