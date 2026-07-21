import { Routes } from '@angular/router';
import { HomeComponent } from './components/home.component';
import { CourseListComponent } from './components/course-list.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'courses', component: CourseListComponent },
  { path: '**', redirectTo: '' }
];
