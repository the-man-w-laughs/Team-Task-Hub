// task-connection-form.component.ts

import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../../../services/auth-service/auth.service';
import { CommentsHubService } from '../../../services/comment-hub-service/comment-hub.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-task-connection-form',
  templateUrl: './task-connection-form.component.html',
  styleUrls: ['./task-connection-form.component.css'],
  imports: [CommonModule, ReactiveFormsModule],
  standalone: true,
})
export class TaskConnectionFormComponent {
  connectForm: FormGroup;
  connectionStatus: 'connecting' | 'success' | 'error' = '' as 'connecting';

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private commentsHubService: CommentsHubService,
    private router: Router
  ) {
    this.connectForm = this.fb.group({
      taskId: ['', Validators.required],
    });
  }

  async connect() {
    const taskId = this.connectForm.get('taskId')?.value;
    const accessToken = this.authService.getAccessToken();

    if (!accessToken) {
      console.error('Access token not available. User is not authenticated.');
      return;
    }

    try {
      this.connectionStatus = 'connecting';
      await this.commentsHubService.initializeConnection(taskId);

      this.commentsHubService.onConnected(() => {
        this.connectionStatus = 'success';
        this.router.navigate([`task/${taskId}/comments`]);
      });

      this.commentsHubService.onDisconnected(() => {
        this.connectionStatus = 'error';
      });
    } catch (error) {
      this.connectionStatus = 'error';
      console.error('Error connecting to hub', error);
    }
  }
}
