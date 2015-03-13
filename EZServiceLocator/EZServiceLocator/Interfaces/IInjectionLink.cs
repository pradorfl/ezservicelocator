using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZServiceLocation.Interfaces
{
    public interface IInjectionLink
    {
        void Use(ConstructorDependency dependencies, bool threadScope = true);
    }
}
