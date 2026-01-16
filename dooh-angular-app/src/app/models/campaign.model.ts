export interface Campaign {
  id?: number;
  name: string;
  startTime: string;
  endTime: string;
  playOrder?: number;

  // Backend navigation properties
  campaignAds?: { adId: number; playOrder: number }[];
  campaignScreens?: { screenId: number }[];
}

export interface PlaylistItem {
  adId: number;
  title: string;           // CHANGED: was "adName"
  durationSeconds: number; // CHANGED: was "duration"
  mediaUrl: string;
  mediaType: string;
  playOrder: number;
}

export interface ProofOfPlay {
  screenId: number;
  adId: number;
  playedAt: string;
  campaignId?: number;
}

export interface CampaignCreateRequest {
  name: string;
  startTime: string;
  endTime: string;
  screenIds: number[];
  adIds: number[];
  playOrder?: number;
}