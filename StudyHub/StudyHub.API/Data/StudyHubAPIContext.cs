using Microsoft.EntityFrameworkCore;
using StudyHub.Models.Accounts;

namespace StudyHub.API.Data
{
    public class StudyHubAPIContext : DbContext
    {
        public StudyHubAPIContext (DbContextOptions<StudyHubAPIContext> options)
            : base(options)
        {
        }

        public DbSet<UserDto> UserDto { get; set; } = default!;
    }
}
