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

namespace Service.Services.SessionSurvey
{
    public class SessionSurveyService : ISessionSurveyService
    {
        private readonly IRepository _repository;

        public SessionSurveyService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.SessionSurvey> GetAll()
        {
            return _repository.GetAll<Domain.Entities.SessionSurvey>();
        }

        public IQueryable<Domain.Entities.SessionSurvey> FindById(int id)
        {
            return _repository.Find<Domain.Entities.SessionSurvey>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.SessionSurvey>> Add(Domain.Entities.SessionSurvey inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.SessionSurvey>> Update(Domain.Entities.SessionSurvey inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.SessionSurvey>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.SessionSurvey>(entity, inpute);

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.SessionSurvey>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.SessionSurvey>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }


        public IQueryable<Domain.Entities.SessionSurvey> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.SessionSurvey>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.SessionSurvey>(queries))
                    .Paginate(pagination));
        }
    }
}