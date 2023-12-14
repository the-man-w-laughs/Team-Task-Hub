import { Component } from '@angular/core';
import { Comment } from '../../../shared/models/comment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-root',
  templateUrl: './comment-section.component.html',
  styleUrls: ['./comment-section.component.css'],
  standalone: true,
  imports: [CommonModule, FormsModule],
})
export class CommentSectionComponent {
  title = 'angular-client';
  author: string = '';
  content: string = '';
  comments: Comment[] = [];
  editingCommentIndex: number | null = null;

  constructor() {
    for (let i = 0; i < 10; i++) {
      const uniqueAuthor = `User${i + 1}`;
      const uniqueContent = `This is comment ${i + 1}.`;
      this.author = uniqueAuthor;
      this.content = uniqueContent;
      this.addComment();
    }
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
