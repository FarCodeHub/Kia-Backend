using Application.CommandQueries.Case.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Case.Command.Update
{
    public class UpdateCaseCommand : CommandBase, IRequest<CaseModel>, IMapFrom<Domain.Entities.Case>, ICommand
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? ConsultantId { get; set; }
        public int? PresentorId { get; set; }
        public int StatusBaseId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCaseCommand, Domain.Entities.Case>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCaseCommandHandler : IRequestHandler<UpdateCaseCommand, CaseModel>
    {
        private readonly IMapper _mapper;
        private readonly ICaseService _contractService;
        private readonly IRepository _repository;

        public UpdateCaseCommandHandler(IMapper mapper, ICaseService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }

        public async Task<CaseModel> Handle(UpdateCaseCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            // update properties

            var updatedEntity = await _contractService.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CaseModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
