using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.AdvertismentSource.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.AdvertismentSource.Command.Delete
{
    public class DeleteAdvertisementSourceCommand : CommandBase, IRequest<AdvertisementSourceModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteAdvertisementSourceCommandHandler : IRequestHandler<DeleteAdvertisementSourceCommand, AdvertisementSourceModel>
    {
        private readonly IMapper _mapper;
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IRepository _repository;

        public DeleteAdvertisementSourceCommandHandler(IMapper mapper, IAdvertisementService advertisementSourceService, IRepository repository)
        {
            _mapper = mapper;
            _advertisementSourceService = advertisementSourceService;
            _repository = repository;
        }

        public async Task<AdvertisementSourceModel> Handle(DeleteAdvertisementSourceCommand request, CancellationToken cancellationToken)
        {
            var entity = await _advertisementSourceService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<AdvertisementSourceModel>(entity.Entity);
            }
            return null;
        }
    }
}
