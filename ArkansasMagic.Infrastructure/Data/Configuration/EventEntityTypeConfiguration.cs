using ArkansasMagic.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArkansasMagic.Infrastructure.Data.Configuration
{
    public class EventEntityTypeConfiguration : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> entity)
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .ValueGeneratedNever();

            entity.Property(e => e.OrganizationId)
                .IsRequired();

            entity.Property(e => e.GroupId)
                .HasMaxLength(36);

            entity.Property(e => e.ShortCode)
                .HasMaxLength(10);

            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Cost)
                .IsRequired()
                .HasColumnType(MySqlConstants.DataType.Currency);

            entity.Property(e => e.StartingTableNumber)
                .IsRequired();

            entity.Property(e => e.HasTop8)
                .IsRequired();

            entity.Property(e => e.IsAdHoc)
                .IsRequired();

            entity.Property(e => e.IsOnline)
                .IsRequired();

            entity.Property(e => e.OfficialEventTemplate)
                .HasMaxLength(300);

            entity.Property(e => e.Reservations)
                .IsRequired();

            entity.Property(e => e.Registrations)
                .IsRequired();

            entity.Property(e => e.IsReserved)
                .IsRequired();

            entity.Property(e => e.StartTime);

            entity.Property(e => e.Format)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.FormatId)
                .IsRequired()
                .HasMaxLength(36);

            entity.Property(e => e.RequiredTeamSize)
                .IsRequired();

            entity.Property(e => e.EmailAddress)
                .HasMaxLength(150);

            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15);

            entity.Property(e => e.TimeZone)
                .HasMaxLength(50);

            entity.Property(e => e.RuleEnforcementLevel)
                .HasMaxLength(36);

            entity.Property(e => e.PairingType)
                .HasMaxLength(36);

            entity.Property(e => e.CreatedDateUtc)
                .HasColumnType(MySqlConstants.DataType.Timestamp)
                .HasDefaultValueSql(MySqlConstants.Defaults.CurrentTimstamp);

            entity.Property(e => e.UpdatedDateUtc)
                .HasColumnType(MySqlConstants.DataType.Timestamp)
                .HasDefaultValueSql(MySqlConstants.Defaults.CurrentTimstamp)
                .ValueGeneratedOnAddOrUpdate();

            //entity.HasOne(e => e.Host)
            //    .WithMany(e => e.Events)
            //    .HasForeignKey(e => e.OrganizationId)
            //    .HasConstraintName("FK_Events_OrganizationId");
        }
    }
}
