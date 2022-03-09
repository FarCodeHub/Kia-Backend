using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Sms.Model
{
    public class SmsModel : IMapFrom<Domain.Entities.OutgoingAdvertismentMessage>
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.OutgoingAdvertismentMessage, SmsModel>();
          
        }
    }
}
