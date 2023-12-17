using TeamHub.BLL.Dtos;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.AutoMapperProfiles
{
    public class CommentsProfile : BaseProfile
    {
        public CommentsProfile()
        {
            CreateMap<CommentRequestDto, Comment>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(comment => comment.Content))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<Comment, CommentResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(comment => comment.Id))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(comment => comment.Author))
                .ForMember(dest => dest.TasksId, opt => opt.MapFrom(comment => comment.TasksId))
                .ForMember(dest => dest.Content, opt => opt.MapFrom(comment => comment.Content))
                .ForMember(
                    dest => dest.CreatedAt,
                    opt => opt.MapFrom(comment => comment.CreatedAt)
                );
        }
    }
}
