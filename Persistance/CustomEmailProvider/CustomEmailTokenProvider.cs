using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Persistance.CustomEmailProvider
{
    public class CustomEmailTokenProvider<ApplicationUser> : DataProtectorTokenProvider<ApplicationUser> where ApplicationUser : class
    {
        public CustomEmailTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<CustomEmailTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<ApplicationUser>> logger)
            : base(dataProtectionProvider, options, logger) { }
    }
}