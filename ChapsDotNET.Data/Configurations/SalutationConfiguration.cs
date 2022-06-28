using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapsDotNET.Data.Configurations
{
    public class SalutationConfiguration : IEntityTypeConfiguration<Salutation>
    {
        public void Configure(EntityTypeBuilder<Salutation> builder)
        {
            builder.HasKey(x => x.salutationID);
            builder.Property(x => x.Detail).IsRequired().HasMaxLength(10);
        }
    }
}
