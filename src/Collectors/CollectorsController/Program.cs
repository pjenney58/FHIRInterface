using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PalisaidMeta.Model;
using Support.Model;

//using ChainOfResponsibility.Model;
//using ChainOfResponsibility.Interface;

namespace CollectorsController;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var dataconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker
                       ? "containerdefault"
                       : "default")
                       ?? throw new InvalidOperationException("Connection string 'default' not found.");

        // Add services to the container.
        builder.Services.AddDbContext<PalisaidMetaContext>(options =>
            options.UseNpgsql(dataconnection));

        // Add services to the container.
        //builder.Services.AddTransient<IChainOfResponsabilityHandler>();
        builder.Services.AddControllers();

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
       {
           c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
           {
               Type = SecuritySchemeType.Http,
               Scheme = "bearer",
               BearerFormat = "JWT",
               Description = "JWT Authorization header using the Bearer scheme."
           });
           c.AddSecurityRequirement(new OpenApiSecurityRequirement
           {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    new string[] {}
                }
           });
       });

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