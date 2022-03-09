using System.Collections.Generic;
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

namespace Application.CommandQueries.Project.Command.Create
{
    public class CreateProjectCommand : CommandBase, IRequest<ProjectModel>, IMapFrom<CreateProjectCommand>, ICommand
    {
        public string Title { get; set; }
        public int? ParentId { get; set; }
        public string? FilePath { get; set; }
        public int PriorityBaseId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Address { get; set; }
        public ICollection<CreateProjectCommand> Children { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateProjectCommand, Domain.Entities.Project>()
                .ForMember(x=>x.InverseParent,opt=>opt.MapFrom(x=>x.Children))
               ;
        }
    }

    public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, ProjectModel>
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IRepository _repository;
        public CreateProjectCommandHandler(IMapper mapper, IProjectService projectService, IRepository repository)
        {
            _mapper = mapper;
            _projectService = projectService;
            _repository = repository;
        }


        public async Task<ProjectModel> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var project = _mapper.Map<Domain.Entities.Project>(request);
            project.Location = new Point(request.Lng, request.Lat);
            var entity = await _projectService.Add(project,request.FilePath);


            try
            {
                if (await _repository.SaveChangesAsync(cancellationToken) > 0)
                {
                    return _mapper.Map<ProjectModel>(entity.Entity);
                }
            }
            catch
            {
                return _mapper.Map<ProjectModel>(entity.Entity);
            }

            return null;
        }
    }
}
