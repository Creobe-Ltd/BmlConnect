using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Creobe.BmlConnect;

public static class DependencyInjection
{
    public static IServiceCollection AddBmlConnect(this IServiceCollection services, Action<BmlConnectOptions> configure)
    {
        services.Configure(configure);

        services.AddHttpClient<BmlConnectClient>((provider, client) =>
        {
            var options = provider.GetRequiredService<IOptions<BmlConnectOptions>>().Value;

            client.BaseAddress = new Uri(options.Endpoint);
            client.DefaultRequestHeaders.Add("Authorization", options.ApiKey);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });

        return services;
    }
}