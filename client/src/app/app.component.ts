import { Component } from '@angular/core';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [LoginFormComponent, HttpClientModule],
  styleUrls: ['./app.component.css'],
  standalone: true,
})
export class AppComponent {
  title = 'angular-client';
}
