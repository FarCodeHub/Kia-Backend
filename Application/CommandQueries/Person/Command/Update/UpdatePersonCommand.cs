using System;
using Application.CommandQueries.Person.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Utilities;


namespace Application.CommandQueries.Person.Command.Update
{
    public class UpdatePersonCommand : CommandBase, IRequest<PersonModel>, IMapFrom<Domain.Entities.Person>, ICommand
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public string? NationalCode { get; set; }
        public string? IdentityCode { get; set; }
        public int? BirthPlaceId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int GenderId { get; set; } = default!;
        public string PhoneNumbers { get; set; } = default!;
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
        public int? ZipCodeId { get; set; }
        public string? Address { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonCommand, Domain.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, PersonModel>
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        private readonly IRepository _repository;
        private readonly IUpLoader _upLoader;
        public UpdatePersonCommandHandler(IMapper mapper, IPersonService personService, IRepository repository, IUpLoader upLoader)
        {
            _mapper = mapper;
            _personService = personService;
            _repository = repository;
            _upLoader = upLoader;
        }

        public async Task<PersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
        {
            var entity = await _personService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            var updatedEntity = await _personService.Update(entity, request.ProfilePhotoUrl,true, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PersonModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
