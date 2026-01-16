import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Campaign, PlaylistItem, ProofOfPlay } from '../models/campaign.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CampaignService {
  private apiUrl = `${environment.apiUrl}/campaigns`;
  private screenApiUrl = `${environment.apiUrl}/screens`;
  private proofApiUrl = `${environment.apiUrl}/proofofplay`;

  constructor(private http: HttpClient) {}

  getCampaigns(): Observable<Campaign[]> {
    return this.http.get<Campaign[]>(this.apiUrl);
  }

  createCampaign(campaign: Campaign): Observable<Campaign> {
    return this.http.post<Campaign>(this.apiUrl, campaign);
  }

  updateCampaign(id: number, campaign: Campaign): Observable<Campaign> {
    return this.http.put<Campaign>(`${this.apiUrl}/${id}`, campaign);
  }

  deleteCampaign(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }

  getPlaylist(screenId: number, at?: string): Observable<PlaylistItem[]> {
    const options = at ? { params: { at } } : {};
    return this.http.get<PlaylistItem[]>(`${this.screenApiUrl}/${screenId}/playlist`, options);
  }

  recordProofOfPlay(proof: ProofOfPlay): Observable<any> {
    return this.http.post(this.proofApiUrl, proof);
  }
}
