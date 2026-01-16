import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ScreenService } from '../../services/screen.service';
import { Screen, ScreenStatus } from '../../models/screen.model';

@Component({
  selector: 'app-screens',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './screens.component.html',
  styleUrls: ['./screens.component.css']
})
export class ScreensComponent implements OnInit {
  screens: Screen[] = [];
  showForm = false;
  editingScreen: Screen | null = null;
  
  formData: Screen = {
    name: '',
    location: '',
    status: ScreenStatus.Active,
    resolution: '',
    orientation: ''
  };

  statuses = Object.values(ScreenStatus);

  constructor(private screenService: ScreenService) {}

  ngOnInit(): void {
    this.loadScreens();
  }

  loadScreens(): void {
    this.screenService.getScreens().subscribe({
      next: (screens) => this.screens = screens,
      error: (err) => console.error('Error loading screens:', err)
    });
  }

  openForm(screen?: Screen): void {
    if (screen) {
      this.editingScreen = screen;
      this.formData = { ...screen };
    } else {
      this.editingScreen = null;
      this.formData = {
        name: '',
        location: '',
        status: ScreenStatus.Active,
        resolution: '',
        orientation: ''
      };
    }
    this.showForm = true;
  }

  closeForm(): void {
    this.showForm = false;
    this.editingScreen = null;
  }

  saveScreen(): void {
    if (this.editingScreen?.id) {
      this.screenService.updateScreen(this.editingScreen.id, this.formData).subscribe({
        next: () => {
          this.loadScreens();
          this.closeForm();
        },
        error: (err) => console.error('Error updating screen:', err)
      });
    } else {
      this.screenService.createScreen(this.formData).subscribe({
        next: () => {
          this.loadScreens();
          this.closeForm();
        },
        error: (err) => console.error('Error creating screen:', err)
      });
    }
  }

  updateStatus(screen: Screen, newStatus: ScreenStatus): void {
    if (screen.id) {
      this.screenService.updateScreenStatus(screen.id, { status: newStatus }).subscribe({
        next: () => this.loadScreens(),
        error: (err) => console.error('Error updating status:', err)
      });
    }
  }

  deleteScreen(id?: number): void {
    if (id && confirm('Are you sure you want to delete this screen?')) {
      this.screenService.deleteScreen(id).subscribe({
        next: () => this.loadScreens(),
        error: (err) => console.error('Error deleting screen:', err)
      });
    }
  }

  getStatusClass(status: ScreenStatus): string {
    switch (status) {
      case ScreenStatus.Active:
        return 'status-active';
      case ScreenStatus.Inactive:
        return 'status-inactive';
      case ScreenStatus.Maintenance:
        return 'status-maintenance';
      default:
        return '';
    }
  }
}
