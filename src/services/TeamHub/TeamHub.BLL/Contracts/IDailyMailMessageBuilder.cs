using System.Net.Mail;
using TeamHub.BLL.Dtos.TeamMember;
using TeamHub.DAL.Models;

namespace TeamHub.BLL.Contracts
{
    public interface IDailyMailMessageBuilder
    {
        MailMessage CreateDailyMailMessage(
            string sourceEmail,
            User user,
            List<TeamMemberDto> teamMemberResponseDtos
        );
    }
}
