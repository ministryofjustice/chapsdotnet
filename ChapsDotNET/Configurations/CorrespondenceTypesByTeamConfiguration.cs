using System;
using ChapsDotNET.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChapsDotNET.Configurations
{
	public class CorrespondenceTypesByTeamConfiguration: IEntityTypeConfiguration<CorrespondenceTypesByTeam>
	{
        
        public void Configure(EntityTypeBuilder<CorrespondenceTypesByTeam> builder)
        {
            
            builder.HasKey(a => new { a.CorrespondenceTypeID, a.TeamID });

        }
    }
}

