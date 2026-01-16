import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdService } from '../../services/ad.service';
import { Ad } from '../../models/ad.model';

@Component({
  selector: 'app-ads',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './ads.component.html',
  styleUrls: ['./ads.component.css']
})
export class AdsComponent implements OnInit {
  ads: Ad[] = [];
  showForm = false;
  selectedFile: File | null = null;
  
  formData = {
    name: '',
    duration: 10
  };

  constructor(private adService: AdService) {}

  ngOnInit(): void {
    this.loadAds();
  }

  loadAds(): void {
    this.adService.getAds().subscribe({
      next: (ads) => {
        this.ads = ads;
        console.log('Loaded ads:', ads); // Check the URLs in console
      },
      error: (err) => console.error('Error loading ads:', err)
    });
  }

  openForm(): void {
    this.formData = { name: '', duration: 10 };
    this.selectedFile = null;
    this.showForm = true;
  }

  closeForm(): void {
    this.showForm = false;
    this.selectedFile = null;
  }

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
      if (!this.formData.name) {
        this.formData.name = file.name.replace(/\.[^/.]+$/, '');
      }
    }
  }

  uploadAd(): void {
    if (!this.selectedFile) {
      alert('Please select a file');
      return;
    }

    this.adService.uploadAd(this.formData.name, this.formData.duration, this.selectedFile).subscribe({
      next: () => {
        this.loadAds();
        this.closeForm();
      },
      error: (err) => console.error('Error uploading ad:', err)
    });
  }

  deleteAd(id?: number): void {
    if (id && confirm('Are you sure you want to delete this ad?')) {
      this.adService.deleteAd(id).subscribe({
        next: () => this.loadAds(),
        error: (err) => console.error('Error deleting ad:', err)
      });
    }
  }

  // ADD THIS METHOD - it handles the error
  onMediaError(event: any): void {
    console.error('Failed to load:', event.target.src);
  }

  formatFileSize(bytes?: number): string {
    if (!bytes) return 'N/A';
    const kb = bytes / 1024;
    const mb = kb / 1024;
    return mb >= 1 ? `${mb.toFixed(2)} MB` : `${kb.toFixed(2)} KB`;
  }

  formatDuration(seconds: number): string {
    if (seconds < 60) return `${seconds}s`;
    const minutes = Math.floor(seconds / 60);
    const secs = seconds % 60;
    return `${minutes}m ${secs}s`;
  }
}