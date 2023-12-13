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

  constructor() {
    for (let i = 0; i < 100; i++) {
      this.addComment(`User${i + 1}`, `This is comment ${i + 1}.`);
    }
  }

  addComment(author: string, content: string) {
    if (author && content) {
      this.comments.push(new Comment(author, content, new Date()));
    }
  }
}
