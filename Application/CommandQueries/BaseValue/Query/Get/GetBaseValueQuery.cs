using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.BaseValue.Query.Get
{
    public class GetBaseValueQuery : IRequest<BaseValueModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBaseValueQueryHandler : IRequestHandler<GetBaseValueQuery, BaseValueModel>
    {
        private readonly IBaseValueService _baseValueService;
        private readonly IMapper _mapper;

        public GetBaseValueQueryHandler(IMapper mapper, IBaseValueService baseValueService)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
        }

        public async Task<BaseValueModel> Handle(GetBaseValueQuery request, CancellationToken cancellationToken)
        {
            var entity = _baseValueService.FindById(request.Id);

            return await entity
                .ProjectTo<BaseValueModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
