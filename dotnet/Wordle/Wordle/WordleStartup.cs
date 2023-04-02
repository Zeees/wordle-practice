using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SharedResources.Settings;
using System.Reflection;
using Wordle.Contexts;
using Wordle.Models.Mapping;
using Wordle.Repos.Dictonary;
using Wordle.Repos.Wordle;
using Wordle.Services;

namespace Wordle
{
    public static class WordleStartup
    {
        public static void Startup(IServiceCollection services, AppSettings appSettings)
        {
            services.AddScoped<IWordleService, WordleService>();
            services.AddScoped<IWordleRepo, WordleRepo>();
            services.AddScoped<IDictonaryRepo, DictonaryRepo>();

            services.AddAutoMapper(typeof(GameInfoMappingProfile));

            services.ConfigureSwaggerGen(x =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            services.AddDbContextFactory<WordleDatabaseContext>(x =>
            {
                if (string.IsNullOrEmpty(appSettings.DatabaseSettings.ConnectionString))
                {
                    throw new ArgumentException("Missing or empty connection string in configuration.");
                }

                x.UseSqlServer(appSettings.DatabaseSettings.ConnectionString);
            });

            services.AddHttpClient("RandomDictonary", x =>
            {
                x.BaseAddress = new Uri(appSettings.DictonarySettings.RandomAPIUrl);
            });

            services.AddHttpClient("Dictonary", x =>
            {
                x.BaseAddress = new Uri(appSettings.DictonarySettings.APIUrl);
            });
        }
    }
}
