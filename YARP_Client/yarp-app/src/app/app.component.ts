import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import {MsalService} from '@azure/msal-angular';
import {ApiService} from './api.service';
@Component({
  selector: 'app-root',
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'yarp-app';

  constructor(
    private msalService: MsalService,
    private apiService: ApiService
  ) {
    this.msalService.handleRedirectObservable().subscribe();
  }

  doSomething(): void {
    this.apiService.getSomething()
      .subscribe(res => console.log(res));
  }
}
