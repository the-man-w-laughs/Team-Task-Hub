import { Component } from '@angular/core';
import { CommentDto } from '../../../shared/models/CommentResponseDto';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommentsService } from '../../../services/comment-service/comment.service';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { CommentsHubService } from '../../../services/comment-hub-service/comment-hub.service';
import { createCommentDto } from '../../../mappers/createCommentDto';

@Component({
  selector: 'app-root',
  templateUrl: './comment-section.component.html',
  styleUrls: ['./comment-section.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
})
export class CommentSectionComponent {
  title = 'angular-client';
  content: string = '';
  comments: CommentDto[] = [];
  editingCommentIndex: number | null = null;
  taskId: string = '';
  commentForm: FormGroup;

  constructor(
    private commentService: CommentsService,
    private route: ActivatedRoute,
    private fb: FormBuilder,
    private commentHubService: CommentsHubService
  ) {
    this.commentForm = this.fb.group({
      content: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.taskId = params['taskId'];
      this.loadComments();

      this.commentHubService.initializeConnection(this.taskId);

      this.commentHubService.onCommentUpdated((updatedComment: any) => {
        const index = this.comments.findIndex(
          (comment) => comment.id === updatedComment.id
        );
        if (index !== -1) {
          this.comments[index] = createCommentDto(updatedComment);
        }
      });

      this.commentHubService.onCommentCreated((newComment: any) => {
        console.log(newComment);

        this.comments.push(createCommentDto(newComment));
      });

      this.commentHubService.onCommentDeleted((deletedComment: any) => {
        const index = this.comments.findIndex(
          (comment) => comment.id === deletedComment.id
        );
        if (index !== -1) {
          this.comments.splice(index, 1);
        }
      });
    });
  }

  loadComments() {
    this.commentService
      .getComments(this.taskId, 0, 100)
      .subscribe((comments) => {
        console.log(comments);
        this.comments = comments;
      });
  }

  updateComment() {
    if (this.content && this.editingCommentIndex !== null) {
      this.commentHubService.updateComment(
        this.comments[this.editingCommentIndex].id,
        { content: this.content }
      );
      this.cancelEditing();
    }
  }

  addComment() {
    console.log('send', this.content);
    if (this.content) {
      if (this.editingCommentIndex !== null) {
        this.updateComment();
      } else {
        this.commentHubService
          .sendComment({ Content: this.content })
          .subscribe((newComment) => {
            this.resetForm();
          });
      }
    }
  }

  deleteComment(index: number) {
    if (index >= 0 && index < this.comments.length) {
      this.commentHubService
        .deleteComment(this.comments[index].id)
        .subscribe(() => {});
    }
  }

  startEditing(index: number) {
    this.editingCommentIndex = index;
    this.content = this.comments[index].content;
  }

  cancelEditing() {
    this.editingCommentIndex = null;
    this.resetForm();
  }

  resetForm() {
    this.content = '';
  }
}
