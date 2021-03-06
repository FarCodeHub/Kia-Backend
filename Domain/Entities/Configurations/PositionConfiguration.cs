// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Domain.Entities.reverse.Configurations
{
    public partial class PositionConfiguration : IEntityTypeConfiguration<Position>
    {
        public void Configure(EntityTypeBuilder<Position> entity)
        {
            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.CreatedById).HasDefaultValueSql("((1))");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.OwnerRoleId).HasDefaultValueSql("((1))");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.UniqueName)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Positions)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Positions_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Position> entity);
    }
}
