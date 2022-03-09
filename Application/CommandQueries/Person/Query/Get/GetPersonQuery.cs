using Application.CommandQueries.Person.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Person.Query.Get
{
    public class GetPersonQuery : IRequest<PersonModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetPersonQueryHandler : IRequestHandler<GetPersonQuery, PersonModel>
    {
        private readonly IPersonService _personService;
        private readonly IMapper _mapper;

        public GetPersonQueryHandler(IMapper mapper, IPersonService personService)
        {
            _mapper = mapper;
            _personService = personService;
        }

        public async Task<PersonModel> Handle(GetPersonQuery request, CancellationToken cancellationToken)
        {
            var entity = _personService.FindById(request.Id);

            return await entity
                .ProjectTo<PersonModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
