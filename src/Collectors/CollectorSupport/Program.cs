
namespace Collectors;
using Collectors.Data;
using Microsoft.EntityFrameworkCore;

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


        app.MapControllers();

        app.Run();
    }
}

