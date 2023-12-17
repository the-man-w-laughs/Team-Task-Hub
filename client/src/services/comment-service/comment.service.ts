import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { AuthService } from '../auth-service/auth.service';

@Injectable({
  providedIn: 'root',
})
export class CommentsService {
  private apiUrl: string = `${environment.apiBaseUrl}/api/tasks`;

  constructor(
    private httpClient: HttpClient,
    private authService: AuthService
  ) {}

  getComments(taskId: string, offset: number, limit: number): Observable<any> {
    const url = `${this.apiUrl}/${taskId}/comments`;

    const token = this.authService.getAccessToken();
    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });
    const params = {
      offset: offset.toString(),
      limit: limit.toString(),
    };

    return this.httpClient.get(url, { headers, params });
  }
}
