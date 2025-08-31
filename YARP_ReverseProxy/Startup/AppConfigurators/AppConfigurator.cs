namespace YARP_ReverseProxy.Startup.AppConfigurators;

public class AppConfigurator
{
    public static void Configure(WebApplication app, WebApplicationBuilder builder)
    {
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapReverseProxy();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        //app.UseHttpsRedirection();
    }
}