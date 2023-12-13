import { Routes } from '@angular/router';

import { LoginFormComponent } from './components/login-form/login-form.component';
import { CommentSectionComponent } from './components/comment-section/comment-section.component';
import { NotFoundComponent } from './components/not-found-component/not-found.component';
import { ConnectTaskFormComponent } from './components/connect-task-form/connect-task-form.component';

export const routes: Routes = [
  { path: 'login', component: LoginFormComponent },
  { path: 'task/:id/comments', component: CommentSectionComponent },
  { path: 'connect-task', component: ConnectTaskFormComponent },
  { path: '**', component: NotFoundComponent },
];
