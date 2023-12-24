using AutoMapper;
using Moq;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.Tests.Helpers
{
    public class MapperHelper
    {
        private readonly Mock<IMapper> _mapperMock;

        public MapperHelper(Mock<IMapper> mapperMock)
        {
            _mapperMock = mapperMock;
        }

        public void SetupMapCommentRequestDtoToComment()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<Comment>(It.IsAny<CommentRequestDto>()))
                .Returns((CommentRequestDto source) => new Comment() { Content = source.Content, });
        }

        public void SetupMapCommentToCommentResponseDto()
        {
            _mapperMock
                .Setup(mapper => mapper.Map<CommentResponseDto>(It.IsAny<Comment>()))
                .Returns(
                    (Comment source) =>
                        new CommentResponseDto()
                        {
                            Id = source.Id,
                            AuthorId = source.AuthorId,
                            Content = source.Content,
                            CreatedAt = source.CreatedAt,
                            TasksId = source.TasksId
                        }
                );
        }
    }
}
