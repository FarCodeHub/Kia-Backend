﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace Domain.Entities.reverse.Configurations
{
    public partial class CaseEmployeeChangeConfiguration : IEntityTypeConfiguration<CaseEmployeeChange>
    {
        public void Configure(EntityTypeBuilder<CaseEmployeeChange> entity)
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.Reason)
                .IsRequired()
                .HasMaxLength(250);

            entity.HasOne(d => d.Case)
                .WithMany(p => p.CaseEmployeeChanges)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseEmployeeChanges_Cases");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CaseEmployeeChanxCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseEmployeeChanges_Users1");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.CaseEmployeeChanges)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseEmployeeChanges_Employees");

            entity.HasOne(d => d.EmployeePosition)
                .WithMany(p => p.CaseEmployeeChanges)
                .HasForeignKey(d => d.EmployeePositionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseEmployeeChanges_Positions");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CaseEmployeeChanxModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_CaseEmployeeChanges_Users");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.CaseEmployeeChanges)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CaseEmployeeChanges_Roles");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<CaseEmployeeChange> entity);
    }
}
