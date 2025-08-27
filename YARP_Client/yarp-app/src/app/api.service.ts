import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  private baseURL: string = "https://localhost:7141/api"
  constructor(
    private http: HttpClient
  ) { }

  getSomething(): Observable<any> {
    return this.http.get(`${this.baseURL}/getSomething`);
  }
}
