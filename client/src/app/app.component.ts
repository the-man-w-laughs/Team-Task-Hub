import { Component } from '@angular/core';
import { LoginFormComponent } from './components/login-form/login-form.component';
import { HttpClientModule } from '@angular/common/http';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  imports: [LoginFormComponent, HttpClientModule, RouterOutlet],
  styleUrls: ['./app.component.css'],
  standalone: true,
})
export class AppComponent {
  title = 'angular-client';
}
