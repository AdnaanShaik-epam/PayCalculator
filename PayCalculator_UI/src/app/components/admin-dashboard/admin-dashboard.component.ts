import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {
  employees: any[] = [];
  error = '';

  // UI state: undefined = not editing, number = editing value
  editHourly: { [key: number]: number | undefined } = {};
  salaryResult: any = null;
  periodStart = '';
  periodEnd = '';

  constructor(private api: ApiService, private auth: AuthService) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.api.getEmployees().subscribe({
      next: (res: any[]) => this.employees = res,
      error: (err: any) => this.error = err?.error || 'Failed to load employees'
    });
  }

  startEdit(e: any) {
    // set explicit value even if 0
    this.editHourly[e.employeeId] = e.hourlyPay ?? 0;
  }

  cancelEdit(e: any) {
    delete this.editHourly[e.employeeId];
  }

  saveHourly(e: any) {
    const value = Number(this.editHourly[e.employeeId]);
    this.api.setHourlyPay(e.employeeId, value).subscribe({
      next: () => {
        // refresh list and clear edit state
        this.load();
        delete this.editHourly[e.employeeId];
      },
      error: (err: any) => this.error = err?.error || 'Failed to set hourly pay'
    });
  }

  calculate(e: any) {
    if (!this.periodStart || !this.periodEnd) {
      this.error = 'Please select period start and end';
      return;
    }

    this.api.calculateSalary(e.employeeId, this.periodStart, this.periodEnd).subscribe({
      next: (res: any) => this.salaryResult = res,
      error: (err: any) => this.error = err?.error || 'Failed to calculate salary'
    });
  }

  logout() {
    this.auth.logout();
    window.location.href = '/';
  }
}
