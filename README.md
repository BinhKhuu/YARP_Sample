# Local debug
1. Start YARP_ReverseProxy (.NET)
2. Start YARP_Api project (.NET)
3. ```ng serve``` YARP_Client (angular)
4. Call YARP_ReverseProxy localhost:5044/web to load Client app
7. Call Yarp_ReverseProxy loclahost:5286/api to get to YARP_Api
   8. configured to validate bearer tokens**

# Docker debug
1. YARP_ReverseProxy Port mapping 5044:80 (localalhost 5044 to docker service 80)
2. YARP_API exposes Port mapping 5286:80 (localalhost 5286 to docker service 80)
3. YARP_Client Port mapping 4200:4200 (localalhost 4200 to docker service 4200)
   4. Client API request are mapped to YARP_ReverseProxy (5044) using your local machines localhost it doesn't need the service name
      5. service names are required for docker service to docker service communication
   5. YARP_ReverseProxy forwards request to 5286 (YARP_Api docker service) via the service name yarpapi:5286
6. Call YARP_ReverseProxy localhost:5044/web to load Client app
7. Call Yarp_ReverseProxy loclahost:5286/api to get to YARP_Api
   8. configured to validate bearer tokens


# YARP_ReverseProxy
### packages
```
Microsoft.Identity.Web
Yarp.ReverseProxy
```
### Reverse Proxy routes
1. Proxy address is 5044
1. /web to Web App (localhost:4200)
    2. protected via OIDC configuration (MicrosoftIdentityWebApp)
2. /api to Web Api (localhost:5231)
    3. protected via jwt configuration (MicrosoftIdentityWebApi)
4. / on the reverse proxy is unprotected, just displays a button that tries to call /api

Depnding on your debug choice (docker or localhost) update the cluster addresses
* use docker service name in docker compose : HostPort or call localhost directly if not using docker
```json
"Clusters": {
      "api": {
        "Destinations": {
          "destination1": {
            "Address": "http://yarpapi:8080"
          }
        }
      },
      "web": {
        "Destinations": {
          "destination1": {
            "Address": "http://yarpclient:4200"
          }
        }
      }
    }
```

### Notes
1. Rerverse Proxy returns 401 to ALL request to /api (because of the catch all)
    2. this includes routes that don't exist
3. Reverse Proxy redirects to login (oidc) on all /web routes
4. You can call /api route from reverse proxy without logging in (oidc) as long as you have a valid bearer token
    5. Note it is possible (i think) to use cookie auth for all routes and still configure jwt on the api
        6. but this means the cookie has to be long expiry otherwise its hard to refresh
    7. note it is possible to use different app registraion for different routes
8. There are JWT event subscripted to in the reverse proxy you can inspect the events.
9. Rerverse proxy validates the JWT then forwards and the API app also validates again because its configured to do so.
    10. you could let the reverse proxy do all the auth and have nothing on the api app

# API Configuration
1 .Set CORS Policy - allow reverse proxy (7141) access to api (5321)
  - 7141 is the https host 
2. Set Authentication and authorisation police
   3. Validating twice onece on reverse proxy then again on api

Update Cors settings to allow host addresses based on Docker service names or localhosts depending on your debug choice
```csharp
builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowAngularApp", policy =>
   {
       policy.WithOrigins("https://localhost:7141","http://localhost:5044") // Your Angular app URL
           .AllowAnyMethod()
           .AllowAnyHeader()
           .AllowCredentials(); // If you need to send credentials
   });
});
```

# Web (client) Configuration
1. Set MSAL to retrieve token from same registraion API is configured to validate against
2. Set MSAL Intercetor config to watch reverse proxy domain (7141)

Depending on if you are debuging locally or using docker set the baseURL in the Api service to call the reverse proxy
* api requests are executed in the browser so you don't need to use the docker service name
```js
export class ApiService {
  private baseURL: string = "http://localhost:5044/api"
```


# App Registration Configuration
## Required Settings
### Enable Token Types
- [ ] **ID Tokens** - Enable for user authentication
- [ ] **Access Tokens** - Enable for API access
### Redirect URLs
Add the following redirect URIs to your app registration:
```
<yourreverseproxy>/signin-oidc
<yourreverseproxy>/auth-redirect
```

> **Note**: These redirects are required when redirecting from web applications.

## API Configuration
### Create Custom Scopes
Create custom scopes for your exposed API:
```
api://1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d/Files.Read
api://1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d/user.access
```
### API Permissions
Add the following API permissions to your application:

- **Files.Read** - Read file access scope
- **user.access** - User access scope

### Audience Claim Alignment
The custom scopes are essential for aligning the tokens' `aud` (audience) claim, ensuring proper token validation and routing within your authentication flow.

> **Important**: Make sure to register these scopes in your API exposure settings to ensure proper audience matching between your application and the tokens it receives.

### Todo add web api and web app setup
* how to configure protected map (web)
* what the api service should call (web)