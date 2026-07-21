import { Component, Input, Output, EventEmitter, SimpleChanges, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Course } from '../models/course.model';
import { HighlightDirective } from '../directives/highlight.directive';
import { CreditLabelPipe } from '../pipes/credit-label.pipe';
import { EnrollmentService } from '../services/enrollment.service';

@Component({
  selector: 'app-course-card',
  standalone: true,
  imports: [CommonModule, HighlightDirective, CreditLabelPipe],
  template: `
    <div [appHighlight]="'lightblue'" [ngClass]="cardClasses" [ngStyle]="{'border-left': getBorderColor()}" style="padding: 15px; margin: 10px; border: 1px solid #ccc; border-radius: 8px;">
      <h3>{{ course.name }}</h3>
      <p>Code: {{ course.code }} | {{ course.credits | creditLabel }}</p>
      <p>Grade Status: 
        <span [ngSwitch]="course.gradeStatus">
          <span *ngSwitchCase="'passed'" style="color: green;">Passed</span>
          <span *ngSwitchCase="'failed'" style="color: red;">Failed</span>
          <span *ngSwitchDefault style="color: grey;">Pending</span>
        </span>
      </p>
      <button (click)="toggleEnroll()">{{ isEnrolled() ? 'Unenroll' : 'Enroll' }}</button>
      <button (click)="isExpanded = !isExpanded">Show Details</button>
      <div *ngIf="isExpanded">
        <p>Detailed course description and syllabus goes here...</p>
      </div>
    </div>
  `
})
export class CourseCardComponent implements OnChanges {
  @Input() course!: Course;
  @Output() enrollRequested = new EventEmitter<number>();
  isExpanded: boolean = false;

  constructor(private enrollmentService: EnrollmentService) {}

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['course']) {
      console.log('Course input changed:', changes['course'].previousValue, changes['course'].currentValue);
    }
  }

  get cardClasses() {
    return {
      'card--enrolled': this.isEnrolled(),
      'card--full': this.course?.credits >= 4,
      'expanded': this.isExpanded
    };
  }

  getBorderColor(): string {
    if (this.course?.gradeStatus === 'passed') return '5px solid green';
    if (this.course?.gradeStatus === 'failed') return '5px solid red';
    return '5px solid grey';
  }

  isEnrolled(): boolean {
    return this.course ? this.enrollmentService.isEnrolled(this.course.id) : false;
  }

  toggleEnroll(): void {
    if (this.isEnrolled()) {
      this.enrollmentService.unenroll(this.course.id);
    } else {
      this.enrollmentService.enroll(this.course.id);
      this.enrollRequested.emit(this.course.id);
    }
  }
}
