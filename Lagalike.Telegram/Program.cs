namespace Lagalike.Telegram
{
    using System;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    public class Program
    {
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
#if DEBUG
            return Host.CreateDefaultBuilder(args).
                        ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
#else
            var port = Environment.GetEnvironmentVariable("PORT");
            if (port is null)
            {
                throw new NullReferenceException("Not setted the bot port");
            }

            return Host.CreateDefaultBuilder(args).
                        ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>().UseUrls("http://*:" + port));
#endif
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
    }
}