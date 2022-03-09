using System.Collections.Generic;
using System.Linq;
using Application.CommandQueries.Contract.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Contract.Command.Update
{
    public class UpdateContractCommand : CommandBase, IRequest<ContractModel>, IMapFrom<Domain.Entities.Contract>, ICommand
    {
        public int Id { get; set; }
        public string Descriptions { get; set; }
        public string[] AttachmentsUrl { get; set; }
        public ICollection<int> ProjectsId { get; set; }
        public ICollection<int> InvolvedEmployeesId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateContractCommand, Domain.Entities.Contract>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateContractCommandHandler : IRequestHandler<UpdateContractCommand, ContractModel>
    {
        private readonly IMapper _mapper;
        private readonly IContractService _contractService;
        private readonly IRepository _repository;
        private readonly IContractProjectService _contractProjectService;
        public UpdateContractCommandHandler(IMapper mapper, IContractService contractService, IRepository repository, IContractProjectService contractProjectService)
        {
            _mapper = mapper;
            _contractService = contractService;
            _repository = repository;
            _contractProjectService = contractProjectService;
        }

        public async Task<ContractModel> Handle(UpdateContractCommand request, CancellationToken cancellationToken)
        {
            var entity = await _contractService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            var updatedEntity = await _contractService.Update(entity,request.InvolvedEmployeesId, request.AttachmentsUrl, cancellationToken);

            foreach (var removedPermission in entity.ContractProjects.Select(x => x.ProjectId).Except(request.ProjectsId))
            {
                var deletingEntity = await _repository.GetQuery<ContractProject>()
                    .FirstOrDefaultAsync(x => x.ProjectId == removedPermission && x.ContractId == entity.Id,cancellationToken);
              
                await _contractProjectService.SoftDelete(deletingEntity.Id, cancellationToken);
            }

            foreach (var addedPermission in request.ProjectsId.Except(entity.ContractProjects.Select(x => x.ProjectId)))
            {
                _repository.Insert(new ContractProject()
                {
                    ContractId = entity.Id,
                    ProjectId = addedPermission
                });
            }


            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<ContractModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
