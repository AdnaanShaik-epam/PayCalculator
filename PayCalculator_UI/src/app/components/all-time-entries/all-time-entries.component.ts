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
  selectedEmployeeId: number | null = null;
  entries: any[] = [];
  employees: any[] = [];
  error = '';

  constructor(private api: ApiService) { }

  ngOnInit(): void {
    this.api.getEmployees().subscribe({
      next: (res: any[]) => this.employees = res,
      error: (err: any) => console.error('Failed to load employees', err)
    });
  }

  load() {
    this.error = '';
    if (!this.periodStart || !this.periodEnd) { this.error = 'Select both dates'; return; }
    this.api.getAllTimeEntries(this.periodStart, this.periodEnd, this.selectedEmployeeId ?? undefined).subscribe({
      next: (res: any[]) => this.entries = res,
      error: (err: any) => this.error = err?.error || 'Failed to load entries'
    });
  }
}
