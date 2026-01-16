import { Routes } from '@angular/router';
import { ScreensComponent } from './components/screens/screens.component';
import { AdsComponent } from './components/ads/ads.component';
import { CampaignsComponent } from './components/campaigns/campaigns.component';
import { PlaylistComponent } from './components/playlist/playlist.component';
import { ProofOfPlayReportComponent } from './components/proof-of-play-report/proof-of-play-report.component';

export const routes: Routes = [
  { path: '', redirectTo: '/screens', pathMatch: 'full' },
  { path: 'screens', component: ScreensComponent },
  { path: 'ads', component: AdsComponent },
  { path: 'campaigns', component: CampaignsComponent },
  { path: 'playlist', component: PlaylistComponent },
  { path: 'proof-of-play', component: ProofOfPlayReportComponent }
];