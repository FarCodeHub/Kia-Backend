using System.Collections.Generic;
using Application.CommandQueries.Contract.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Commission.Command.Create;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Service.Services.BaseValue;

namespace Application.CommandQueries.Contract.Command.Create
{
    public class CreateContractCommand : CommandBase, IRequest<ContractModel>, IMapFrom<CreateContractCommand>, ICommand
    {
        public int TaskId { get; set; } = default!;
        public string Descriptions { get; set; }
        public int CaseId { get; set; }
        public string[] AttachmentsUrl { get; set; }
        public ICollection<int> ProjectsId { get; set; }
        public IDictionary<int,long> InvolvedEmployeesCommision { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateContractCommand, Domain.Entities.Contract>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateContractCommandHandler : IRequestHandler<CreateContractCommand, ContractModel>
    {
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly IRepository _repository;
        private readonly ICommissionService _commissionService;
        private readonly ICaseService _caseService;
        private readonly TaskResultsAndTypesSingleton _taskResultsAndTypesSingleton;
        public CreateContractCommandHandler(IMapper mapper, IContractService contractService, IRepository repository, ICommissionService commissionService, ICaseService caseService)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
            _commissionService = commissionService;
            _caseService = caseService;
            _taskResultsAndTypesSingleton = TaskResultsAndTypesSingleton.Instance(_repository);
        }


        public async Task<ContractModel> Handle(CreateContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.Add(_mapper.Map<Domain.Entities.Contract>(request), request.AttachmentsUrl);


            foreach (var i in request.InvolvedEmployeesCommision)
            {
                await _commissionService.Add(new Domain.Entities.Commission()
                { EmployeeId = i.Key, Contract = entity.Entity, IsPaid = false, Amount = i.Value });
            }

            foreach (var i in request.ProjectsId)
            {
                _repository.Insert(new ContractProject()
                {
                    Contract = entity.Entity,
                    ProjectId = i
                });
            }

            await _caseService.Close(request.CaseId, _taskResultsAndTypesSingleton.ApplyToContract, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<ContractModel>(entity.Entity);
            }
            return null;
        }
    }
}