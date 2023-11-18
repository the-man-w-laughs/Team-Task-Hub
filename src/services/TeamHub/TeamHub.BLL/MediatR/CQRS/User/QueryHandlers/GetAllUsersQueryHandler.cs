using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using TeamHub.BLL.Dtos;
using TeamHub.DAL.Contracts.Repositories;

namespace TeamHub.BLL.MediatR.CQRS.Users.Queries;

public class GetAllUsersQueryHandler
    : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetAllUsersQueryHandler(
        IHttpContextAccessor httpContextAccessor,
        IUserRepository userRepository,
        IMapper mapper
    )
    {
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var users = await _userRepository.GetAllAsync(
            request.Offset,
            request.Limit,
            cancellationToken
        );
        var userResponseDtos = users.Select(project => _mapper.Map<UserResponseDto>(project));

        return userResponseDtos;
    }
}
