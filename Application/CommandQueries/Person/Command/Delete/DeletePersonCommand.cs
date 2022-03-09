using Application.CommandQueries.Person.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Person.Command.Delete
{
    public class DeletePersonCommand : CommandBase, IRequest<PersonModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeletePersonCommandHandler : IRequestHandler<DeletePersonCommand, PersonModel>
    {
        private readonly IMapper _mapper;
        private readonly IPersonService _personService;
        private readonly IRepository _repository;

        public DeletePersonCommandHandler(IMapper mapper, IPersonService personService, IRepository repository)
        {
            _mapper = mapper;
            _personService = personService;
            _repository = repository;
        }

        public async Task<PersonModel> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
        {
            var entity = await _personService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<PersonModel>(entity.Entity);
            }

            return null;
        }
    }
}
