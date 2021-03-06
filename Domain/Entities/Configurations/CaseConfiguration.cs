// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
using Domain.Entities.reverse;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;
using System;

namespace Domain.Entities.Configurations
{
    public partial class CaseConfiguration : IEntityTypeConfiguration<Case>
    {
        public void Configure(EntityTypeBuilder<Case> entity)
        {
            entity.ToTable("Cases", "dbo");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getdate())");

            entity.Property(e => e.ModifiedAt).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Consultant)
                .WithMany(p => p.CasConsultants)
                .HasForeignKey(d => d.ConsultantId)
                .HasConstraintName("FK_Cases_Employees1");

            entity.HasOne(d => d.CreatedBy)
                .WithMany(p => p.CasCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Users");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Cases)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Customers");

            entity.HasOne(d => d.ModifiedBy)
                .WithMany(p => p.CasModifiedBies)
                .HasForeignKey(d => d.ModifiedById)
                .HasConstraintName("FK_Cases_Users1");

            entity.HasOne(d => d.OwnerRole)
                .WithMany(p => p.Cas)
                .HasForeignKey(d => d.OwnerRoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_Roles");

            entity.HasOne(d => d.Presentor)
                .WithMany(p => p.CasPresentors)
                .HasForeignKey(d => d.PresentorId)
                .HasConstraintName("FK_Cases_Employees");

            entity.HasOne(d => d.StatusBase)
                .WithMany(p => p.Cas)
                .HasForeignKey(d => d.StatusBaseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cases_BaseValues");

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Case> entity);
    }
}
