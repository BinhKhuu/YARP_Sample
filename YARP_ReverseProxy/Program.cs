using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using YARP_ReverseProxy.Startup.AppConfigurators;
using YARP_ReverseProxy.Startup.BuildConfigurators;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AuthenticationConfigurator.Configure(builder);
AuthorizationConfigurator.Configure(builder);
// this might be needed for OID and AddMicrosoftWebApp to enable oidc-sign controller routes.
builder.Services.AddControllers();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(transforms =>
    {
        // can transform request here
        // not if your request gets here it means its passed the Authpolicy configured for the route
        transforms.AddRequestTransform(async transformContext =>
        {
            var user = transformContext.HttpContext.User;
            if (user.Identity?.IsAuthenticated == true)
            {
                var name = user.FindFirst("name")?.Value;
                transformContext.ProxyRequest.Headers.Add("X-User-Name", name);
            }
        });
    });

var app = builder.Build();
AppConfigurator.Configure(app,builder);
RootControllerConfigurator.Configure(app);
app.Run();

