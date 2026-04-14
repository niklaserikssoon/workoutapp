using Microsoft.EntityFrameworkCore;
using User_API.Models;

namespace User_API.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext()
        {
        }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Exercise> Exercises { get; set; } = null!;
        public virtual DbSet<Workout> Workouts { get; set; } = null!;

    }
}
