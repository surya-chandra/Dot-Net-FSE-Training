import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet, RouterLink } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, RouterLink],
  template: `
    <header style="background: #003366; color: white; padding: 15px;">
      <h2>Student Course Portal</h2>
      <nav>
        <a routerLink="/" style="color: white; margin-right: 15px;">Home</a>
        <a routerLink="/courses" style="color: white; margin-right: 15px;">Courses</a>
        <a routerLink="/profile" style="color: white;">Profile</a>
      </nav>
    </header>
    <main style="min-height: 400px;">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {
  title = 'Student Course Portal';
}
