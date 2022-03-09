﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Domain.Entities.reverse.Configurations
{
    public partial class UnitConfiguration : IEntityTypeConfiguration<Unit>
    {
        public void Configure(EntityTypeBuilder<Unit> entity)
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.LevelCode)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("((0))");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasOne(d => d.Branch)
                .WithMany(p => p.Units)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Units_Branches");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.UnitCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Units_Users1");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.UnitModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Units_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Units)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Units_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Unit> entity);
    }
}