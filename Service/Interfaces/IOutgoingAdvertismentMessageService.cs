using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IOutgoingAdvertismentMessageService : ICrudService<Domain.Entities.OutgoingAdvertismentMessage>, ISearchService<Domain.Entities.OutgoingAdvertismentMessage>
    {

    }
}