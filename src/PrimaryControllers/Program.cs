
using System.Diagnostics;
using System.Net;
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

        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            Debug.WriteLine(ip.ToString());
        }
        
        var connection = builder.Configuration.GetConnectionString("default")
                        ?? throw new InvalidOperationException("Connection string 'default' not found.");

        // Add services to the container.
        builder.Services.AddDbContext<DataShapeContext>(
            options => options.UseNpgsql(connection));

        Debug.WriteLine($"Connection to postgres using {connection} appears to have worked");

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

