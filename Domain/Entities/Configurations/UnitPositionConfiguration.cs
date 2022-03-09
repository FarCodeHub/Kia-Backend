﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Domain.Entities.reverse.Configurations
{
    public partial class UnitPositionConfiguration : IEntityTypeConfiguration<UnitPosition>
    {
        public void Configure(EntityTypeBuilder<UnitPosition> entity)
        {
            entity.ToTable("UnitPosition");

            entity.HasIndex(e => new { e.UnitId, e.PositionId })
                .IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Position)
                .WithMany(p => p.UnitPositions)
                .HasForeignKey(d => d.PositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitPosition_Positions");

            entity.HasOne(d => d.Unit)
                .WithMany(p => p.UnitPositions)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UnitPosition_Units");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<UnitPosition> entity);
    }
}