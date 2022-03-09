using Application.CommandQueries.Employee.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Employee.Query.Get
{
    public class GetEmployeeQuery : IRequest<EmployeeModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetEmployeeQueryHandler : IRequestHandler<GetEmployeeQuery, EmployeeModel>
    {
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;

        public GetEmployeeQueryHandler(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<EmployeeModel> Handle(GetEmployeeQuery request, CancellationToken cancellationToken)
        {
            var entity = _employeeService.FindById(request.Id);

            return await entity
                .ProjectTo<EmployeeModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        }
    }
}
