using System;
using System.Linq;
using Application.CommandQueries.Employee.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.Person.Model;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.CommandQueries.Employee.Command.Create
{
    public class CreateEmployeeCommand : CommandBase, IRequest<int>, IRequest<PersonModel>, IMapFrom<CreateEmployeeCommand>, ICommand
    {
        public int? UnitPositionId { get; set; }
        public int PersonId { get; set; }
        public DateTime EmploymentDate { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? FatherName { get; set; }
        public string? NationalCode { get; set; }
        public string? IdentityCode { get; set; }
        public int? BirthPlaceId { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? GenderBaseId { get; set; }
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
        public int? ZipCodeId { get; set; }
        public string? Address { get; set; }
        public string ProfilePhotoUrl { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public string ExtentionNumber { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateEmployeeCommand, Domain.Entities.Employee>()
                .IgnoreAllNonExisting();

            profile.CreateMap<CreateEmployeeCommand, Domain.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, int>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IPersonService _personService;
        private readonly IOperatorService _operatorService;
        private readonly IRepository _repository;
        public CreateEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService, IRepository repository, IPersonService personService, IOperatorService operatorService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _repository = repository;
            _personService = personService;
            _operatorService = operatorService;
        }


        public async Task<int> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var inputeEmployee = _mapper.Map<Domain.Entities.Employee>(request);
            var inputePerson = _mapper.Map<Domain.Entities.Person>(request);
            inputeEmployee.EmployeeCode = "0";

            if (request.PersonId != 0)
            {
                if (await _personService.FindById(request.PersonId).AnyAsync(cancellationToken))
                {
                    inputeEmployee.PersonId = request.PersonId;
                }
            }
            else
            {
                var person = await _personService.Add(inputePerson);
                inputeEmployee.Person = person.Entity;
            }

            var employee = await _employeeService.Add(inputeEmployee);

            if (!string.IsNullOrEmpty(request.ExtentionNumber))
            {
                if (!string.IsNullOrEmpty(request.ExtentionNumber) && _repository.GetQuery<Domain.Entities.Operator>()
                    .Any(t => t.IsActive && t.ExtentionNumber == request.ExtentionNumber))
                {
                    throw new Exception("extention is in use");
                }

                var opt = await _operatorService.Add(new Domain.Entities.Operator()
                {
                    Employee = employee.Entity,
                    ExtentionNumber = request.ExtentionNumber,
                    QueueNumber = $"1{request.ExtentionNumber}"
                });
            }
           


            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return employee.Entity.Id;
            }
            return 0;
        }
    }
}
