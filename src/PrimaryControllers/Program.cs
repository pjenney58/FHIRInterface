using PalisaidMeta.Model;
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
        //configuration.AddJsonFile("controllerappsettings.json", optional: false, reloadOnChange: true);
        

        //Console.WriteLine($"Running InDocker: {AppRunningIn.Docker}");

        var dataconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker ? "containerdefault" : "default")
                        ?? throw new InvalidOperationException("Connection string 'default' not found.");

        var idconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker ? "containeridentity" : "identity")
                        ?? throw new InvalidOperationException("Connection string 'identity' not found.");

        // Migration -- Update-Database
        //     Update-Database -Context IdentityDataContext
        //     Update-Database -Context DataShapeContext

        ///Console.WriteLine($"Running in Docker: {AppRunningIn.Docker}");
        ///Console.WriteLine($"dataconnection = {dataconnection}");
        ///Console.WriteLine($"idconnection = {idconnection}");

        // Add services to the container.
        builder.Services.AddDbContext<PalisaidMetaContext>(options =>
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
            options.AddPolicy("PalisaidOwner", policy => policy.RequireClaim("PalisaidOwner"));
            options.AddPolicy("PalisaidRootAdministrator", policy => policy.RequireClaim("PalisaidRootAdministrator"));
            options.AddPolicy("PalisaidTenantAdministrator", policy => policy.RequireClaim("PalisaidTenantAdministrator"));
            options.AddPolicy("PalisaidUser", policy => policy.RequireClaim("PalisaidUser"));
            options.AddPolicy("TenantRootAdministrator", policy => policy.RequireClaim("TenantRootAdministrator"));
            options.AddPolicy("TenantGroupAdministrator", policy => policy.RequireClaim("TenantGroupAdministrator"));
            options.AddPolicy("TenantUserAdministrator", policy => policy.RequireClaim("TenantUserAdministrator"));
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

        /*
       .AddCookie()
       .AddOpenIdConnect(options =>
       {
           options.SignInScheme = "Cookies";
           options.Authority = "-your-identity-provider-";
           options.RequireHttpsMetadata = true;
           options.ClientId = "-your-clientid-";
           options.ClientSecret = "-your-client-secret-from-user-secrets-or-keyvault";
           options.ResponseType = "code";
           options.UsePkce = true;
           options.Scope.Add("profile");
           options.SaveTokens = true;
       });
        */

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

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Seed the db if it's not setup yet
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            SeedRoles.Initialize(services);
            SeedUsers.Initialize(services);
        }

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

    public static class SeedUsers
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (UserManager<ApplicationUser> _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>())
            {
                // Create root user
                var user = new RegisterAdminModel()
                {
                    Username = "root",
                    Password = "!Password0",
                    Email = "root@palisaid.com",
                    Phone = "603.264.3961"
                };

                // TODO: ALTER TABLE AspNetUserRoles DROP CONSTRAINT FK_AspNetUserRoles_AspNetRoles_UserId;
                // Manually deleting it works fine and user/role can be saved

                if (!_userManager.Users.Any(r => r.UserName == user.Username))
                {
                    var newuser = new ApplicationUser
                    {
                        UserName = user.Username,
                        Email = user.Username,
                        PhoneNumber = user.Phone,
                        TwoFactorEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        TenantId = Guid.Empty
                    };

                    try
                    {
                        Task.Run(async () => await _userManager.AddToRolesAsync(newuser,
                            new string[]
                            {
                            "PalisaidOwner",
                            "PalisaidRootAdministrator",
                            "PalisaidTenantAdministrator",
                            "PalisaidUser",
                            "Everyone"
                            }
                            )).Wait();
                    }
                    catch (Exception ex)
                    {
                        // hit constraint
                    }

                    Task.Run(async () => await _userManager.CreateAsync(newuser, user.Password)).Wait();
                }
            }
        }
    }

    public static class SeedRoles
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new IdentityDataContext(serviceProvider.GetRequiredService<DbContextOptions<IdentityDataContext>>()))
            {
                string[] roles = new string[]
                {
                    "PalisaidOwner",
                    "PalisaidRootAdministrator",
                    "PalisaidTenantAdministrator",
                    "PalisaidUser",
                    "TenantOwner",
                    "TenantRootAdministrator",
                    "TenantGroupAdministrator",
                    "TenantUserAdministrator",
                    "TenantUser",
                    "Everyone"
                };

                var newrolelist = new List<IdentityRole>();
                foreach (string role in roles)
                {
                    if (!context.Roles.Any(r => r.Name == role))
                    {
                        newrolelist.Add(new IdentityRole()
                        {
                            Name = role,
                            NormalizedName = role.ToUpper(),
                            ConcurrencyStamp = Guid.NewGuid().ToString()
                        });
                    }
                }
                context.Roles.AddRange(newrolelist);
                context.SaveChanges();
            }
        }

      //  private void RemoveConstraints()
      //  {
      //      // ALTER TABLE AspNetUserRoles DROP CONSTRAINT FK_AspNetUserRoles_AspNetRoles_UserId;
      //  }
    }
}