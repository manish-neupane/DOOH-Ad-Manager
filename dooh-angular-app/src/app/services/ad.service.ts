import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Ad } from '../models/ad.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AdService {
  private apiUrl = `${environment.apiUrl}/ads`;

  constructor(private http: HttpClient) {}

  getAds(): Observable<Ad[]> {
    return this.http.get<Ad[]>(this.apiUrl);
  }

  uploadAd(name: string, duration: number, file: File): Observable<Ad> {
    const formData = new FormData();
    formData.append('name', name);
    formData.append('duration', duration.toString());
    formData.append('file', file);

    return this.http.post<Ad>(`${this.apiUrl}/upload`, formData);
  }

  deleteAd(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
