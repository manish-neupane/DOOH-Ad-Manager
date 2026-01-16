import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProofOfPlayService {
  private apiUrl = `${environment.apiUrl}/screens/proofofplay`;

  constructor(private http: HttpClient) {}

  recordProofOfPlay(screenId: number, adId: number): Observable<any> {
    return this.http.post(this.apiUrl, { screenId, adId });
  }

  getProofOfPlayReport(screenId?: number, startDate?: string, endDate?: string): Observable<any[]> {
    let url = `${environment.apiUrl}/proofofplay`;
    const params: string[] = [];
    
    if (screenId) params.push(`screenId=${screenId}`);
    if (startDate) params.push(`startDate=${startDate}`);
    if (endDate) params.push(`endDate=${endDate}`);
    
    if (params.length > 0) {
      url += '?' + params.join('&');
    }
    
    return this.http.get<any[]>(url);
  }
}