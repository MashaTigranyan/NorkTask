using System.IdentityModel.Tokens.Jwt;
using System.Text;
using MariamApp.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using db = MariamApp.Data;

namespace MariamApp.Helpers.ServiceExtensions;

public static class JwtAuthenticationExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwtSettings = config.GetSection("JwtSettings");
        var secret = jwtSettings["Secret"]; // Correct value!
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
            ClockSkew = TimeSpan.Zero
        };

        services.AddIdentity<ApplicationUser, Role>()
            .AddEntityFrameworkStores<db.AppUsersDbContext>()
            .AddDefaultTokenProviders();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.ClaimsIssuer = jwtSettings["Issuer"];
            options.TokenValidationParameters = validationParameters;

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                    {
                        context.Token = accessToken;
                        
                        var tokenHandler = new JwtSecurityTokenHandler();

                        try
                        {
                            tokenHandler.ValidateToken(accessToken, validationParameters, out SecurityToken validatedToken);
                        }
                        catch (Exception ex)
                        {
                            return context.Response.WriteAsync("Invalid token");
                        }

                    }
                    return Task.CompletedTask;
                }
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Administrator"));
            options.AddPolicy("OperatorOnly", policy => policy.RequireRole("Operator"));
        });

        return services;
    }
}
