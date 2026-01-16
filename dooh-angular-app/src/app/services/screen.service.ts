import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Screen, ScreenStatusUpdate } from '../models/screen.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ScreenService {
  private apiUrl = `${environment.apiUrl}/screens`;

  constructor(private http: HttpClient) {}

  getScreens(): Observable<Screen[]> {
    return this.http.get<Screen[]>(this.apiUrl);
  }

  getScreen(id: number): Observable<Screen> {
    return this.http.get<Screen>(`${this.apiUrl}/${id}`);
  }

  createScreen(screen: Screen): Observable<Screen> {
    return this.http.post<Screen>(this.apiUrl, screen);
  }

  updateScreen(id: number, screen: Screen): Observable<Screen> {
    return this.http.put<Screen>(`${this.apiUrl}/${id}`, screen);
  }

  updateScreenStatus(id: number, status: ScreenStatusUpdate): Observable<Screen> {
    return this.http.patch<Screen>(`${this.apiUrl}/${id}/status`, status);
  }

  deleteScreen(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
