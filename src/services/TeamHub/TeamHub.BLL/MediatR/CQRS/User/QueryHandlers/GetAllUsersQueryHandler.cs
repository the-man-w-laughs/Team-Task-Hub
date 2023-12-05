using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;
using Shared.Extensions;
using TeamHub.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace TeamHub.BLL.MediatR.CQRS.Users.Queries;

public class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUserQueryService _userService;
    private readonly ILogger<GetAllUsersQueryHandler> _logger;

    public GetAllUsersQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IMapper mapper,
        IUserQueryService userService,
        ILogger<GetAllUsersQueryHandler> logger
    )
    {
        _mapper = mapper;
        _userService = userService;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        // retrieve current user id
        var userId = _httpContextAccessor.GetUserId();

        _logger.LogInformation("User {UserId} is trying to retrieve all users.", userId);

        // check if current user exists
        await _userService.GetExistingUserAsync(userId, cancellationToken);

        // get all users
        var users = await _userRepository.GetAllAsync(
            request.Offset,
            request.Limit,
            cancellationToken
        );
        var userResponseDtos = users.Select(project => _mapper.Map<UserResponseDto>(project));

        _logger.LogInformation(
            "Successfully retrieved all users: {Count}.",
            userResponseDtos.Count()
        );

        return userResponseDtos;
    }
}
