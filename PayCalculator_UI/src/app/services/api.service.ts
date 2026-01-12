import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class ApiService {
  constructor(private http: HttpClient) { }

  getEmployees() {
    return this.http.get<any[]>('https://localhost:7089/api/Admin/employees');
  }

  getEmployee(employeeId: number) {
    return this.http.get<any>(`https://localhost:7089/api/Employee/${employeeId}`);
  }

  setHourlyPay(employeeId: number, hourlyPay: number) {
    return this.http.post('https://localhost:7089/api/Admin/set-hourly-pay', { employeeId, hourlyPay });
  }

  calculateSalary(employeeId: number, periodStart: string, periodEnd: string) {
    return this.http.get<any>('https://localhost:7089/api/Admin/calculate-salary', { params: { employeeId: employeeId.toString(), periodStart, periodEnd } });
  }

  // Employee endpoints
  addTimeEntry(employeeId: number, loginTime: string, logoutTime: string) {
    return this.http.post('https://localhost:7089/api/Employee/time-entry', { employeeId, loginTime, logoutTime });
  }

  getWorkingHours(employeeId: number, periodStart: string, periodEnd: string) {
    return this.http.get<number>(`https://localhost:7089/api/Employee/${employeeId}/working-hours`, { params: { periodStart, periodEnd } });
  }

  // Admin: get all time entries in a date range
  getAllTimeEntries(periodStart: string, periodEnd: string, employeeId?: number) {
    let params = new HttpParams().set('periodStart', periodStart).set('periodEnd', periodEnd);
    if (employeeId != null) params = params.set('employeeId', employeeId.toString());
    return this.http.get<any[]>('https://localhost:7089/api/Admin/time-entries', { params });
  }
}
