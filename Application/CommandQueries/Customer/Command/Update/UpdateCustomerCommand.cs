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


namespace Application.CommandQueries.Customer.Command.Update
{
    public class UpdateCustomerCommand : CommandBase, IRequest<CustomerModel>, IMapFrom<UpdateCustomerCommand>, ICommand
    {
        public int Id { get; set; }
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
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? Phone3 { get; set; }
        public bool FullUpdate { get; set; } = false;
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateCustomerCommand, Domain.Entities.Customer>()
                .IgnoreAllNonExisting();

            profile.CreateMap<UpdateCustomerCommand, Domain.Entities.Person>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerModel>
    {
        private readonly IMapper _mapper;
        private readonly ICustomerService _customerService;
        private readonly IRepository _repository;

        public UpdateCustomerCommandHandler(IMapper mapper, ICustomerService customerService, IRepository repository)
        {
            _mapper = mapper;
            _customerService = customerService;
            _repository = repository;
        }

        public async Task<CustomerModel> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _customerService.Update(_mapper.Map<Domain.Entities.Customer>(request),_mapper.Map<Domain.Entities.Person>(request),request.FullUpdate, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<CustomerModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
