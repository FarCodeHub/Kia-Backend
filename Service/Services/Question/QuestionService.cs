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

namespace Service.Services.Question
{
    public class QuestionService : IQuestionService
    {
        private readonly IRepository _repository;

        public QuestionService(IRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Domain.Entities.Question> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Question>();
        }

        public IQueryable<Domain.Entities.Question> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Question>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Question>> Add(Domain.Entities.Question inpute)
        {
            return  _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Question>> Update(Domain.Entities.Question inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Question>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.Question>(entity, inpute);

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Question>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Question>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Question> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Question>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Question>(queries))
                    .Paginate(pagination));
        }
    }
}