import { Component } from '@angular/core';
import { Comment } from '../../../shared/models/comment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CommentsService } from '../../../services/comment-service/comment.service';
import { ActivatedRoute } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  templateUrl: './comment-section.component.html',
  styleUrls: ['./comment-section.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
})
export class CommentSectionComponent {
  title = 'angular-client';
  author: string = '';
  content: string = '';
  comments: Comment[] = [];
  editingCommentIndex: number | null = null;
  taskId: string = '';

  constructor(
    private commentService: CommentsService,
    private route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.taskId = params['taskId'];
      this.loadComments();
    });
  }

  loadComments() {
    this.commentService
      .getComments(this.taskId, 0, 100)
      .subscribe((comments) => {
        console.log(comments);
      });
  }

  updateComment() {
    if (this.content && this.editingCommentIndex !== null) {
      this.comments[this.editingCommentIndex].content = this.content;
      this.editingCommentIndex = null;
      this.resetForm();
    }
  }

  addComment() {
    if (this.author && this.content) {
      if (this.editingCommentIndex !== null) {
        this.updateComment();
      } else {
        this.comments.push(new Comment(this.author, this.content, new Date()));
      }
      this.resetForm();
    }
  }

  deleteComment(index: number) {
    if (index >= 0 && index < this.comments.length) {
      this.comments.splice(index, 1);
    }
  }

  startEditing(index: number) {
    this.editingCommentIndex = index;
    this.author = this.comments[index].author;
    this.content = this.comments[index].content;
  }

  cancelEditing() {
    this.editingCommentIndex = null;
    this.resetForm();
  }

  resetForm() {
    this.author = '';
    this.content = '';
  }
}
