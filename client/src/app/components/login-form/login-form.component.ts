import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  standalone: true,
  styleUrls: ['./login-form.component.css'],
  imports: [ReactiveFormsModule, CommonModule],
})
export class LoginFormComponent {
  loginForm: FormGroup;
  showPassword: boolean = false;

  constructor(private formBuilder: FormBuilder, private http: HttpClient) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const formData = this.loginForm.value;

      const apiUrl = 'https://example.com/login';

      this.http.post(apiUrl, formData).subscribe({
        next: (response) => {
          console.log('Server response:', response);
        },
        error: (error) => {
          console.error('Error:', error);
        },
        complete: () => {},
      });
    } else {
    }
  }

  togglePasswordVisibility(event: Event) {
    event.preventDefault();
    this.showPassword = !this.showPassword;
  }
}
