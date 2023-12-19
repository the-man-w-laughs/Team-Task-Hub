import { Routes } from '@angular/router';

import { LoginFormComponent } from './components/login-form/login-form.component';
import { CommentSectionComponent } from './components/comment-section/comment-section.component';
import { NotFoundComponent } from './components/not-found-component/not-found.component';
import { TaskConnectionFormComponent } from './components/task-connection-form/task-connection-form.component';

export const routes: Routes = [
  { path: 'login', component: LoginFormComponent },
  { path: 'task/:taskId/comments', component: CommentSectionComponent },
  { path: 'task-connection', component: TaskConnectionFormComponent },
  { path: '**', component: NotFoundComponent },
];
