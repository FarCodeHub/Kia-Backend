using System.Threading;
using Application.CommandQueries.Person.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.Person.Command.Update
{
    public class UpdatePersonAvatarCommand : CommandBase, IRequest<PersonModel>, IMapFrom<Domain.Entities.Person>, ICommand
    {
        public int Id { get; set; }
        public string ProfilePhotoUrl { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdatePersonAvatarCommand, Domain.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }

    public class UpdatePersonAvatarCommandHandler : IRequestHandler<UpdatePersonAvatarCommand, PersonModel>
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        private readonly IRepository _repository;
        public UpdatePersonAvatarCommandHandler(IMapper mapper, IPersonService personService, IRepository repository)
        {
            _mapper = mapper;
            _personService = personService;
            _repository = repository;
        }

        public async System.Threading.Tasks.Task<PersonModel> Handle(UpdatePersonAvatarCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _personService.UpdateAvatar(request.Id, request.ProfilePhotoUrl, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PersonModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}