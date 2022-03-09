using System;
using Application.CommandQueries.Commission.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Commission.Command.Update
{
    public class UpdateCommissionCommand : CommandBase, IRequest<CommissionModel>, IMapFrom<Domain.Entities.Commission>, ICommand
    {
        public int Id { get; set; }
        public long Amount { get; set; } = default!;
        public bool IsPaid { get; set; } = default!;
        public DateTime? PaidAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCommissionCommand, Domain.Entities.Commission>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCommissionCommandHandler : IRequestHandler<UpdateCommissionCommand, CommissionModel>
    {
        private readonly IMapper _mapper;
        private readonly ICommissionService _contractService;
        private readonly IRepository _repository;

        public UpdateCommissionCommandHandler(IMapper mapper, ICommissionService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }

        public async Task<CommissionModel> Handle(UpdateCommissionCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _contractService.Update(_mapper.Map<Domain.Entities.Commission>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CommissionModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
