using System;
using System.Linq;
using Application.CommandQueries.Employee.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;


namespace Application.CommandQueries.Employee.Command.Update
{
    public class UpdateEmployeeCommand : CommandBase, IRequest<EmployeeModel>, IMapFrom<UpdateEmployeeCommand>, ICommand
    {
        public int Id { get; set; }
        public int? UnitPositionId { get; set; }
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
        public bool IsOperator { get; set; }
        public string ExtentionNumber { get; set; } = default!;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateEmployeeCommand, Domain.Entities.Employee>()
                .IgnoreAllNonExisting();

            profile.CreateMap<UpdateEmployeeCommand, Domain.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeModel>
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IRepository _repository;
        private readonly IOperatorService _operatorService;
        public UpdateEmployeeCommandHandler(IMapper mapper, IEmployeeService employeeService, IRepository repository, IOperatorService operatorService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _repository = repository;
            _operatorService = operatorService;
        }

        public async Task<EmployeeModel> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _employeeService.Update(_mapper.Map<Domain.Entities.Employee>(request),
                _mapper.Map<Domain.Entities.Person>(request),request.ProfilePhotoUrl, cancellationToken);

            if (request.IsOperator != true)
            {
                var opt = await _operatorService.GetAll().FirstOrDefaultAsync(x => x.EmployeeId == request.Id, cancellationToken: cancellationToken);
                opt.IsActive = false;
                await _operatorService.Update(opt, cancellationToken);
            }

            if (!string.IsNullOrEmpty(request.ExtentionNumber))
            {
                var currentOpt = await _operatorService.GetAll().FirstOrDefaultAsync(x => x.EmployeeId == request.Id,
                    cancellationToken: cancellationToken);

                if (currentOpt != null)
                {
                    if (request.ExtentionNumber != currentOpt.ExtentionNumber)
                    {
                        if (_repository
                            .GetQuery<Domain.Entities.Operator>()
                            .Any(t => t.IsActive && t.ExtentionNumber == request.ExtentionNumber))
                        {
                            throw new Exception("extention is in use");
                        }

                        var opt = await _operatorService.Add(new Domain.Entities.Operator()
                        {
                            EmployeeId = request.Id,
                            ExtentionNumber = request.ExtentionNumber,
                            QueueNumber = $"1{request.ExtentionNumber}"
                        });
                    }
                }
            }

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<EmployeeModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
