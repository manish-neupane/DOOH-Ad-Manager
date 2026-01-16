import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CampaignService } from '../../services/campaign.service';
import { ScreenService } from '../../services/screen.service';
import { PlaylistItem } from '../../models/campaign.model';
import { Screen } from '../../models/screen.model';

@Component({
  selector: 'app-playlist',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit {
  screens: Screen[] = [];
  selectedScreenId: number = 0;
  selectedDateTime: string = '';
  playlist: PlaylistItem[] = [];
  loading = false;

  constructor(
    private campaignService: CampaignService,
    private screenService: ScreenService
  ) {}

  ngOnInit(): void {
    this.loadScreens();
    this.selectedDateTime = this.formatDateForInput(new Date());
  }

  loadScreens(): void {
    this.screenService.getScreens().subscribe({
      next: (screens) => {
        this.screens = screens;
        if (screens.length > 0 && !this.selectedScreenId) {
          this.selectedScreenId = screens[0].id || 0;
        }
      },
      error: (err) => console.error('Error loading screens:', err)
    });
  }

  loadPlaylist(): void {
    if (!this.selectedScreenId) {
      alert('Please select a screen');
      return;
    }

    this.loading = true;
    const atTime = this.selectedDateTime ? new Date(this.selectedDateTime).toISOString() : undefined;

    this.campaignService.getPlaylist(this.selectedScreenId, atTime).subscribe({
      next: (playlist) => {
        this.playlist = playlist;
        console.log('Loaded playlist:', playlist); // Check the data
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading playlist:', err);
        this.loading = false;
        alert('Error loading playlist. Make sure the backend is running.');
      }
    });
  }

  formatDateForInput(date: Date): string {
    return date.toISOString().slice(0, 16);
  }

  formatDuration(seconds: number): string {
    if (!seconds || isNaN(seconds)) return '0s'; // Handle NaN
    if (seconds < 60) return `${seconds}s`;
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}m ${secs}s`;
  }

  getTotalDuration(): string {
    const total = this.playlist.reduce((sum, item) => sum + (item.durationSeconds || 0), 0); // CHANGED: duration → durationSeconds
    return this.formatDuration(total);
  }
}