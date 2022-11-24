using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
    public class CorrespondenceTypesByTeamConfiguration : IEntityTypeConfiguration<CorrespondenceTypesByTeam>
    {
        public void Configure(EntityTypeBuilder<CorrespondenceTypesByTeam> builder)
        {
            builder.HasKey(a => new { a.CorrespondenceTypeID, a.TeamID });
        }
    }
}
