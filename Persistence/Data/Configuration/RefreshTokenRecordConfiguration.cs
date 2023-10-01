using System.IO.Compression;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
public class RefreshTokenRecordConfiguration : IEntityTypeConfiguration<RefreshTokenRecord>
{
    public void Configure(EntityTypeBuilder<RefreshTokenRecord> builder){

        // Table name and define principal key

        builder.ToTable("refresh_token_record");
        builder.HasKey(x => x.Id);

        // Properties

        builder.Property(x => x.Id)
            .IsRequired()
            .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
            .HasColumnName("RefreshTokenRecordId");

        builder.Property(x => x.Token)
            .IsRequired()
            .HasColumnName("Token")
            .HasMaxLength(500);

        builder.Property(x => x.RefreshToken)
            .IsRequired()
            .HasColumnName("RefreshToken")
            .HasMaxLength(200);

        builder.Property(x => x.CreationDate)
            .IsRequired()
            .HasColumnName("CreationDate")
            .HasColumnType("DateTime");

        builder.Property(x => x.ExpirationDate)
            .IsRequired()
            .HasColumnName("ExpirationDate")
            .HasColumnType("DateTime");

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasColumnName("IsActive");

        // Relationships

        builder.HasOne(d => d.User)
            .WithMany(p => p.RefreshTokenRecords)
            .HasForeignKey(d => d.UserId)
            .HasConstraintName("FK__Record__UserId__24927208");

    }
}