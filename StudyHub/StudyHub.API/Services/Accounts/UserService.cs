using StudyHub.API.Data;
using StudyHub.API.Interfaces.Accounts;
using StudyHub.API.Services.Base;
using StudyHub.Models.Accounts;

namespace StudyHub.API.Services.Accounts
{
    public class UserService : BaseService<UserDto>, IUserService
    {
        public UserService(StudyHubAPIContext context) : base(context)
        {
            Model = context.UserDto;
        }
    }
}
