using Application.CommandQueries.Project.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using NetTopologySuite.Geometries;


namespace Application.CommandQueries.Project.Command.Update
{
    public class UpdateProjectCommand : CommandBase, IRequest<ProjectModel>, IMapFrom<Domain.Entities.Project>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public int PriorityBaseId { get; set; }
        public string Address { get; set; }
        public string? FilePath { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateProjectCommand, Domain.Entities.Project>()
                ;
        }
    }


    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ProjectModel>
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IRepository _repository;

        public UpdateProjectCommandHandler(IMapper mapper, IProjectService projectService, IRepository repository)
        {
            _mapper = mapper;
            _projectService = projectService;
            _repository = repository;
        }

        public async Task<ProjectModel> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Domain.Entities.Project>(request);
            project.Location = new Point(request.Lng, request.Lat);

            var updatedEntity = await _projectService.Update(project, request.FilePath, cancellationToken);

            try
            {
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return _mapper.Map<ProjectModel>(updatedEntity.Entity);
                }
            }
            catch
            {
                return _mapper.Map<ProjectModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
