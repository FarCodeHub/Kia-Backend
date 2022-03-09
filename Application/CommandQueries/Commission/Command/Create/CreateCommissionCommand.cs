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

namespace Application.CommandQueries.Commission.Command.Create
{
    public class CreateCommissionCommand : CommandBase, IRequest<CommissionModel>, IMapFrom<CreateCommissionCommand>, ICommand
    {
        public int ContractId { get; set; } = default!;
        public long Amount { get; set; } = default!;
        public int EmployeeId { get; set; } = default!;
        public bool IsPaid { get; set; } = default!;
        public DateTime? PaidAt { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCommissionCommand, Domain.Entities.Commission>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCommissionCommandHandler : IRequestHandler<CreateCommissionCommand, CommissionModel>
    {
        private readonly IMapper _mapper;
        private readonly ICommissionService _contractService;
        private readonly IRepository _repository;

        public CreateCommissionCommandHandler(IMapper mapper, ICommissionService contractService, IRepository repository)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
        }


        public async Task<CommissionModel> Handle(CreateCommissionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.Add(_mapper.Map<Domain.Entities.Commission>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CommissionModel>(entity.Entity);
            }
            return null;
        }
    }
}
