using StudyHub.Models.Base;

namespace StudyHub.Models.Accounts
{
    public class UserDto : BaseDto
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Password { get; set; }
        public string? PhotoId { get; set; }
    }
}
