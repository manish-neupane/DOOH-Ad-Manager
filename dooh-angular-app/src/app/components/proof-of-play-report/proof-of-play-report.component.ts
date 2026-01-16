import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProofOfPlayService } from '../../services/proof-of-play.service';
import { ScreenService } from '../../services/screen.service';
import { Screen } from '../../models/screen.model';

@Component({
  selector: 'app-proof-of-play-report',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './proof-of-play-report.component.html',
  styleUrls: ['./proof-of-play-report.component.css']
})
export class ProofOfPlayReportComponent implements OnInit {
  screens: Screen[] = [];
  selectedScreenId: number = 0;
  startDate: string = '';
  endDate: string = '';
  proofOfPlayData: any[] = [];
  loading = false;

  constructor(
    private proofOfPlayService: ProofOfPlayService,
    private screenService: ScreenService
  ) {}

  ngOnInit(): void {
    this.loadScreens();
    // Default to last 7 days
    const now = new Date();
    const weekAgo = new Date(now.getTime() - 7 * 24 * 60 * 60 * 1000);
    this.startDate = weekAgo.toISOString().slice(0, 10);
    this.endDate = now.toISOString().slice(0, 10);
  }

  loadScreens(): void {
    this.screenService.getScreens().subscribe({
      next: (screens) => this.screens = screens,
      error: (err) => console.error('Error loading screens:', err)
    });
  }

  loadReport(): void {
    this.loading = true;
    this.proofOfPlayService.getProofOfPlayReport(
      this.selectedScreenId || undefined,
      this.startDate,
      this.endDate
    ).subscribe({
      next: (data) => {
        this.proofOfPlayData = data;
        this.loading = false;
      },
      error: (err) => {
        console.error('Error loading report:', err);
        this.loading = false;
        alert('Error loading proof of play report');
      }
    });
  }

  formatDate(dateString: string): string {
    return new Date(dateString).toLocaleString();
  }

  exportToCSV(): void {
    const headers = ['Screen ID', 'Ad ID', 'Played At', 'Campaign ID'];
    const rows = this.proofOfPlayData.map(item => [
      item.screenId,
      item.adId,
      item.playedAt,
      item.campaignId || 'N/A'
    ]);

    const csv = [
      headers.join(','),
      ...rows.map(row => row.join(','))
    ].join('\n');

    const blob = new Blob([csv], { type: 'text/csv' });
    const url = window.URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `proof-of-play-${new Date().toISOString()}.csv`;
    a.click();
  }
}