using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.CommandQueries.Person.Query.NationalCodeIsExist
{
    public class NationalCodeIdExistQuery : IRequest<bool>, IQuery
    {
        public string NationalCode
        {
            get;
            set;
        }

        public class NationalCodeIdExistQueryHandler : IRequestHandler<NationalCodeIdExistQuery, bool>
        {
            private readonly IRepository _repository;
            private readonly IMapper _mapper;

            public NationalCodeIdExistQueryHandler(IRepository repository, IMapper mapper)
            {
                _mapper = mapper;
                _repository = repository;
            }

            public async Task<bool> Handle(NationalCodeIdExistQuery request, CancellationToken cancellationToken)
            {
                return await _repository
                    .Exist<Domain.Entities.Person>(c
                        => c.ConditionExpression(x => x.NationalCode == request.NationalCode));

            }
        }
    }
}