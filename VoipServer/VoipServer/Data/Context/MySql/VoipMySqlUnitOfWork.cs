using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySql.Data.EntityFramework;
using VoipServer.Data.Entities;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace VoipServer.Data.Context.MySql
{
    public interface IVoipMySqlUnitOfWork : IUnitOfWork
    {
        public Microsoft.EntityFrameworkCore.DbSet<cdr> Cdr { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<crm> Crm { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<cel> Cel { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<quitedQueue> QuitedQueues { get; set; }

    }

    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class VoipMySqlUnitOfWork : DbContext, IVoipMySqlUnitOfWork
    {
        public Microsoft.EntityFrameworkCore.DbSet<cdr> Cdr { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<crm> Crm { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<cel> Cel { get; set; }
        public Microsoft.EntityFrameworkCore.DbSet<quitedQueue> QuitedQueues { get; set; }

        public IModel Model()
        {
            return base.Model;
        }

        public new Microsoft.EntityFrameworkCore.DbSet<TEntity> Set<TEntity>() where TEntity : class, IBaseEntity
        {
            return base.Set<TEntity>();
        }


        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            try
            {
                return await _appDbContext.SaveChangesAsync(cancellationToken);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int Save()
        {
            try
            {
                throw new NotImplementedException(); // doesnt work with syncronous
                return _appDbContext.Save();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        private readonly VoipMySqlUnitOfWork _appDbContext;

        public VoipMySqlUnitOfWork(DbContextOptions<VoipMySqlUnitOfWork> options)
            : base(options)
        {
            _appDbContext = this;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");
            modelBuilder.Entity<cdr>();
            modelBuilder.Entity<crm>();
            modelBuilder.Entity<cel>();
            modelBuilder.Entity<quitedQueue>();
            //modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            //modelBuilder.ApplyConfigurationsFromAssembly(AppDomain.CurrentDomain.GetAssemblies()
            //    .First(x => x.FullName.Contains("VoipServer.Models.VoipCrm")));


            base.OnModelCreating(modelBuilder);
        }

        public DbContext Context()
        {
            return _appDbContext;
        }
    }
}
