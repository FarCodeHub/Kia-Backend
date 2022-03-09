using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Service.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Service.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private IRepository _repository;
        private readonly IPersonService _personService;
        private readonly ICaseService _caseService;
        public IRepository Repository { get => _repository; set => _repository = value; }

        public CustomerService(IRepository repository, IPersonService personService, ICaseService caseService)
        {
            _repository = repository;
            _personService = personService;
            _caseService = caseService;
        }

        public IQueryable<Domain.Entities.Customer> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Customer>();
        }

        public IQueryable<Domain.Entities.Customer> FindById(int id)
        {
            return _repository.Find<Domain.Entities.Customer>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Customer>> Add(Domain.Entities.Customer inpute)
        {
            var customer = _repository.Insert(inpute);
            await _caseService.Add(new Domain.Entities.Case() { Customer = customer.Entity });
            return customer;
        }

        public async Task<EntityEntry<Domain.Entities.Customer>> AddWithOutCase(Domain.Entities.Customer inpute)
        {
            var customer = _repository.Insert(inpute);
            return customer;
        }

        public async Task<EntityEntry<Domain.Entities.Customer>> Merge(int idBase, int idMerge)
        {
            var baseCustomer = await _repository
                .Find<Domain.Entities.Customer>(c =>
                    c.ObjectId(idBase)).Include(x => x.Person)
                .FirstOrDefaultAsync();

            var mergeCustomer = await _repository
                .Find<Domain.Entities.Customer>(c =>
                    c.ObjectId(idMerge)).Include(x => x.Person)
                .FirstOrDefaultAsync();


            var mergedPhones = FillPhones(baseCustomer.Person, mergeCustomer.Person);
            var i = 1;
            foreach (var phone in mergedPhones)
            {
                switch (i)
                {
                    case 1:
                        baseCustomer.Person.Phone1 = phone;
                        i++;
                        continue;
                    case 2:
                        baseCustomer.Person.Phone2 = phone;
                        i++;
                        continue;
                    case 3:
                        baseCustomer.Person.Phone3 = phone;
                        i++;
                        break;
                }
            }


            _repository.Update(baseCustomer.Person);


            var newCustomer = _repository.Update(baseCustomer);
            foreach (var session in _repository.GetQuery<Domain.Entities.Task>().Where(x => x.CustomerId == mergeCustomer.Id))
            {
                session.Customer = newCustomer.Entity;
                _repository.Update(session);
            }

            _repository.Delete(mergeCustomer);
            _repository.Delete(mergeCustomer.Person);
            return newCustomer;
        }


        private void MergePhones()
        {

        }

        private List<string> FillPhones(Domain.Entities.Person baseperson, Domain.Entities.Person mergeperson)
        {
            var phones = new List<string>();

            if (!string.IsNullOrEmpty(baseperson.Phone1))
            {
                phones.Add(baseperson.Phone1);
            }
            if (!string.IsNullOrEmpty(baseperson.Phone2))
            {
                phones.Add(baseperson.Phone2);
            }
            if (!string.IsNullOrEmpty(baseperson.Phone3))
            {
                phones.Add(baseperson.Phone3);
            }

            if (!string.IsNullOrEmpty(mergeperson.Phone1))
            {
                if (!phones.Contains(mergeperson.Phone1))
                {
                    phones.Add(mergeperson.Phone1);
                }
            }
            if (!string.IsNullOrEmpty(mergeperson.Phone2))
            {
                if (!phones.Contains(mergeperson.Phone2))
                {
                    phones.Add(mergeperson.Phone2);
                }
            }
            if (!string.IsNullOrEmpty(mergeperson.Phone3))
            {
                if (!phones.Contains(mergeperson.Phone3))
                {
                    phones.Add(mergeperson.Phone3);
                }
            }

            return phones;
        }


        public async Task<EntityEntry<Domain.Entities.Customer>> Update(Domain.Entities.Customer inpute,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Customer>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
                .Update<Domain.Entities.Customer>(entity, inpute);

            return _repository.Update(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Customer>> Block(int id,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Customer>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            entity.IsBlocked = true;

            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Customer>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Customer>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public async Task<EntityEntry<Domain.Entities.Customer>> Add(Domain.Entities.Customer inpute, Domain.Entities.Person inputePerson)
        {
            var person = await _personService.Add(inputePerson);
            inpute.Person = person.Entity;
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Customer>> Update(Domain.Entities.Customer inpute,
            Domain.Entities.Person personInput,
            bool fullUpdate,
            CancellationToken cancellationToken)
        {
            var customer = await _repository
                .Find<Domain.Entities.Customer>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            personInput.Id = customer.PersonId;
            var person = await _personService.Update(personInput, null, fullUpdate, cancellationToken);

            customer.Person = person.Entity;


            return _repository.Update(customer);
        }
    }
}