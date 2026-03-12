using Authentication.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql;
using PalisaidMeta.Model;
using Support.Model;
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

        var dataconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker ? "containerdefault" : "default")
                        ?? throw new InvalidOperationException("Connection string 'default' not found.");

        var idconnection = builder.Configuration.GetConnectionString(AppRunningIn.Docker ? "containeridentity" : "identity")
                        ?? throw new InvalidOperationException("Connection string 'identity' not found.");

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
            /*c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "bearerAuth" }
                    },
                    new string[] {}
                }
            });*/
        });

        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            // Seed the db if it's not setup yet
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IdentityDataContext>();
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();

                var services = scope.ServiceProvider;
                SeedRoles.Initialize(services);
                SeedUsers.Initialize(services);
            }

            // Create the palisaid database if it doesn't exist
            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<PalisaidMetaContext>();
                dbContext.Database.Migrate();
                dbContext.Database.EnsureCreated();
            }

            // booooohGussss hack ...
            BogusConstraints.DropData(dataconnection);
            BogusConstraints.DropIdentity(idconnection);

            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        //app.UseAuthentication();
        //app.UseCors("AllowAll");
        app.MapControllers();

        app.Run();
    }

    public static class BogusConstraints
    {
        public static void DropData(string dataconnection)
        {
            using (var connection = new NpgsqlConnection(dataconnection))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("ALTER TABLE IF EXISTS \"Address\" DROP CONSTRAINT \"FK_Address_Tenants_TenantId\"", connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // hit constraint
                    }

                    try
                    {
                        command.CommandText = "ALTER TABLE IF EXISTS \"Contact\" DROP CONSTRAINT \"FK_Contact_Tenants_TenantId\"";
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // hit constraint
                    }
                }
            }
        }

        public static void DropIdentity(string idconnection)
        {
            using (var connection = new NpgsqlConnection(idconnection))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("ALTER TABLE \"AspNetUserRoles\" DROP CONSTRAINT \"FK_AspNetUserRoles_AspNetRoles_UserId\"", connection))
                {
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch
                    {
                        // hit constraint
                    }
                }
            }
        }
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

                if (!_userManager.Users.Any(r => r.UserName == user.Username))
                {
                    var newuser = new ApplicationUser
                    {
                        UserName = user.Username,
                        Email = user.Username,
                        PhoneNumber = user.Phone,
                        TwoFactorEnabled = false,
                        SecurityStamp = Guid.NewGuid().ToString(),
                        TenantId = BaseConstants.AdminId
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
                    catch
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