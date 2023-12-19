import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private accessTokenKey = 'access_token';

  getAccessToken(): string | null {
    return localStorage.getItem(this.accessTokenKey);
  }

  setAccessToken(token: string): void {
    localStorage.setItem(this.accessTokenKey, token);
  }

  resetAccessToken(): void {
    localStorage.removeItem(this.accessTokenKey);
  }
}
