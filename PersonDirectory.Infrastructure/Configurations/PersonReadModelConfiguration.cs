using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonDirectory.Domain.PersonManagement.ReadModels;
using PersonDirectory.Shared.Extensions;

namespace PersonDirectory.Infrastructure.Configurations;

public class PersonReadModelConfiguration : IEntityTypeConfiguration<PersonReadModel>
{
    public void Configure(EntityTypeBuilder<PersonReadModel> builder)
    {
        builder.ToTable("PersonReadModels", "ReadModels");

        builder.HasKey(c => c.Id);
        builder.HasIndex(c => c.PersonId);

        builder.Property(x => x.PhoneNumbers)
            .HasJsonConversion();

        builder.Property(x => x.Photo)
            .HasJsonConversion();
    }
}
