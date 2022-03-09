using Application.CommandQueries.BaseValueType.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.BaseValueType.Query.Get
{
    public class GetBaseValueTypeQuery : IRequest<BaseValueTypeModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetBaseValueTypeQueryHandler : IRequestHandler<GetBaseValueTypeQuery, BaseValueTypeModel>
    {
        private readonly IBaseValueTypeService _baseValueTypeService;
        private readonly IMapper _mapper;

        public GetBaseValueTypeQueryHandler(IMapper mapper, IBaseValueTypeService baseValueTypeService)
        {
            _mapper = mapper;
            _baseValueTypeService = baseValueTypeService;
        }

        public async Task<BaseValueTypeModel> Handle(GetBaseValueTypeQuery request, CancellationToken cancellationToken)
        {
            var entity = _baseValueTypeService.FindById(request.Id);

            return await entity
                .ProjectTo<BaseValueTypeModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
