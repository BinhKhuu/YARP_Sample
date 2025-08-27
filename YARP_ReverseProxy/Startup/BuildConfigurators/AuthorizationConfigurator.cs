using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace YARP_ReverseProxy.Startup.BuildConfigurators;

public class AuthorizationConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("jwt", policy =>
                {
                    policy.RequireAuthenticatedUser()
                        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                }

            );
            options.AddPolicy("cookie", policy =>
            {
                policy.RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(OpenIdConnectDefaults.AuthenticationScheme);
            });
        });
    }
}