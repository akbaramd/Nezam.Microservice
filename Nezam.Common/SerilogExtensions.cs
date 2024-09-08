using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Nezam.Common;

public static class SerilogExtensions
{
    public static IHostBuilder UseNezamSerilog(this IHostBuilder builder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();

        return builder.UseSerilog();
    }
}