using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Data.Contexts
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }
        public DbSet<CorrespondenceType> CorrespondenceTypes => Set<CorrespondenceType>();
        public DbSet<CorrespondenceTypesByTeam> CorrespondenceTypesByTeams => Set<CorrespondenceTypesByTeam>();
        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<LeadSubject> LeadSubjects => Set<LeadSubject>();
        public DbSet<MoJMinister> MoJMinisters => Set<MoJMinister>();
        public DbSet<PublicHoliday> PublicHolidays => Set<PublicHoliday>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Salutation> Salutations => Set<Salutation>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<User> Users => Set<User>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);

        }
    }
}
