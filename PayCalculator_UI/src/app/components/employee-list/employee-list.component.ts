import { Component, OnInit } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';

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

  isAdmin = false;
  currentEmployeeId: number | null = null;

  constructor(private api: ApiService, private auth: AuthService) { }

  ngOnInit() {
    this.isAdmin = this.auth.isAdmin();
    this.currentEmployeeId = this.auth.getEmployeeId();

    if (this.isAdmin) {
      this.api.getEmployees().subscribe((res: any[]) => {
        this.employees = res;
      }, (err: any) => console.error('Failed to load employees', err));
    } else {
      // load only the current employee
      if (this.currentEmployeeId != null) {
        this.api.getEmployee(this.currentEmployeeId).subscribe({
          next: (res: any) => {
            this.employees = [res];
            this.selectedEmployeeId = res.employeeId;
          },
          error: (err: any) => console.error('Failed to load employee', err)
        });
      }
    }
  }

  // returns start of current day in YYYY-MM-DDT00:00 format for datetime-local min attribute
  get minDateTime(): string {
    const now = new Date();
    const pad = (n: number) => n < 10 ? '0' + n : n;
    const yyyy = now.getFullYear();
    const mm = pad(now.getMonth() + 1);
    const dd = pad(now.getDate());
    return `${yyyy}-${mm}-${dd}T00:00`;
  }

  // returns current datetime in YYYY-MM-DDTHH:mm format for datetime-local max attribute
  get maxDateTime(): string {
    const now = new Date();
    const pad = (n: number) => n < 10 ? '0' + n : n;
    const yyyy = now.getFullYear();
    const mm = pad(now.getMonth() + 1);
    const dd = pad(now.getDate());
    const hh = pad(now.getHours());
    const min = pad(now.getMinutes());
    return `${yyyy}-${mm}-${dd}T${hh}:${min}`;
  }

  submitTime() {
    this.message = '';
    if (!this.selectedEmployeeId || !this.loginTime || !this.logoutTime) {
      this.message = 'Please select employee and both times';
      return;
    }

    // Parse datetimes
    const login = new Date(this.loginTime);
    const logout = new Date(this.logoutTime);
    const now = new Date();

    // If user is not admin, enforce current-day and no future times (past earlier today allowed)
    if (!this.isAdmin) {
      const today = new Date();
      if (login.toDateString() !== today.toDateString() || logout.toDateString() !== today.toDateString()) {
        this.message = 'Employees can only add entries for the current day';
        return;
      }

      if (login > now || logout > now) {
        this.message = 'Cannot add entries with future times';
        return;
      }

      if (logout <= login) {
        this.message = 'Logout must be after login';
        return;
      }

      // ensure employee is adding for themselves
      if (this.currentEmployeeId != null && this.selectedEmployeeId !== this.currentEmployeeId) {
        this.message = 'You can only add entries for yourself';
        return;
      }
    }

    // send raw datetime-local strings to server (no timezone conversion)
    this.api.addTimeEntry(this.selectedEmployeeId, this.loginTime, this.logoutTime).subscribe({
      next: () => this.message = 'Time entry added',
      error: (err: any) => this.message = err?.error || 'Failed to add time entry'
    });
  }
}
