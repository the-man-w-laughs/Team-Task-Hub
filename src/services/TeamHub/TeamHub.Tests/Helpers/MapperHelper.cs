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

        public void SetupMap(CommentRequestDto commentRequestDto, Comment comment)
        {
            _mapperMock.Setup(mapper => mapper.Map<Comment>(commentRequestDto)).Returns(comment);
        }

        public void SetupMap(Comment comment, CommentResponseDto commentResponseDto)
        {
            _mapperMock
                .Setup(mapper => mapper.Map<CommentResponseDto>(comment))
                .Returns(commentResponseDto);
        }
    }
}
