using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApp.Entities;

namespace TestApp.Interfaces
{
    public interface ITracker : INavigationSearcher
    {
        void SaveNavigation(Navigation nav);
    }
}
