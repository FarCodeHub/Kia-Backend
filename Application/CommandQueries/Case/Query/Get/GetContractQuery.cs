using Application.CommandQueries.Case.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Case.Query.Get
{
    public class GetCaseQuery : IRequest<CaseModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetCaseQueryHandler : IRequestHandler<GetCaseQuery, CaseModel>
    {
        private readonly ICaseService _contractService;
        private readonly IMapper _mapper;

        public GetCaseQueryHandler(IMapper mapper, ICaseService contractService)
        {
            _mapper = mapper;
            _contractService = contractService;
        }

        public async Task<CaseModel> Handle(GetCaseQuery request, CancellationToken cancellationToken)
        {
            return await _contractService.FindById(request.Id)
                .ProjectTo<CaseModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
