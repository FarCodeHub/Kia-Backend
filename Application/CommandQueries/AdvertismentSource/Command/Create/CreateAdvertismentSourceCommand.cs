using System;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.AdvertismentSource.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.AdvertismentSource.Command.Create
{
    public class CreateAdvertisementSourceCommand : CommandBase, IRequest<AdvertisementSourceModel>, IMapFrom<CreateAdvertisementSourceCommand>, ICommand
    {
        public string HostName { get; set; }
        public string Title { get; set; }
        public int AdvertisementTypeBaseId { get; set; }
        public int AdvertisementSourceBaseId { get; set; }
        public int Reputation { get; set; }
        public int HeadLineNumberBaseId { get; set; }
        public int FeedbackNumber { get; set; }
        public int Expense { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string Descriptions { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateAdvertisementSourceCommand, Domain.Entities.Advertisement>()
                ;

        }
    }

    public class CreateAdvertisementSourceCommandHandler : IRequestHandler<CreateAdvertisementSourceCommand, AdvertisementSourceModel>
    {
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IRepository _repository;

        public CreateAdvertisementSourceCommandHandler(IMapper mapper, IAdvertisementService advertisementSourceService, IRepository repository)
        {
            _mapper = mapper;
            _advertisementSourceService = advertisementSourceService;
            _repository = repository;
        }


        public async Task<AdvertisementSourceModel> Handle(CreateAdvertisementSourceCommand request, CancellationToken cancellationToken)
        {
            var advertisment = _mapper.Map<Domain.Entities.Advertisement>(request);
            var entity = await _advertisementSourceService.Add(advertisment);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<AdvertisementSourceModel>(entity.Entity);
            }
            return null;
        }
    }
}
