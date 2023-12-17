import { CommentDto } from '../shared/models/CommentResponseDto';

export const createCommentDto = (comment: any) =>
  new CommentDto(
    comment.id,
    comment.author.email,
    comment.content,
    new Date(comment.createdAt)
  );
