export class CommentDto {
  constructor(
    public id: string,
    public author: string,
    public content: string,
    public date: Date
  ) {}
}
