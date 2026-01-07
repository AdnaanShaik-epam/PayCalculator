import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterModule],
  template: `
    <div class="container">
      <nav>
        <a routerLink="/login">Login</a> |
        <a routerLink="/register">Register</a> |
        <a routerLink="/employees">Employees</a> |
        <a routerLink="/admin">Admin</a> |
        <a routerLink="/time-entries-all">All Time Entries</a>
      </nav>

      <div class="card">
        <router-outlet></router-outlet>
      </div>
    </div>
  `
})
export class App {
  title = 'PayCalculator UI';
}
