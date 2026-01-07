import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';

  constructor(private auth: AuthService, private router: Router) { }

  submit() {
    this.error = '';
    this.auth.login(this.email, this.password).subscribe({
      next: (res: any) => {
        // use response value to route
        const isAdmin = res?.isAdmin ?? false;
        if (isAdmin) this.router.navigate(['/admin']);
        else this.router.navigate(['/employees']);
      },
      error: (err: any) => {
        console.error('Login error', err);
        if (err?.error) {
          try {
            this.error = typeof err.error === 'string' ? err.error : JSON.stringify(err.error);
          } catch {
            this.error = 'Login failed';
          }
        } else {
          this.error = err?.message || 'Login failed';
        }
      }
    });
  }
}
