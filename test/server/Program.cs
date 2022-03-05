using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                    {
                        webBuilder.ConfigureKestrel(item =>
                        {
                            item.ListenAnyIP(5001);
                        });
                    }
                    webBuilder.UseStartup<Startup>();
                });
    }
}
