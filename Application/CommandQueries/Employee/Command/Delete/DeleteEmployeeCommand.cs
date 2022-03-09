using Application.CommandQueries.Employee.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Employee.Command.Delete
{
    public class DeleteEmployeeCommand : CommandBase, IRequest<EmployeeModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, EmployeeModel>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IRepository _repository;

        public DeleteEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService, IRepository repository)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _repository = repository;
        }

        public async Task<EmployeeModel> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _employeeService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<EmployeeModel>(entity.Entity);
            }

            return null;
        }
    }
}
