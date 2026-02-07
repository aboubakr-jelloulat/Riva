using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;

namespace Riva.API.Utils;


public static class JwtAuthenticationConfiguration
{
    public static IServiceCollection JwtConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var key = Encoding.ASCII.GetBytes(configuration.GetSection("JwtSettings")["Secret"]);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),

                ValidateIssuer = false,
                ValidateAudience = false,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,

                //NameClaimType = ClaimTypes.Name,
                //RoleClaimType = ClaimTypes.Role
            };
        });

        return services;
    }


    /*
        * When you use JWT authentication:
            Every protected request needs this header: Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...


        * Without Swagger/Scalar configuration →
                You must manually paste the token in headers for every request

        * With this configuration →
            Swagger/Scalar shows an Authorize  button You paste the token once Swagger/Scalar sends it automatically for all secured endpoints

     */

    public static IServiceCollection AddScalarBearerAuth(this IServiceCollection services, string versionName, string displayName
    )
    {
        services.AddOpenApi(versionName, options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                //document.Info = new OpenApiInfo
                //{
                //    Title = displayName,
                //    Version = versionName,
                //    Description = "API secured with JWT Bearer authentication",
                //    Contact = new OpenApiContact
                //    {
                //        Name = "API Support",
                //        Email = "support@example.com"
                //    }
                //};

                document.Components ??= new();

                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT",
                        Description = "Enter JWT token like: Bearer {your token}"
                    }
                };

                document.Security =
                [
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecuritySchemeReference("Bearer"),
                            new List<string>()
                        }
                    }
                ];

                return Task.CompletedTask;
            });
        });

        return services;
    }
}
