
using DataShapes.Model;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();

        // Add services to the container.
        ConfigurationManager configuration = builder.Configuration;

        var connection = builder.Configuration.GetConnectionString("default")
                        ?? throw new InvalidOperationException("Connection string 'default' not found.");

        // Add services to the container.
        builder.Services.AddDbContext<DataShapeContext>(
            options => options.UseNpgsql(connection));

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

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}

