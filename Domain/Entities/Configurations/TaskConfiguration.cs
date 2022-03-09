﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

#nullable disable

namespace Domain.Entities.reverse.Configurations
{
    public partial class TaskConfiguration : IEntityTypeConfiguration<Task>
    {
        public void Configure(EntityTypeBuilder<Task> entity)
        {
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.DuoAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.StatusTitle)
                .HasMaxLength(12)
                .HasComputedColumnSql("(case [Status] when (1) then N'انجام نشده' when (2) then N'درحال انجام' when (3) then N'انجام شده'  end)", false);

            entity.HasOne(d => d.Communication)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CommunicationId)
                .HasConstraintName("FK_Tasks_Calls");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.TaskCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Users");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Customers");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.TaskModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Tasks_Users1");

            entity.HasOne(d => d.Employee)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Employees");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Roles");

            entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Tasks_Tasks1");

            entity.HasOne(d => d.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.ProjectId)
                .HasConstraintName("FK_Tasks_Projects");

            entity.HasOne(d => d.ResultBase)
                .WithMany(p => p.TaskResultBases)
                .HasForeignKey(d => d.ResultBaseId)
                .HasConstraintName("FK_Tasks_BaseValues1");

            entity.HasOne(d => d.TypeBase)
                .WithMany(p => p.TaskTypeBases)
                .HasForeignKey(d => d.TypeBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_BaseValues");

            entity.HasOne(d => d.Case)
                .WithMany(p => p.Tasks)
                .HasForeignKey(d => d.CaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tasks_Cases");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Task> entity);
    }
}
