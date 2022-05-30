using ChapsDotNET.DAL;
using ChapsDotNET.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Contexts
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
    }
}
