using Application.CommandQueries.Project.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Project.Query.Get
{
    public class GetProjectQuery : IRequest<ProjectModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, ProjectModel>
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public GetProjectQueryHandler(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper;
            _projectService = projectService;
        }

        public async Task<ProjectModel> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var entity = _projectService.FindById(request.Id);

            return await entity
                .ProjectTo<ProjectModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
