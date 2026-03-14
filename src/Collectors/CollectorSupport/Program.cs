using Collector.Data;
using Collector.Messaging;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;

namespace Collector.Model
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration config = new ConfigurationBuilder()
              .AddJsonFile("collectorsupportsettings.json")
              .Build();

            builder.Services.AddDbContext<CollectorDataContext>(
                options => options.UseNpgsql(config.GetConnectionString("default")));

            builder.Services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.SetDefaultCulture("en-Us");
                options.AddSupportedUICultures("en-US", "de-DE", "ja-JP");
                options.FallBackToParentUICultures = true;

                options
                .RequestCultureProviders
                .Remove((IRequestCultureProvider)typeof(AcceptLanguageHeaderRequestCultureProvider));
            });

            builder.Services.AddTransient<MessageService>();

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // TODO: Turn on https redirect
            //app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseRequestLocalization();

            app.MapControllers();

            app.Run();
        }
    }
}