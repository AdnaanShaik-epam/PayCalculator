import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-all-time-entries',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './all-time-entries.component.html',
  styleUrls: ['./all-time-entries.component.css']
})
export class AllTimeEntriesComponent implements OnInit {
  periodStart = '';
  periodEnd = '';
  entries: any[] = [];
  error = '';

  constructor(private api: ApiService) { }

  ngOnInit(): void { }

  load() {
    this.error = '';
    if (!this.periodStart || !this.periodEnd) { this.error = 'Select both dates'; return; }
    this.api.getAllTimeEntries(this.periodStart, this.periodEnd).subscribe({
      next: (res: any[]) => this.entries = res,
      error: (err: any) => this.error = err?.error || 'Failed to load entries'
    });
  }
}
