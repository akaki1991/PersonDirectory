using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonDirectory.Domain.PersonManagement;
using PersonDirectory.Shared.Extensions;

namespace PersonDirectory.Infrastructure.Configurations;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("Persons");

        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.PersonalNumber).IsUnique();

        builder.Property(x => x.Address).HasJsonConversion();
        builder.Property(x => x.PhoneNumbers).HasJsonConversion();
        builder.Property(x => x.Photo).HasJsonConversion();
    }
}
