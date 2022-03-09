using Application.CommandQueries.Operator.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Operator.Query.Get
{
    public class GetOperatorQuery : IRequest<OperatorModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetOperatorQueryHandler : IRequestHandler<GetOperatorQuery, OperatorModel>
    {
        private readonly IOperatorService _operatorService;
        private readonly IMapper _mapper;

        public GetOperatorQueryHandler(IMapper mapper, IOperatorService operatorService)
        {
            _mapper = mapper;
            _operatorService = operatorService;
        }

        public async Task<OperatorModel> Handle(GetOperatorQuery request, CancellationToken cancellationToken)
        {
            var entity = _operatorService.FindById(request.Id);

            return await entity
                .ProjectTo<OperatorModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
