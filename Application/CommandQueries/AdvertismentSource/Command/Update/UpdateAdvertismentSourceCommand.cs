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

namespace Application.CommandQueries.AdvertismentSource.Command.Update
{
    public class UpdateAdvertisementSourceCommand : CommandBase, IRequest<AdvertisementSourceModel>, IMapFrom<Domain.Entities.Advertisement>, ICommand
    {
        public int Id { get; set; }
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
            profile.CreateMap<UpdateAdvertisementSourceCommand, Domain.Entities.Advertisement>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateAdvertisementSourceCommandHandler : IRequestHandler<UpdateAdvertisementSourceCommand, AdvertisementSourceModel>
    {
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IRepository _repository;

        public UpdateAdvertisementSourceCommandHandler(IMapper mapper, IAdvertisementService advertisementSourceService, IRepository repository)
        {
            _mapper = mapper;
            _advertisementSourceService = advertisementSourceService;
            _repository = repository;
        }

        public async Task<AdvertisementSourceModel> Handle(UpdateAdvertisementSourceCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _advertisementSourceService.Update(_mapper.Map<Domain.Entities.Advertisement>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<AdvertisementSourceModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
