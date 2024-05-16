using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonDirectory.Domain.PersonManagement;

namespace PersonDirectory.Infrastructure.Configurations;

public class PersonRelationshipConfiguration : IEntityTypeConfiguration<PersonRelationship>
{
    public void Configure(EntityTypeBuilder<PersonRelationship> builder)
    {
        builder.ToTable("PersonRelationships");

        builder.HasKey(x => x.Id);

        builder.HasIndex(x => new { x.TargetPersonId, x.RelatedPersonId });

        builder.HasOne(x => x.RelatedPerson).WithMany().HasForeignKey(rpp => rpp.RelatedPersonId)
                   .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(x => x.TargetPerson).WithMany().HasForeignKey(rpp => rpp.TargetPersonId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
