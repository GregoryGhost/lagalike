namespace Lagalike.Telegram
{
    using System;

    using Lagalike.Telegram.Services;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly IConfigurationSection _botConfiguration;

        private readonly IHostEnvironment _environment;

        public Startup(IConfiguration configuration, IHostEnvironment environment)
        {
            _environment = environment;
            _botConfiguration = configuration.GetSection(TelegramBotConfiguration.CONFIGURATION_SECTION_NAME);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureTelegramMode(services);

            // Register named HttpClient to get benefits of IHttpClientFactory
            // and consume it with ITelegramBotClient typed client.
            // More read:
            //  https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-5.0#typed-clients
            //  https://docs.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests
            services.AddHttpClient("tgwebhook").AddTypedClient<ConfiguredTelegramBotClient>();

            // The Telegram.Bot library heavily depends on Newtonsoft.Json library to deserialize
            // incoming webhook updates and send serialized responses back.
            // Read more about adding Newtonsoft.Json to ASP.NET Core pipeline:
            //   https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/formatting?view=aspnetcore-5.0#add-newtonsoftjson-based-json-format-support
            services.AddControllers().AddNewtonsoftJson();

            services.Configure<TelegramBotConfiguration>(_botConfiguration);
        }

        private void ConfigureTelegramMode(IServiceCollection services)
        {
            services.AddMemoryCache(options => options.ExpirationScanFrequency = TimeSpan.FromHours(1));
            services.AddSingleton<TelegramConversationCache>();
            services.AddSingleton<HandleUpdateService>();

            if (_environment.IsDevelopment())
            {
                services.AddSingleton<PollingUpdateHandler>();
                services.AddHostedService<PollingConfigurator>();
            }
            else
            {
                services.AddSingleton<TelegramWebhookConfiguration>();
                services.AddHostedService<WebhookConfigurator>();
            }
        }
    }
}