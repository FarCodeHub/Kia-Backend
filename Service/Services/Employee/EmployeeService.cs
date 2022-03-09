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

namespace Service.Services.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private IRepository _repository;
        private readonly IPersonService _personService;
        public IRepository Repository { get => _repository; set => _repository = value; }

        public EmployeeService(IRepository repository, IPersonService personService)
        {
            _repository = repository;
            _personService = personService;
        }

        public IQueryable<Domain.Entities.Employee> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Employee>();
        }

        public IQueryable<Domain.Entities.Employee> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Employee>(cfg => cfg.ObjectId(id));
        }

        public async Task<Domain.Entities.Employee> FindByPositionUniqueName(string positionUniqueName)
        {
            return await _repository.Find<Domain.Entities.Employee>(x => x.ConditionExpression(x => x.UnitPosition.Position.UniqueName == positionUniqueName)).FirstOrDefaultAsync(CancellationToken.None);
        }


        public Task<EntityEntry<Domain.Entities.Employee>> Update(Domain.Entities.Employee inpute, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<EntityEntry<Domain.Entities.Employee>> Add(Domain.Entities.Employee inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Employee>> Update(Domain.Entities.Employee inpute, Domain.Entities.Person personInput, string profilePhotoUrl,
            CancellationToken cancellationToken)
        {
            var employee = await _repository
                .Find<Domain.Entities.Employee>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            personInput.Id = employee.PersonId;
            var person = await _personService.Update(personInput, profilePhotoUrl, true, cancellationToken);

            employee.Person = person.Entity;

            employee.LeaveDate = inpute.LeaveDate;
            employee.UnitPositionId = inpute.UnitPositionId;

            return _repository.Update(employee);
        }


        public async Task<EntityEntry<Domain.Entities.Employee>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Employee>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Employee> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Employee>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Employee>(queries))
                    .Paginate(pagination));
        }
    }
}