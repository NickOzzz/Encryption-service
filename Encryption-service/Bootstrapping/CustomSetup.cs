using Encryption_service.Services;
using Microsoft.AspNetCore.DataProtection;

namespace Encryption_service.Bootstrapping
{
    public static class CustomSetup
    {
        public static IServiceCollection AddCustomSetup(this IServiceCollection services)
            => services
                 .AddTransient<ICrypticService, CrypticService>()
                 .AddTransient(_ => DataProtectionProvider.Create(Directory.GetCurrentDirectory()));
    }
}
