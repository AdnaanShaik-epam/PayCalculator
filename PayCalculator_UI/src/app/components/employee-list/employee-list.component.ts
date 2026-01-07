import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-employee-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css']
})
export class EmployeeListComponent implements OnInit {
  employees: any[] = [];
  // form
  selectedEmployeeId: number | null = null;
  loginTime = '';
  logoutTime = '';
  message = '';

  constructor(private api: ApiService) { }

  ngOnInit() { this.api.getEmployees().subscribe((res: any[]) => this.employees = res); }

  submitTime() {
    if (!this.selectedEmployeeId || !this.loginTime || !this.logoutTime) {
      this.message = 'Please select employee and both times';
      return;
    }

    this.api.addTimeEntry(this.selectedEmployeeId, this.loginTime, this.logoutTime).subscribe({
      next: () => this.message = 'Time entry added',
      error: (err: any) => this.message = err?.error || 'Failed to add time entry'
    });
  }
}
