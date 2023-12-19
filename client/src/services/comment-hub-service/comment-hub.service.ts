// Import necessary modules
import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { AuthService } from '../auth-service/auth.service';
import { CommentRequestDto } from '../../shared/models/CommentRequestDto';
import { Observable, from } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CommentsHubService {
  private hubConnection!: HubConnection;

  constructor(private authService: AuthService) {}

  public async initializeConnection(task_id: string) {
    var accessToken = this.authService.getAccessToken();

    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}/commentsHub?task_id=${task_id}`, {
        accessTokenFactory: () => accessToken as string,
      })
      .build();
    await this.hubConnection.start();
  }

  public onConnected(callback: (comment: any) => void) {
    this.hubConnection.on('ConnectionAsync', callback);
  }

  public onDisconnected(callback: () => void) {
    this.hubConnection.onclose(() => {
      callback();
    });
  }

  public onCommentUpdated(callback: (comment: any) => void) {
    this.hubConnection.on('UpdateCommentAsync', callback);
  }

  public onCommentCreated(callback: (comment: any) => void) {
    this.hubConnection.on('CreateCommentAsync', callback);
  }

  public onCommentDeleted(callback: (comment: any) => void) {
    this.hubConnection.on('DeleteCommentAsync', callback);
  }
  public sendComment(comment: CommentRequestDto): Observable<void> {
    return from(this.hubConnection.invoke('SendComment', comment));
  }

  public updateComment(
    commentId: string,
    updatedComment: any
  ): Observable<void> {
    return from(
      this.hubConnection.invoke('UpdateComment', commentId, updatedComment)
    );
  }

  public deleteComment(commentId: string): Observable<void> {
    return from(this.hubConnection.invoke('DeleteComment', commentId));
  }
}
