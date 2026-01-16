import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CampaignService } from '../../services/campaign.service';
import { ScreenService } from '../../services/screen.service';
import { AdService } from '../../services/ad.service';
import { Campaign, CampaignCreateRequest } from '../../models/campaign.model';
import { Screen } from '../../models/screen.model';
import { Ad } from '../../models/ad.model';

@Component({
  selector: 'app-campaigns',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './campaigns.component.html',
  styleUrls: ['./campaigns.component.css']
})
export class CampaignsComponent implements OnInit {
  campaigns: Campaign[] = [];
  screens: Screen[] = [];
  ads: Ad[] = [];
  showForm = false;
  editingCampaign: Campaign | null = null;

  // Form data for binding
  formData = {
    name: '',
    screenIds: [] as number[],
    adIds: [] as number[],
    startTime: '',
    endTime: '',
    playOrder: 1
  };

  constructor(
    private campaignService: CampaignService,
    private screenService: ScreenService,
    private adService: AdService
  ) {}

  ngOnInit(): void {
    this.loadCampaigns();
    this.loadScreens();
    this.loadAds();
  }

  loadCampaigns(): void {
    this.campaignService.getCampaigns().subscribe({
      next: (campaigns) => this.campaigns = campaigns,
      error: (err) => console.error('Error loading campaigns:', err)
    });
  }

  loadScreens(): void {
    this.screenService.getScreens().subscribe({
      next: (screens) => this.screens = screens,
      error: (err) => console.error('Error loading screens:', err)
    });
  }

  loadAds(): void {
    this.adService.getAds().subscribe({
      next: (ads) => this.ads = ads,
      error: (err) => console.error('Error loading ads:', err)
    });
  }

  openForm(campaign?: Campaign): void {
    if (campaign) {
      this.editingCampaign = campaign;
      
      // Extract screen IDs from campaignScreens
      const screenIds = campaign.campaignScreens?.map(cs => cs.screenId) || [];
      
      // Extract ad IDs from campaignAds
      const adIds = campaign.campaignAds?.map(ca => ca.adId) || [];
      
      this.formData = {
        name: campaign.name,
        screenIds: screenIds,
        adIds: adIds,
        startTime: this.formatDateForInput(new Date(campaign.startTime)),
        endTime: this.formatDateForInput(new Date(campaign.endTime)),
        playOrder: campaign.playOrder || 1
      };
    } else {
      this.editingCampaign = null;
      const now = new Date();
      const later = new Date(now.getTime() + 3600000); // +1 hour
      
      this.formData = {
        name: '',
        screenIds: [],
        adIds: [],
        startTime: this.formatDateForInput(now),
        endTime: this.formatDateForInput(later),
        playOrder: 1
      };
    }
    this.showForm = true;
  }

  closeForm(): void {
    this.showForm = false;
    this.editingCampaign = null;
  }

  // NEW: Check if screen is selected
  isScreenSelected(screenId: number): boolean {
    return this.formData.screenIds.includes(screenId);
  }

  // NEW: Check if ad is selected
  isAdSelected(adId: number): boolean {
    return this.formData.adIds.includes(adId);
  }

  // NEW: Toggle screen selection
  toggleScreen(screenId: number, event: any): void {
    if (event.target.checked) {
      if (!this.formData.screenIds.includes(screenId)) {
        this.formData.screenIds.push(screenId);
      }
    } else {
      this.formData.screenIds = this.formData.screenIds.filter(id => id !== screenId);
    }
  }

  // NEW: Toggle ad selection
  toggleAd(adId: number, event: any): void {
    if (event.target.checked) {
      if (!this.formData.adIds.includes(adId)) {
        this.formData.adIds.push(adId);
      }
    } else {
      this.formData.adIds = this.formData.adIds.filter(id => id !== adId);
    }
  }

  saveCampaign(): void {
    // Validate form
    if (!this.formData.name || !this.formData.startTime || !this.formData.endTime) {
      alert('Please fill in all required fields');
      return;
    }

    if (this.formData.screenIds.length === 0) {
      alert('Please select at least one screen');
      return;
    }

    if (this.formData.adIds.length === 0) {
      alert('Please select at least one advertisement');
      return;
    }

    // Prepare request to match backend DTO
    const request: CampaignCreateRequest = {
      name: this.formData.name,
      startTime: new Date(this.formData.startTime).toISOString(),
      endTime: new Date(this.formData.endTime).toISOString(),
      screenIds: this.formData.screenIds,
      adIds: this.formData.adIds,
      playOrder: this.formData.playOrder
    };

    if (this.editingCampaign?.id) {
      this.campaignService.updateCampaign(this.editingCampaign.id, request).subscribe({
        next: () => {
          this.loadCampaigns();
          this.closeForm();
        },
        error: (err) => {
          console.error('Error updating campaign:', err);
          alert(err.error?.message || 'Error updating campaign');
        }
      });
    } else {
      this.campaignService.createCampaign(request).subscribe({
        next: () => {
          this.loadCampaigns();
          this.closeForm();
        },
        error: (err) => {
          console.error('Error creating campaign:', err);
          alert(err.error?.message || 'Error creating campaign');
        }
      });
    }
  }

  deleteCampaign(id?: number): void {
    if (id && confirm('Are you sure you want to delete this campaign?')) {
      this.campaignService.deleteCampaign(id).subscribe({
        next: () => this.loadCampaigns(),
        error: (err) => console.error('Error deleting campaign:', err)
      });
    }
  }

  formatDateForInput(date: Date): string {
    return date.toISOString().slice(0, 16);
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }

  getScreenName(screenId: number): string {
    return this.screens.find(s => s.id === screenId)?.name || 'Unknown';
  }

  getAdName(adId: number): string {
    return this.ads.find(a => a.id === adId)?.title || 'Unknown';
  }

  getScreenNames(campaign: Campaign): string {
    if (!campaign.campaignScreens || campaign.campaignScreens.length === 0) {
      return 'None';
    }
    return campaign.campaignScreens
      .map(cs => this.getScreenName(cs.screenId))
      .join(', ');
  }

  getAdNames(campaign: Campaign): string {
    if (!campaign.campaignAds || campaign.campaignAds.length === 0) {
      return 'None';
    }
    return campaign.campaignAds
      .map(ca => this.getAdName(ca.adId))
      .join(', ');
  }
}