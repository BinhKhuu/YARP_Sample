import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {BrowserModule} from '@angular/platform-browser';
import {msalGuardConfig, msalInstance, msalInterceptorConfig} from './MSIdentityConfig';
import {
  MSAL_INSTANCE,
  MSAL_INTERCEPTOR_CONFIG,
  MsalBroadcastService,
  MsalInterceptor,
  MsalModule,
  MsalService
} from '@azure/msal-angular';
import {HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [
    {
      provide: MSAL_INSTANCE,
      useValue: msalInstance,
    },
    {
      provide: MSAL_INTERCEPTOR_CONFIG,
      useValue: msalInterceptorConfig,
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: MsalInterceptor,
      multi: true,
    },
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    importProvidersFrom(
      BrowserModule,
      MsalModule.forRoot(
        msalInstance,
        msalGuardConfig,
        msalInterceptorConfig
      ),
      MsalService,
      MsalBroadcastService
    ),
    provideHttpClient(withInterceptorsFromDi()),
    MsalInterceptor,
  ]
};
