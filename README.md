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


### Todo add web api and web app setup
* how to configure api auth (notes on double validation, api) 
* how to configure cors (api)
* how to configure protected map (web)
* what the api service should call (web)