namespace YARP_ReverseProxy.Startup.AppConfigurators;

public class RootControllerConfigurator
{
    public static void Configure(WebApplication app)
    {
        // simple getter to show something on home page for reverse proxy note it is not an authorized route
        app.MapGet("/", () =>
        {
            var html =
                @"<html>
            <body>
                <h2>Click the button below:</h2>
                <button id='myButton'>GetSomething unauthorised</button>
                <p id='result'></p>

                <script>
                    document.getElementById('myButton').addEventListener('click', function() {
                        fetch('/api/getSomething')
                            .then(response => response.text())
                            .then(data => {
                                document.getElementById('result').innerText = data;
                            });
                    });
                </script>
            </body>
        </html>";
    
            return Results.Content(html, "text/html");
        });
    }
}