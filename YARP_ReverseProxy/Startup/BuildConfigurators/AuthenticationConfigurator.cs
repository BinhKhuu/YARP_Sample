using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace YARP_ReverseProxy.Startup.BuildConfigurators;

public static class AuthenticationConfigurator
{
    public static void Configure(WebApplicationBuilder builder)
    {
        // Add Authentication configuration for jwt bearer defaults used for APIs
        ConfigureWebApiAuthentication(builder);
        ConfigureWebAppAuthentication(builder);
    }

    private static void ConfigureWebApiAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApi(
            jwtOptions =>
            {
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"❌ Auth failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine($"✅ Token validated for: {context.Principal?.Identity?.Name}");
                        return Task.CompletedTask;
                    }
                };
            },
            identityOptions =>
            {
                builder.Configuration.Bind("AzureAd", identityOptions);
            });
    }

    private static void ConfigureWebAppAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme; 
        })
        .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdCookie"));
    }
    
}