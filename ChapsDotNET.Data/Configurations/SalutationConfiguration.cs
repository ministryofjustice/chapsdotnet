using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Data.Configurations
{
    public class SalutationConfiguration : IEntityTypeConfiguration<Salutation>
    {
        public void Configure(EntityTypeBuilder<Salutation> builder)
        {
            builder.HasKey(a => new { a.salutationID });
            builder.Property(x => x.Detail).IsRequired().HasMaxLength(10);
        }
    }
}
