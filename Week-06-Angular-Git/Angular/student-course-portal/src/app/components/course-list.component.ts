import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CourseService } from '../services/course.service';
import { Course } from '../models/course.model';
import { CourseCardComponent } from './course-card.component';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, CourseCardComponent],
  template: `
    <div style="padding: 20px;">
      <h2>Available Courses</h2>
      <div *ngIf="isLoading; else courseContent">
        <p>Loading courses...</p>
      </div>
      <ng-template #courseContent>
        <div *ngIf="courses.length > 0; else noCourses">
          <app-course-card 
            *ngFor="let c of courses; let i = index; trackBy: trackByCourseId" 
            [course]="c" 
            (enrollRequested)="onEnroll($event)">
          </app-course-card>
        </div>
        <ng-template #noCourses>
          <p>No courses available.</p>
        </ng-template>
      </ng-template>
      <p *ngIf="selectedCourseId">Selected course ID: {{ selectedCourseId }}</p>
    </div>
  `
})
export class CourseListComponent implements OnInit {
  isLoading: boolean = true;
  courses: Course[] = [];
  selectedCourseId: number | null = null;

  constructor(private courseService: CourseService) {}

  ngOnInit(): void {
    setTimeout(() => {
      this.courseService.getCourses().subscribe(data => {
        this.courses = data;
        this.isLoading = false;
      });
    }, 1500);
  }

  trackByCourseId(index: number, course: Course): number {
    return course.id;
  }

  onEnroll(courseId: number): void {
    console.log('Enrolling in course: ' + courseId);
    this.selectedCourseId = courseId;
  }
}
