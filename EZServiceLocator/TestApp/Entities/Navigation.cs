using System;
using System.Net;
using TestApp.Interfaces;

namespace TestApp.Entities
{
    public class Navigation : IValidation
    {
        public Guid Code { get; private set; }
        public IPAddress ClientIpAddres { get; private set; }
        public string PageURL { get; private set; }
        public string RefererURL { get; private set; }

        public Navigation(string ip, string page, string referer, Guid? code = null)
        {
            Code = code.HasValue ? code.Value : Guid.NewGuid();

            IPAddress ipObj;
            if (IPAddress.TryParse(ip, out ipObj))
                ClientIpAddres = ipObj;

            PageURL = page;
            RefererURL = referer;
        }

        public bool IsValid()
        {
            return !PageURL.Equals(RefererURL) && ClientIpAddres != null;
        }

        public override string ToString()
        {
            return string.Format("Code:{0} | ClientIpAddres:{1} | PageURL:{2} | RefererURL:{3}", Code, ClientIpAddres, PageURL, RefererURL);
        }
    }
}
