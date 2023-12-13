import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-connect-task-form',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './connect-task-form.component.html',
  styleUrl: './connect-task-form.component.css',
})
export class ConnectTaskFormComponent {
  connectForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.connectForm = this.fb.group({
      taskId: ['', Validators.required],
    });
  }

  connect() {
    const taskId = this.connectForm.get('taskId')?.value;
    console.log(`Connecting with task ID: ${taskId}`);
  }
}
