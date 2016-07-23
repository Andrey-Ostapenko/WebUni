using System.Web;
using livemenu.Kernel.Configuration;

namespace livemenu.Services
{
    public class CurrentConfigurationService : ICurrentConfigurationService
    {
        public string MainDomain => HttpContext.Current.Request.Url.Host;
        public string MainDomainWithProtocol => $"{HttpContext.Current.Request.Url.Scheme}://{HttpContext.Current.Request.Url.Authority}";
    }
        
}
