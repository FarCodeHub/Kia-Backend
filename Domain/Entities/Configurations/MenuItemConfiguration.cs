// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Domain.Entities.reverse.Configurations
{
    public partial class MenuItemConfiguration : IEntityTypeConfiguration<MenuItem>
    {
        public void Configure(EntityTypeBuilder<MenuItem> entity)
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Icon).HasMaxLength(50);

            entity.Property(e => e.Link).HasMaxLength(100);

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.SubTitle).HasMaxLength(100);

            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.MenuItemCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuItems_Users");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.MenuItemModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_MenuItems_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuItems_Roles");

            entity.HasOne(d => d.Permission)
                .WithMany(p => p.MenuItems)
                .HasForeignKey(d => d.PermissionId)
                .HasConstraintName("FK_MenuItems_Permissions");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<MenuItem> entity);
    }
}
