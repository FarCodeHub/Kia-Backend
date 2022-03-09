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

namespace Service.Services.Branch
{
    public class BranchService : IBranchService
    {
        private readonly IRepository _repository;

        public BranchService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.Branch> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Branch>();
        }

        public IQueryable<Domain.Entities.Branch> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Branch>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Branch>> Add(Domain.Entities.Branch inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Branch>> Update(Domain.Entities.Branch inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Branch>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.Location = inpute.Location;
            entity.Title = inpute.Title;
            entity.Address = inpute.Address;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Branch>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Branch>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Branch> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Branch>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Branch>(queries))
                    .Paginate(pagination));
        }
    }
}