import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  ReactiveFormsModule,
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { HttpParams } from '@angular/common/http';

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
      const apiUrl = 'http://localhost:5283/connect/token';

      // Create a new instance of HttpParams
      const body = new HttpParams()
        .set('client_id', 'client')
        .set('grant_type', 'password')
        .set('username', this.loginForm.get('username')?.value)
        .set('password', this.loginForm.get('password')?.value);

      // Make the POST request with the appropriate content type and body
      this.http
        .post(apiUrl, body.toString(), {
          headers: {
            'Content-Type': 'application/x-www-form-urlencoded',
          },
        })
        .subscribe({
          next: (response) => {
            console.log(response);
          },
          error: (error) => {
            console.error(error);
          },
          complete: () => {
            console.log('HTTP request completed.');
          },
        });
    } else {
      // Handle the case when the form is not valid, e.g., show validation messages.
    }
  }
  togglePasswordVisibility(event: Event) {
    event.preventDefault();
    this.showPassword = !this.showPassword;
  }
}
