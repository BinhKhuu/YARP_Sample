import { Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { MsalGuard } from '@azure/msal-angular';

export const routes: Routes = [
    {
        path: '',
        canActivate: [MsalGuard],
        component: AppComponent
    }
];
