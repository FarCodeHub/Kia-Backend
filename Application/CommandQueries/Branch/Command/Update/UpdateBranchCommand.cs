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


namespace Application.CommandQueries.Branch.Command.Update
{
    public class UpdateBranchCommand : CommandBase, IRequest<BranchModel>, IMapFrom<Domain.Entities.Branch>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; } = default!;
        public double Lat { get; set; }
        public double Lng { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBranchCommand, Domain.Entities.Branch>()
                .IgnoreAllNonExisting()
                .ForMember(x => x.Location, opt =>
                    opt.MapFrom(x => new Point(x.Lng, x.Lat)));
        }
    }


    public class UpdateBranchCommandHandler : IRequestHandler<UpdateBranchCommand, BranchModel>
    {
        private readonly IMapper _mapper;
        private readonly IBranchService _branchService;
        private readonly IRepository _repository;

        public UpdateBranchCommandHandler(IMapper mapper, IBranchService branchService, IRepository repository)
        {
            _mapper = mapper;
            _branchService = branchService;
            _repository = repository;
        }

        public async Task<BranchModel> Handle(UpdateBranchCommand request, CancellationToken cancellationToken)
        {
            var entity = _mapper.Map<Domain.Entities.Branch>(request);

            entity.Location = new Point(request.Lng, request.Lat);

            var updatedEntity = await _branchService.Update(entity, cancellationToken);
            try
            {
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return _mapper.Map<BranchModel>(updatedEntity.Entity);
                }
            }
            catch
            {
                return _mapper.Map<BranchModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
