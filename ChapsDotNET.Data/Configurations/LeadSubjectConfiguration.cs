using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
    public class LeadSubjectConfiguration : IEntityTypeConfiguration<LeadSubject>
    {
        public void Configure(EntityTypeBuilder<LeadSubject> builder)
        {
            builder.HasKey(x => x.LeadSubjectId);
            builder.Property(x => x.Detail).IsRequired().HasMaxLength(100);
        }
    }
}