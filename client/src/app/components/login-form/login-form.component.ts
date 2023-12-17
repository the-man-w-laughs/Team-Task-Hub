import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth-service/auth.service';

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

  loginError: string | undefined;

  constructor(
    private formBuilder: FormBuilder,
    private http: HttpClient,
    private router: Router,
    private authService: AuthService
  ) {
    this.loginForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      const apiUrl = 'http://localhost:5283/connect/token';

      const body = new HttpParams()
        .set('client_id', 'client')
        .set('grant_type', 'password')
        .set('username', this.loginForm.get('username')?.value)
        .set('password', this.loginForm.get('password')?.value);

      this.http
        .post(apiUrl, body.toString(), {
          headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
          },
        })
        .subscribe({
          next: (response) => {
            console.log(response);

            const token = (response as any)?.access_token;

            this.authService.setAccessToken(token);

            this.router.navigate(['/task-connection']);
          },
          error: (error) => {
            console.error(error);
            this.loginError = 'Login failed. Please check your credentials.';
          },
          complete: () => {
            console.log('HTTP request completed.');
          },
        });
    } else {
    }
  }

  togglePasswordVisibility(event: Event) {
    event.preventDefault();
    this.showPassword = !this.showPassword;
  }
}
