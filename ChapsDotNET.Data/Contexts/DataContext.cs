using System.Security.Claims;
using ChapsDotNET.Data.Entities;
using ChapsDotNET.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ChapsDotNET.Data.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ChapsDotNET.Data.Contexts
{
    public class DataContext : DbContext
    {
        private readonly IHttpContextAccessor? _httpContextAccessor;

        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public int? CurrentUserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst("userId") ??
                                  _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId))
                {
                    return userId;
                }

                throw new InvalidOperationException("No UserId claim present in the current context");
            }
        }

        public DbSet<Campaign> Campaigns => Set<Campaign>();
        public DbSet<CorrespondenceType> CorrespondenceTypes => Set<CorrespondenceType>();
        public DbSet<CorrespondenceTypesByTeam> CorrespondenceTypesByTeams => Set<CorrespondenceTypesByTeam>();
        public DbSet<LeadSubject> LeadSubjects => Set<LeadSubject>();
        public DbSet<MoJMinister> MoJMinisters => Set<MoJMinister>();
        public DbSet<MP> MPs => Set<MP>();
        public DbSet<PublicHoliday> PublicHolidays => Set<PublicHoliday>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<Salutation> Salutations => Set<Salutation>();
        public DbSet<Stage> Stages => Set<Stage>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<Alert> Alerts => Set<Alert>();
        public DbSet<DotNetAudit> DotNetAudits => Set<DotNetAudit>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DataContext).Assembly);
        }

       
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //ChangeTracker.AutoDetectChangesEnabled = true;
            ChangeTracker.DetectChanges();

            var changeSet = ChangeTracker.Entries<IAuditable>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Deleted || e.State == EntityState.Modified).ToList();

            // save the entities and entity.States for the audit
            var tempChangeInfo = changeSet.Select(e => new
            {
                e.Entity,
                EntityState = e.State

            }).ToList();

            int result;

            // Transaction for entity save
            var entityTransaction = await Database.BeginTransactionAsync();
            
            try
            {
                result = await base.SaveChangesAsync(cancellationToken);
                await entityTransaction.CommitAsync();
            }

            catch (Exception)
            {
                await entityTransaction.RollbackAsync();
                throw new DbUpdateException("Error saving entities");
            }
            
            var auditRecords = tempChangeInfo.Select(changeInfo =>
            {
                var entry = Entry(changeInfo.Entity);
                return CreateAuditRecordForChange(entry, changeInfo.EntityState);
            }).Where(a => a != null).ToList();


            //Transaction for Audit 
            if(auditRecords.Count > 0)
            {
                var auditTransaction = await Database.BeginTransactionAsync();
                
                try
                {
                    DotNetAudits.AddRange(auditRecords);
                    await base.SaveChangesAsync(cancellationToken);
                    await auditTransaction.CommitAsync();
                }
                catch (Exception)
                {
                    await auditTransaction.RollbackAsync();
                    throw new DbUpdateException("Error saving audit records");
                }
                
            }
            return result;

        }

        private DotNetAudit CreateAuditRecordForChange(EntityEntry<IAuditable> entry, EntityState entityState)
        {
            Actions action = entityState switch
            {
                EntityState.Added => Actions.Added,
                EntityState.Deleted => Actions.Deleted,
                EntityState.Modified => Actions.Modified,
                _ => throw new InvalidOperationException("Invalid entity state")
            };

            
            var primaryKeyProperty = entry.Metadata.FindPrimaryKey()!.Properties.Single();
            var primaryKeyValue = entry.CurrentValues[primaryKeyProperty];
            var auditRecord = new DotNetAudit
            {
                Date = DateTime.Now,
                UserId = (int)CurrentUserId!,
                ObjectPrimaryKey = Convert.ToInt32(primaryKeyValue),
                //Object = primaryKeyProperty.DeclaringEntityType.DisplayName().ToString(), ** warning CS0618 ** 
                Object = ((IEntityType)primaryKeyProperty.DeclaringType).DisplayName(),
                ActionId = (int)action
            };

            if (entry.CurrentValues.Properties.Any(p => p.Name == "CorrespondenceID"))
            {
                var correspondenceIdValue = entry.CurrentValues["CorrespondenceID"];
                auditRecord.RootPrimaryKey = Convert.ToInt32(correspondenceIdValue);
            }
            return auditRecord;
        }
    }
}
