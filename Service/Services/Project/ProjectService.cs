using Infrastructure.Models;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Service.Services.Project
{
    public class ProjectService : IProjectService
    {
        private readonly IRepository _repository;
        private readonly IUpLoader _upLoader;
        public ProjectService(IRepository repository, IUpLoader upLoader)
        {
            _repository = repository;
            _upLoader = upLoader;
        }

        public IQueryable<Domain.Entities.Project> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Project>();
        }

        public IQueryable<Domain.Entities.Project> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Project>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Project>> Add(Domain.Entities.Project inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Project>> Update(Domain.Entities.Project inpute, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Project>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Location = inpute.Location;
            entity.FilePath = inpute.FilePath;
            entity.FileUrl = inpute.FileUrl;
            entity.Address = inpute.Address;
            entity.PriorityBaseId = inpute.PriorityBaseId;
            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Project>> Add(Domain.Entities.Project inpute, string projectFileReletiveAddress)
        {
            var project = (Domain.Entities.Project)inpute.Clone();

            if (!string.IsNullOrEmpty(projectFileReletiveAddress))
            {
                var profileUrl = await _upLoader.UpLoadAsync(projectFileReletiveAddress,
                    CustomPath.ProjectFile);

                inpute.FilePath = profileUrl.ReletivePath;
            }

            var projectEntity = _repository.Insert(project);

            foreach (var p in inpute.InverseParent)
            {
                _repository.Insert(new Domain.Entities.Project()
                {
                    Parent = projectEntity.Entity,
                    Title = p.Title,
                    PriorityBaseId = projectEntity.Entity.PriorityBaseId
                });
            }

            return projectEntity;
        }

        public async Task<EntityEntry<Domain.Entities.Project>> Update(Domain.Entities.Project inpute, string projectFileReletiveAddress,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Project>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Location = inpute.Location;

            if (!string.IsNullOrEmpty(projectFileReletiveAddress))
            {
                if (entity.FilePath != projectFileReletiveAddress)
                {
                    var profileUrl = await _upLoader.UpLoadAsync(projectFileReletiveAddress,
                        CustomPath.ProjectFile, cancellationToken);

                    entity.FilePath = profileUrl.ReletivePath;
                }
            }

            entity.FileUrl = inpute.FileUrl;
            entity.Title = inpute.Title;
            entity.Address = inpute.Address;
            entity.PriorityBaseId = inpute.PriorityBaseId;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Project>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Project>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Project> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Project>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Project>(queries))
                    .Paginate(pagination));
        }
    }
}