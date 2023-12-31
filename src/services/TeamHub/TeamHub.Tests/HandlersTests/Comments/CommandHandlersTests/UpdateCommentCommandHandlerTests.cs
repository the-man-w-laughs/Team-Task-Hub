using AutoMapper;
using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Shared.Exceptions;
using Shared.Helpers;
using TeamHub.BLL.Contracts;
using TeamHub.BLL.Dtos;
using TeamHub.BLL.MediatR.CQRS.Comments.Commands;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;
using TeamHub.Tests.Fakers;
using TeamHub.Tests.Helpers;

namespace TeamHub.Tests.HandlersTests.Comments.CommandHandlersTests
{
    public class UpdateCommentCommandHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUserQueryService> _userQueryServiceMock;
        private readonly Mock<ICommentQueryService> _commentQueryServiceMock;
        private readonly Mock<ILogger<UpdateCommentCommandHandler>> _loggerMock;

        private readonly MapperHelper _mapperHelper;
        private readonly CommentRepositoryHelper _commentRepositoryHelper;
        private readonly CommentQueryServiceHelper _commentQueryServiceHelper;
        private readonly UserQueryServiceHelper _userQueryServiceHelper;
        private readonly HttpContextAccessorHelper _httpContextAccessorHelper;

        private readonly Faker<User> _userFaker;
        private readonly CommentFaker _commentFaker;
        private readonly CommentRequestDtoFaker _commentRequestDtoFaker;
        private readonly UpdateCommentCommandHandler _handler;

        public UpdateCommentCommandHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _userQueryServiceMock = new Mock<IUserQueryService>();
            _commentQueryServiceMock = new Mock<ICommentQueryService>();
            _loggerMock = new Mock<ILogger<UpdateCommentCommandHandler>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _httpContextAccessorHelper = new HttpContextAccessorHelper(_httpContextAccessorMock);
            _mapperHelper = new MapperHelper(_mapperMock);
            _userQueryServiceHelper = new UserQueryServiceHelper(_userQueryServiceMock);
            _commentRepositoryHelper = new CommentRepositoryHelper(_commentRepositoryMock);
            _commentQueryServiceHelper = new CommentQueryServiceHelper(_commentQueryServiceMock);

            _handler = new UpdateCommentCommandHandler(
                _httpContextAccessorMock.Object,
                _mapperMock.Object,
                _commentRepositoryMock.Object,
                _userQueryServiceMock.Object,
                _commentQueryServiceMock.Object,
                _loggerMock.Object
            );

            _userFaker = new UserFaker();
            _commentFaker = new CommentFaker();
            _commentRequestDtoFaker = new CommentRequestDtoFaker();

            _httpContextAccessorHelper.SetupHttpContextProperty(It.IsAny<int>());
        }

        [Fact]
        public async Task Handle_ValidRequest_CommentDeletedSuccessFully()
        {
            // Arrange
            var user = _userFaker.Generate();
            _httpContextAccessorHelper.SetupHttpContextProperty(user.Id);
            _userQueryServiceHelper.SetupGetExistingUserAsync(
                user.Id,
                CancellationToken.None,
                user
            );

            var comment = _commentFaker.Generate();
            comment.AuthorId = user.Id;

            var commentRequestDto = _commentRequestDtoFaker.Generate();
            var commentRequest = new Comment() { Content = commentRequestDto.Content };

            _mapperHelper.SetupMapCommentRequestDtoToComment();
            var request = new UpdateCommentCommand(comment.Id, commentRequestDto);

            _commentQueryServiceHelper.SetupGetExistingCommentAsync(comment.Id, comment);

            _commentRepositoryHelper.SetupUpdate();
            _commentRepositoryHelper.SetupSaveAsync();

            _mapperHelper.SetupMapCommentToCommentResponseDto();
            var commentResponseDto = _mapperMock.Object.Map<CommentResponseDto>(comment);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(commentResponseDto);
            _commentRepositoryMock.Verify(x => x.Update(It.IsAny<Comment>()), Times.Once);
            _commentRepositoryMock.Verify(x => x.SaveAsync(CancellationToken.None), Times.Once);
        }

        [Fact]
        public async Task Handle_UserDoesNotExist_ShouldThrowException()
        {
            // Arrange
            var user = _userFaker.Generate();
            _httpContextAccessorHelper.SetupHttpContextProperty(user.Id);
            _userQueryServiceHelper.SetupGetExistingUserAsync(
                user.Id,
                CancellationToken.None,
                new NotFoundException()
            );

            var comment = _commentFaker.Generate();
            var commentRequestDto = _commentRequestDtoFaker.Generate();

            var request = new UpdateCommentCommand(comment.Id, commentRequestDto);

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task Handle_CommentDoesNotExists_ShouldThrowException()
        {
            // Arrange
            var user = _userFaker.Generate();
            _httpContextAccessorHelper.SetupHttpContextProperty(user.Id);
            _userQueryServiceHelper.SetupGetExistingUserAsync(
                user.Id,
                CancellationToken.None,
                user
            );

            var comment = _commentFaker.Generate();
            var commentRequestDto = _commentRequestDtoFaker.Generate();

            var request = new UpdateCommentCommand(comment.Id, commentRequestDto);

            _commentQueryServiceHelper.SetupGetExistingCommentAsync(
                comment.Id,
                new NotFoundException()
            );

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task Handle_UserIsNotACommentAuthor_ShouldThrowException()
        {
            // Arrange
            var user = _userFaker.Generate();
            _httpContextAccessorHelper.SetupHttpContextProperty(user.Id);
            _userQueryServiceHelper.SetupGetExistingUserAsync(
                user.Id,
                CancellationToken.None,
                user
            );

            var comment = _commentFaker.Generate();
            comment.AuthorId = user.Id == int.MaxValue ? user.Id + 1 : int.MinValue;
            var commentRequestDto = _commentRequestDtoFaker.Generate();

            var request = new UpdateCommentCommand(comment.Id, commentRequestDto);

            _commentQueryServiceHelper.SetupGetExistingCommentAsync(
                comment.Id,
                new ForbiddenException()
            );

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<ForbiddenException>(act);
        }
    }
}
