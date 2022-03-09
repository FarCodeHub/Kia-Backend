using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Task.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.Task.Query.Get
{
    public class GetTaskQuery : IRequest<TaskModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetTaskQueryHandler : IRequestHandler<GetTaskQuery, TaskModel>
    {
        private readonly ITaskService _sessionService;
        private readonly IMapper _mapper;

        public GetTaskQueryHandler(IMapper mapper, ITaskService sessionService)
        {
            _mapper = mapper;
            _sessionService = sessionService;
        }

        public async Task<TaskModel> Handle(GetTaskQuery request, CancellationToken cancellationToken)
        {
            var entity = _sessionService.FindById(request.Id);

            return await entity
                .ProjectTo<TaskModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
