
using System.Diagnostics;
using System.Net;
using DataShapes.Model;
using Authentication.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Support.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

        //Console.WriteLine($"Running InDocker: {AppRunningIn.Docker}");

        var dataconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker ? "containerdefault" : "default")
                        ?? throw new InvalidOperationException("Connection string 'default' not found.");

        var idconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker ? "containeridentity" : "identity")
                        ?? throw new InvalidOperationException("Connection string 'identity' not found.");

        //Console.WriteLine($"dataconnection = {dataconnection}");
        //Console.WriteLine($"idconnection = {idconnection}");

        // Add services to the container.
        builder.Services.AddDbContext<DataShapeContext>(options => 
            options.UseNpgsql(dataconnection));

        // Add security
        builder.Services.AddDbContext<IdentityDataContext>(options => 
            options.UseNpgsql(idconnection));

        // For Identity
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<IdentityDataContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("PalisaidRootAdministrator", policy => policy.RequireClaim("PalisaidRootAdministrator"));
            options.AddPolicy("PalisaidTenantAdministrator", policy => policy.RequireClaim("PalisaidTenantAdministrator"));
            options.AddPolicy("PalisaidUser", policy => policy.RequireClaim("PalisaidUser"));       

            options.AddPolicy("TenantRootAdministrator", policy => policy.RequireClaim("TenantRootAdministrator"));
            options.AddPolicy("TenantGroupAdministrator", policy => policy.RequireClaim("TenantGroupAdministrator"));
            options.AddPolicy("TenantGroupMember", policy => policy.RequireClaim("TenantGroupMember"));
            options.AddPolicy("TenantUser", policy => policy.RequireClaim("TenantUser"));

            options.AddPolicy("Everyone", policy => policy.RequireClaim("Everyone"));
        });

        // Adding Token management
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
       .AddJwtBearer(jwtoptions =>
       {
           jwtoptions.SaveToken = true;
           jwtoptions.RequireHttpsMetadata = false;
           jwtoptions.TokenValidationParameters = new TokenValidationParameters()
           {
               ValidateIssuer = true,
               ValidateAudience = true,
               ValidateLifetime = true,
               ValidateIssuerSigningKey = true,
               ClockSkew = TimeSpan.Zero,

               ValidAudience = configuration["JWT:ValidAudience"],
               ValidIssuer = configuration["JWT:ValidIssuer"],
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"] ?? throw new ArgumentNullException("JWT:Secret")))
           };
       });

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

