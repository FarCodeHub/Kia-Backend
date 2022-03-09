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

namespace Service.Services.Person
{
    public class PersonService : IPersonService
    {
        private IRepository _repository;
        private readonly IUpLoader _upLoader;
        public PersonService(IRepository repository, IUpLoader upLoader)
        {
            _repository = repository;
            _upLoader = upLoader;
        }
        public IRepository Repository { get => _repository; set => _repository = value; }


        public IQueryable<Domain.Entities.Person> GetAll()
        {
            return _repository.GetAll<Domain.Entities.Person>();
        }

        public IQueryable<Domain.Entities.Person> FindById(int id)
        {
            var a = _repository.Find<Domain.Entities.Person>(cfg => cfg.ObjectId(id));

            return _repository.Find<Domain.Entities.Person>(cfg => cfg.ObjectId(id));
        }

        public async Task<EntityEntry<Domain.Entities.Person>> Add(Domain.Entities.Person inpute)
        {
            return _repository.Insert(inpute);
        }

        public async Task<EntityEntry<Domain.Entities.Person>> Add(Domain.Entities.Person inpute, string profilePhotoUrl)
        {
            if (!string.IsNullOrEmpty(profilePhotoUrl))
            {
                var profileUrl = await _upLoader.UpLoadAsync(profilePhotoUrl,
                    CustomPath.PersonProfile);

                inpute.ProfilePhotoUrl = profileUrl.ReletivePath;
            }
            return _repository.Insert(inpute);
        }

        public Task<EntityEntry<Domain.Entities.Person>> Update(Domain.Entities.Person inpute, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<EntityEntry<Domain.Entities.Person>> UpdateAvatar(int id, string profilePhotoUrl, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Person>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);
            if (!string.IsNullOrEmpty(profilePhotoUrl))
            {
                var profileUrl = await _upLoader.UpLoadAsync(profilePhotoUrl,
                    CustomPath.PersonProfile, cancellationToken);

                entity.ProfilePhotoUrl = profileUrl.ReletivePath;
            }
            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Person>> Update(Domain.Entities.Person inpute, string profilePhotoUrl, bool fullUpdate,
            CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Person>(c =>
                    c.ObjectId(inpute.Id))
                .FirstOrDefaultAsync(cancellationToken);

            if (inpute.FirstName != null)
            {
                entity.FirstName = inpute.FirstName;
            }
            if (inpute.LastName != null)
            {
                entity.LastName = inpute.LastName;
            }
            if (inpute.GenderBaseId != null)
            {
                entity.GenderBaseId = inpute.GenderBaseId;
            }
            if (inpute.ZipCodeId != null)
            {
                entity.ZipCodeId = inpute.ZipCodeId;
            }
            if (inpute.Phone1 != null)
            {
                entity.Phone1 = inpute.Phone1;
            }
            if (inpute.Phone2 != null)
            {
                entity.Phone2 = inpute.Phone2;
            }
            if (inpute.Phone3 != null)
            {
                entity.Phone3 = inpute.Phone3;
            }

            if (inpute.Address != null)
            {
                entity.Address = inpute.Address;
            }

            if (fullUpdate)
            {
                entity.BirthPlaceId = inpute.BirthPlaceId;
                entity.BirthDate = inpute.BirthDate;
                entity.FatherName = inpute.FatherName;
                entity.Email = inpute.Email;
                entity.IdentityCode = inpute.IdentityCode;
                entity.NationalCode = inpute.NationalCode;
                entity.PostalCode = inpute.PostalCode;
                entity.ProfilePhotoUrl = inpute.ProfilePhotoUrl;

                if (!string.IsNullOrEmpty(profilePhotoUrl))
                {
                    var profileUrl = await _upLoader.UpLoadAsync(profilePhotoUrl,
                        CustomPath.PersonProfile, cancellationToken);

                    entity.ProfilePhotoUrl = profileUrl.ReletivePath;
                }
            }



            return _repository.Update(entity);
        }


        public async Task<EntityEntry<Domain.Entities.Person>> SoftDelete(int id, CancellationToken cancellationToken)
        {
            var entity = await _repository
                .Find<Domain.Entities.Person>(c =>
                    c.ObjectId(id))
                .FirstOrDefaultAsync(cancellationToken);

            return _repository.Delete(entity);
        }

        public IQueryable<Domain.Entities.Person> Search(Pagination pagination, List<SearchQueryItem> queries)
        {
            return _repository
                .GetAll<Domain.Entities.Person>(config => config
                    .ConditionExpression(SearchQueryMaker.MakeSearchQuery<Domain.Entities.Person>(queries))
                    .Paginate(pagination));
        }
    }
}