using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Data.Contexts
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<CorrespondenceTypesByTeam> CorrespondenceTypesByTeams => Set<CorrespondenceTypesByTeam>();
        public DbSet<CorrespondenceType> CorrespondenceTypes => Set<CorrespondenceType>();
        public DbSet<Salutation> Salutations => Set<Salutation>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<PublicHoliday> PublicHolidays => Set<PublicHoliday>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        }
    }
}
