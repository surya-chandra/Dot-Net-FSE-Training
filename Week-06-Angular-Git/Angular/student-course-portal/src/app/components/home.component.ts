import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CourseService } from '../services/course.service';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div style="padding: 20px;">
      <h1>{{ portalName }}</h1>
      <p>Welcome to the Student Course Portal.</p>
      <div style="margin-bottom: 15px;">
        <button [disabled]="!isPortalActive" (click)="onEnrollClick()">Enroll Now</button>
        <span *ngIf="message" style="margin-left: 10px; color: green;">{{ message }}</span>
      </div>
      <div>
        <input [(ngModel)]="searchTerm" placeholder="Search courses..." />
        <p *ngIf="searchTerm">Searching for: {{ searchTerm }}</p>
      </div>
      <div style="margin-top: 20px; font-weight: bold;">
        Stats: Available Courses: {{ availableCount }} | Enrolled: 2 | GPA: 3.8
      </div>
    </div>
  `
})
export class HomeComponent implements OnInit, OnDestroy {
  portalName: string = 'Student Course Portal';
  isPortalActive: boolean = true;
  message: string = '';
  searchTerm: string = '';
  availableCount: number = 0;

  constructor(private courseService: CourseService) {}

  ngOnInit(): void {
    this.courseService.getCourses().subscribe(courses => {
      this.availableCount = courses.length;
    });
    console.log('HomeComponent initialised — courses loaded');
  }

  ngOnDestroy(): void {
    console.log('HomeComponent destroyed');
  }

  onEnrollClick(): void {
    this.message = 'Enrollment opened!';
  }
}
