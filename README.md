# Start up
1. Start YARP_ReverseProxy (.NET)
2. Start YARP_Api project (.NET)
3. ```ng serve``` YARP_Client (angular)

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

# Web (client) Configuration
1. Set MSAL to retrieve token from same registraion API is configured to validate against
2. Set MSAL Intercetor config to watch reverse proxy domain (7141)


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