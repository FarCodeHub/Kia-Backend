// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Domain.Entities.reverse.Configurations
{
    public partial class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> entity)
        {
            entity.HasIndex(e => e.Title)
                .IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Location).HasColumnType("geometry");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.BranchCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.BranchModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Branches_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Branches)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Branches_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Branch> entity);
    }
}
