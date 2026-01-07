import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private tokenKey = 'pc_token';
  private adminKey = 'pc_is_admin';
  private employeeIdKey = 'pc_employee_id';

  constructor(private http: HttpClient) { }

  login(email: string, password: string) {
    return this.http.post<any>('https://localhost:7089/api/Auth/login', { email, password }).pipe(
      tap(res => {
        if (res?.token) {
          localStorage.setItem(this.tokenKey, res.token);
          localStorage.setItem(this.adminKey, res.isAdmin ? 'true' : 'false');
          if (res?.employeeId != null) {
            localStorage.setItem(this.employeeIdKey, res.employeeId.toString());
          }
        }
      })
    );
  }

  register(fullName: string, email: string, password: string) {
    return this.http.post<any>('https://localhost:7089/api/Auth/register', { fullName, email, password }).pipe(
      tap(res => {
        if (res?.token) {
          localStorage.setItem(this.tokenKey, res.token);
          localStorage.setItem(this.adminKey, res.isAdmin ? 'true' : 'false');
          if (res?.employeeId != null) {
            localStorage.setItem(this.employeeIdKey, res.employeeId.toString());
          }
        }
      })
    );
  }

  logout() {
    localStorage.removeItem(this.tokenKey);
    localStorage.removeItem(this.adminKey);
    localStorage.removeItem(this.employeeIdKey);
  }

  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  isAdmin() {
    return localStorage.getItem(this.adminKey) === 'true';
  }

  getEmployeeId(): number | null {
    const v = localStorage.getItem(this.employeeIdKey);
    return v ? Number(v) : null;
  }
}
