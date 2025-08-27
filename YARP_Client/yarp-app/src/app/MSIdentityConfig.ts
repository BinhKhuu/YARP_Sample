import {MsalGuardConfiguration, MsalInterceptorConfiguration} from '@azure/msal-angular';
import {BrowserCacheLocation, InteractionType, PublicClientApplication} from '@azure/msal-browser';

export const msalGuardConfig: MsalGuardConfiguration = {
  interactionType: InteractionType.Redirect,
};
export const msalInstance = new PublicClientApplication({
  auth: {
    clientId: '1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d',
    authority: 'https://login.microsoftonline.com/fb2492a7-4cfe-4506-95f9-2284aa380e41',
    redirectUri: 'http://localhost:5044/auth-redirect',
    postLogoutRedirectUri: 'http://localhost:5044'
  },
  cache: {
    cacheLocation: BrowserCacheLocation.SessionStorage
  },
  system: {
    loggerOptions: {
      loggerCallback: () => {},
      piiLoggingEnabled: false,
    },
  },
});
export const msalInterceptorConfig: MsalInterceptorConfiguration = {
  interactionType: InteractionType.Redirect,
  protectedResourceMap: new Map([
    ['http://localhost:4200/', ['api://1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d/Files.Read']],
    ['http://localhost:5044/', ['api://1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d/Files.Read']],
    ['https://localhost:7141',['api://1f92f546-9fa1-4e51-b2a5-25fadb5b5d1d/Files.Read']]
  ])
}
