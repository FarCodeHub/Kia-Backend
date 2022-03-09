using Application.CommandQueries.Project.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Project.Command.Delete
{
    public class DeleteProjectCommand : CommandBase, IRequest<ProjectModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, ProjectModel>
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IRepository _repository;

        public DeleteProjectCommandHandler(IMapper mapper, IProjectService projectService, IRepository repository)
        {
            _mapper = mapper;
            _projectService = projectService;
            _repository = repository;
        }

        public async Task<ProjectModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await _projectService.SoftDelete(request.Id, cancellationToken);
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
