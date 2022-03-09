using System;
using Application.CommandQueries.Customer.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Customer.Command.Create
{
    public class CreateCustomerCommand : CommandBase, IRequest<CustomerModel>, IMapFrom<CreateCustomerCommand>, ICommand
    {
        public int PersonId { get; set; } = default!;
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
        public string Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public int ConsultantId { get; set; }
        public int PresentorId { get; set; }
        public int StatusId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateCustomerCommand, Domain.Entities.Customer>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IRepository _repository;
        private readonly ICaseService _caseService;
        public CreateCustomerCommandHandler(IMapper mapper, ICustomerService customerService, IRepository repository, ICaseService caseService)
        {
            _mapper = mapper;
            _customerService = customerService;
            _repository = repository;
            _caseService = caseService;
        }


        public async Task<CustomerModel> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _customerService.Add(_mapper.Map<Domain.Entities.Customer>(request), _mapper.Map<Domain.Entities.Person>(request));

            await _caseService.Add(new Domain.Entities.Case()
            {
                Customer = entity.Entity, StatusBaseId = request.StatusId, CustomerId = request.ConsultantId,
                PresentorId = request.PresentorId
            });

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CustomerModel>(entity.Entity);
            }
            return null;
        }
    }
}
