using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.KeyCloak
{
    public static class KeyCloak
    {
        public static IServiceCollection AddKeyCloak(this IServiceCollection services)
        {
            var keyCloakBaseUrl = Environment.GetEnvironmentVariable("KEYCLOAK_BASE_URL") ??
                                  throw new InvalidOperationException("KEYCLOAK_BASE_URL Not Found!");

            var keyCloakClientId = Environment.GetEnvironmentVariable("KEYCLOAK_CLIENT_ID") ??
                                   throw new InvalidOperationException("KEYCLOAK_CLIENT_ID Not Found!");

            //if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            //    services.AddScoped<IAuthorizationHandler, AllowAnonymous>();
            //else
            services.AddTransient<IClaimsTransformation, ClaimsTransformer>();

            services
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"{keyCloakBaseUrl}/auth/realms/master";
                    options.Audience = keyCloakClientId;
                    options.IncludeErrorDetails = true;
                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = true,
                        ValidIssuer = keyCloakBaseUrl,
                        ValidateLifetime = false
                    };
                });

            return services;
        }

        public static IApplicationBuilder UseKeyCloak(this IApplicationBuilder app)
        {
            app.UseAuthentication();

            app.UseAuthorization();

            return app;
        }
    }
}