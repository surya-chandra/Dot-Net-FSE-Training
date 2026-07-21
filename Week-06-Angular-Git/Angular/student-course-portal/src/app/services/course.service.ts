import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, of, throwError } from 'rxjs';
import { catchError, map, retry, tap } from 'rxjs/operators';
import { Course } from '../models/course.model';

@Injectable({
  providedIn: 'root'
})
export class CourseService {
  private apiUrl = 'http://localhost:3000/courses';
  private courses: Course[] = [
    { id: 1, name: 'Data Structures & Algorithms', code: 'CS101', credits: 4, gradeStatus: 'passed' },
    { id: 2, name: 'Web Development with Angular', code: 'CS102', credits: 3, gradeStatus: 'passed' },
    { id: 3, name: 'Database Management Systems', code: 'CS103', credits: 4, gradeStatus: 'pending' },
    { id: 4, name: 'Operating Systems', code: 'CS104', credits: 3, gradeStatus: 'failed' },
    { id: 5, name: 'Cloud Computing', code: 'CS105', credits: 2, gradeStatus: 'pending' }
  ];

  constructor(private http: HttpClient) {}

  getCourses(): Observable<Course[]> {
    return of(this.courses).pipe(
      tap(courses => console.log('Courses loaded:', courses.length)),
      retry(2),
      catchError(err => {
        console.error(err);
        return throwError(() => new Error('Failed to load courses. Please try again.'));
      })
    );
  }

  getCourseById(id: number): Observable<Course | undefined> {
    const course = this.courses.find(c => c.id === id);
    return of(course);
  }

  addCourse(course: Course): void {
    this.courses.push(course);
  }
}
