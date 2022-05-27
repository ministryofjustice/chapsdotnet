using ChapsDotNET.DAL;
using ChapsDotNET.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Contexts
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
                
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<CorrespondenceTypesByTeam> CorrespondenceTypesByTeams { get; set; }
        public DbSet<CorrespondenceType> CorrespondenceTypes { get; set; }
    }
}
