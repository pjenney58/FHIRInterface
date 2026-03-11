using Authentication.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Support.Model;
using System.Text;

namespace Authentication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;

            var connection = builder.Configuration.GetConnectionString("identity")
                            ?? throw new InvalidOperationException("Connection string 'identity' not found.");

            // Add services to the container.
            builder.Services.AddDbContext<IdentityDataContext>(
                options => options.UseNpgsql(connection));

            // For Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDataContext>()
                .AddDefaultTokenProviders();

            // Adding Authentication
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

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("PalisaidRootAdministrator", policy => policy.RequireClaim("PalisaidRootAdministrator"));
                options.AddPolicy("PalisaidTenantAdministrator", policy => policy.RequireClaim("PalisaidTenantAdministrator"));
                options.AddPolicy("PalisaidUser", policy => policy.RequireClaim("PalisaidUser"));

                options.AddPolicy("TenantRootAdministrator", policy => policy.RequireClaim("TenantRootAdministrator"));
                options.AddPolicy("TenantGroupAdministrator", policy => policy.RequireClaim("TenantGroupAdministrator"));
                options.AddPolicy("TenantGroupMember", policy => policy.RequireClaim("TenantGroupMember"));
                options.AddPolicy("TenantUser", policy => policy.RequireClaim("TenantUser"));
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

            // TODO: Turn on HTTPS redirection
            //app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            app.MapControllers();

            app.Run();
        }
    }
}