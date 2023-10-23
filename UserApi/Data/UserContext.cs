using Microsoft.EntityFrameworkCore;
using UserApi.models;

namespace UserApi.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext>options) : base(options) { }

        public DbSet<UserModel> userModels { get; set; }
    }
}
