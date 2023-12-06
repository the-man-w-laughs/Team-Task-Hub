using System.Linq.Expressions;
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
using TeamHub.BLL.MediatR.CQRS.Comments.Queries;
using TeamHub.BLL.MediatR.CQRS.Projects.Queries;
using TeamHub.DAL.Contracts.Repositories;
using TeamHub.DAL.Models;
using TeamHub.Tests.Fakers;
using TeamHub.Tests.Helpers;

namespace TeamHub.Tests.HandlersTests.Comments.QueryHandlersTests
{
    public class GetAllTasksCommentsQueryHandlerTests
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUserQueryService> _userQueryServiceMock;
        private readonly Mock<ITaskQueryService> _taskQueryServiceMock;
        private readonly Mock<ITeamMemberQueryService> _teamMemberQueryServiceMock;
        private readonly Mock<ILogger<GetAllTasksCommentsQueryHandler>> _loggerMock;

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
        private readonly CommentFaker _commentFaker;
        private readonly GetAllTasksCommentsQueryHandler _handler;

        public GetAllTasksCommentsQueryHandlerTests()
        {
            _mapperMock = new Mock<IMapper>();
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _userQueryServiceMock = new Mock<IUserQueryService>();
            _taskQueryServiceMock = new Mock<ITaskQueryService>();
            _teamMemberQueryServiceMock = new Mock<ITeamMemberQueryService>();
            _loggerMock = new Mock<ILogger<GetAllTasksCommentsQueryHandler>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            _httpContextAccessorHelper = new HttpContextAccessorHelper(_httpContextAccessorMock);
            _mapperHelper = new MapperHelper(_mapperMock);
            _teamMemberQueryServiceHelper = new TeamMemberQueryServiceHelper(
                _teamMemberQueryServiceMock
            );
            _userQueryServiceHelper = new UserQueryServiceHelper(_userQueryServiceMock);
            _commentRepositoryHelper = new CommentRepositoryHelper(_commentRepositoryMock);
            _taskQueryServiceHelper = new TaskQueryServiceHelper(_taskQueryServiceMock);

            _handler = new GetAllTasksCommentsQueryHandler(
                _httpContextAccessorMock.Object,
                _commentRepositoryMock.Object,
                _mapperMock.Object,
                _userQueryServiceMock.Object,
                _taskQueryServiceMock.Object,
                _teamMemberQueryServiceMock.Object,
                _loggerMock.Object
            );

            _faker = new Faker();

            _commentRequestDtoFaker = new CommentRequestDtoFaker();

            _userFaker = new UserFaker();
            _taskFaker = new TaskModelFaker();
            _commentFaker = new CommentFaker();

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

            var task = _taskFaker.Generate();
            var offset = 0;
            var limit = 10;
            var request = new GetAllTasksCommentsQuery(task.Id, offset, limit);

            _taskQueryServiceHelper.SetupGetExistingTaskAsync(task.Id, task);

            var teamMember = new TeamMember() { UserId = user.Id, ProjectId = task.ProjectId };
            _teamMemberQueryServiceHelper.SetupGetExistingTeamMemberAsync(
                user.Id,
                task.ProjectId,
                teamMember
            );

            var comments = _commentFaker.Generate(10).ToList();

            _commentRepositoryHelper.SetupGetAllAsync(offset, limit, comments);

            _mapperHelper.SetupMapCommentToCommentResponseDto();

            var expectedResult = comments
                .Select(_mapperMock.Object.Map<CommentResponseDto>)
                .ToList();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _commentRepositoryMock.Verify(
                x =>
                    x.GetAllAsync(
                        comment => comment.TasksId == request.TaskId,
                        offset,
                        limit,
                        CancellationToken.None
                    ),
                Times.Once
            );
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

            var task = _taskFaker.Generate();
            var offset = 0;
            var limit = 10;
            var comments = _commentFaker.Generate(10).ToList();
            var request = new GetAllTasksCommentsQuery(task.Id, offset, limit);

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
            var offset = 0;
            var limit = 10;
            var comments = _commentFaker.Generate(10).ToList();
            var request = new GetAllTasksCommentsQuery(task.Id, offset, limit);

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

            _taskQueryServiceHelper.SetupGetExistingTaskAsync(task.Id, task);

            _teamMemberQueryServiceHelper.SetupGetExistingTeamMemberAsync(
                It.IsAny<int>(),
                task.ProjectId,
                new NotFoundException()
            );

            var offset = 0;
            var limit = 10;
            var comments = _commentFaker.Generate(10).ToList();
            var request = new GetAllTasksCommentsQuery(task.Id, offset, limit);

            // Act
            var act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await Assert.ThrowsAsync<NotFoundException>(act);
        }
    }
}
