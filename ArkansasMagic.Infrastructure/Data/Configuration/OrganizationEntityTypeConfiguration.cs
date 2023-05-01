using ArkansasMagic.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArkansasMagic.Infrastructure.Data.Configuration
{
    public class OrganizationEntityTypeConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.Name)
                .IsRequired();

            entity.Property(e => e.Latitude)
                .IsRequired()
                .HasColumnType(MySqlConstants.DataType.Coordinate);

            entity.Property(e => e.Longitude)
                .IsRequired()
                .HasColumnType(MySqlConstants.DataType.Coordinate);

            entity.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(36);

            entity.Property(e => e.PhoneNumbers);

            entity.Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Website)
                .HasMaxLength(150);

            entity.Property(e => e.Websites);

            entity.Property(e => e.Brands);

            entity.Property(e => e.PostalAddress)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(e => e.City)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(6);

            entity.Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasMaxLength(15);

            entity.Property(e => e.IsPremium)
                .IsRequired();

            entity.Property(e => e.IsTestStore)
                .IsRequired();

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType(MySqlConstants.DataType.Timestamp)
                .HasDefaultValueSql(MySqlConstants.Defaults.CurrentTimstamp);

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType(MySqlConstants.DataType.Timestamp)
                .HasDefaultValueSql(MySqlConstants.Defaults.CurrentTimstamp)
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
