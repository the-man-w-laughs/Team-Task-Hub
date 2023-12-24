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
    public class CreateCommentCommandHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUserQueryService> _userQueryServiceMock;
        private readonly Mock<ITaskQueryService> _taskQueryServiceMock;
        private readonly Mock<ITeamMemberQueryService> _teamMemberQueryServiceMock;
        private readonly Mock<ILogger<CreateCommentCommandHandler>> _loggerMock;

        private readonly TeamMemberQueryServiceHelper _teamMemberQueryServiceHelper;
        private readonly MapperHelper _mapperHelper;
        private readonly CommentRepositoryHelper _commentRepositoryHelper;
        private readonly UserQueryServiceHelper _userQueryServiceHelper;
        private readonly TaskQueryServiceHelper _taskQueryServiceHelper;
        private readonly HttpContextAccessorHelper _httpContextAccessorHelper;

        private readonly Faker _faker;
        private readonly Faker<CommentRequestDto> _commentRequestDtoFaker;
        private readonly Faker<User> _userFaker;
        private readonly TaskModelFaker _taskFaker;
        private readonly CreateCommentCommandHandler _handler;

        public CreateCommentCommandHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _userQueryServiceMock = new Mock<IUserQueryService>();
            _taskQueryServiceMock = new Mock<ITaskQueryService>();
            _teamMemberQueryServiceMock = new Mock<ITeamMemberQueryService>();
            _loggerMock = new Mock<ILogger<CreateCommentCommandHandler>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _httpContextAccessorHelper = new HttpContextAccessorHelper(_httpContextAccessorMock);
            _mapperHelper = new MapperHelper(_mapperMock);
            _teamMemberQueryServiceHelper = new TeamMemberQueryServiceHelper(
                _teamMemberQueryServiceMock
            );
            _userQueryServiceHelper = new UserQueryServiceHelper(_userQueryServiceMock);
            _commentRepositoryHelper = new CommentRepositoryHelper(_commentRepositoryMock);
            _taskQueryServiceHelper = new TaskQueryServiceHelper(_taskQueryServiceMock);

            _handler = new CreateCommentCommandHandler(
                _httpContextAccessorMock.Object,
                _mapperMock.Object,
                _commentRepositoryMock.Object,
                _userQueryServiceMock.Object,
                _taskQueryServiceMock.Object,
                _teamMemberQueryServiceMock.Object,
                _loggerMock.Object
            );

            _faker = new Faker();

            _commentRequestDtoFaker = new CommentRequestDtoFaker();

            _userFaker = new UserFaker();
            _taskFaker = new TaskModelFaker();

            _httpContextAccessorHelper.SetupHttpContextProperty(It.IsAny<int>());
        }

        [Fact]
        public async Task Handle_ValidRequest_CommentCreatedSuccessfully()
        {
            // Arrange
            var user = _userFaker.Generate();
            _httpContextAccessorHelper.SetupHttpContextProperty(user.Id);
            _userQueryServiceHelper.SetupGetExistingUserAsync(
                user.Id,
                CancellationToken.None,
                user
            );

            var commentRequestDto = _commentRequestDtoFaker.Generate();
            _mapperHelper.SetupMapCommentRequestDtoToComment();
            var comment = _mapperMock.Object.Map<Comment>(commentRequestDto);

            var task = _taskFaker.Generate();
            var request = new CreateCommentCommand(task.Id, commentRequestDto);

            _taskQueryServiceHelper.SetupGetExistingTaskAsync(task.Id, task);

            var teamMember = new TeamMember() { UserId = user.Id, ProjectId = task.ProjectId };
            _teamMemberQueryServiceHelper.SetupGetExistingTeamMemberAsync(
                user.Id,
                task.ProjectId,
                teamMember
            );

            _commentRepositoryHelper.SetupAddAsync(comment);
            _commentRepositoryHelper.SetupSaveAsync();

            _mapperHelper.SetupMapCommentToCommentResponseDto();
            var commentResponseDto = _mapperMock.Object.Map<CommentResponseDto>(comment);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(commentResponseDto);
            _commentRepositoryMock.Verify(
                x => x.AddAsync(It.IsAny<Comment>(), CancellationToken.None),
                Times.Once
            );
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

            var commentRequestDto = _commentRequestDtoFaker.Generate();
            var task = _taskFaker.Generate();

            var request = new CreateCommentCommand(task.Id, commentRequestDto);

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task Handle_TaskDoesNotExists_ShouldThrowException()
        {
            // Arrange
            var task = _taskFaker.Generate();
            var commentRequestDto = _commentRequestDtoFaker.Generate();
            var request = new CreateCommentCommand(task.Id, commentRequestDto);

            _taskQueryServiceHelper.SetupGetExistingTaskAsync(task.Id, new NotFoundException());

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }

        [Fact]
        public async Task Handle_UserIsNotInProject_ShouldThrowException()
        {
            // Arrange
            var task = _taskFaker.Generate();
            var commentRequestDto = _commentRequestDtoFaker.Generate();
            var request = new CreateCommentCommand(task.Id, commentRequestDto);

            _taskQueryServiceHelper.SetupGetExistingTaskAsync(task.Id, task);

            _teamMemberQueryServiceHelper.SetupGetExistingTeamMemberAsync(
                It.IsAny<int>(),
                task.ProjectId,
                new NotFoundException()
            );

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }
    }
}
