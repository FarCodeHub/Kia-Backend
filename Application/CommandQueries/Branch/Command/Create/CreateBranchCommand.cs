using Application.CommandQueries.Branch.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using NetTopologySuite.Geometries;

namespace Application.CommandQueries.Branch.Command.Create
{
    public class CreateBranchCommand : CommandBase, IRequest<BranchModel>, IMapFrom<CreateBranchCommand>, ICommand
    {
        public string Title { get; set; }
        public string Address { get; set; } = default!;
        public double Lat { get; set; }
        public double Lng { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBranchCommand, Domain.Entities.Branch>()
                .IgnoreAllNonExisting()
                ;
        }
    }

    public class CreateBranchCommandHandler : IRequestHandler<CreateBranchCommand, BranchModel>
    {
        private readonly IMapper _mapper;
        private readonly IBranchService _branchService;
        private readonly IRepository _repository;

        public CreateBranchCommandHandler(IMapper mapper, IBranchService branchService, IRepository repository)
        {
            _mapper = mapper;
            _branchService = branchService;
            _repository = repository;
        }


        public async Task<BranchModel> Handle(CreateBranchCommand request, CancellationToken cancellationToken)
        {
            var branch = _mapper.Map<Domain.Entities.Branch>(request);
            branch.Location = new Point(request.Lng, request.Lat);

            var entity = await _branchService.Add(branch);

            try
            {
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return _mapper.Map<BranchModel>(entity.Entity);
                }
            }
            catch
            {
                return _mapper.Map<BranchModel>(entity.Entity);
            }

            return null;
        }
    }
}
