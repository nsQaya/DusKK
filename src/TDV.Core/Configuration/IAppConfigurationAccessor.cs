using Microsoft.Extensions.Configuration;

namespace TDV.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
