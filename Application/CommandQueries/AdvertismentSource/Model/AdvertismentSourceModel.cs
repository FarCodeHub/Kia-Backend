using System;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.AdvertismentSource.Model
{
    public class AdvertisementSourceModel : IMapFrom<Domain.Entities.Advertisement>
    {
        public int Id { get; set; }
        public string HostName { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int AdvertisementTypeBaseId { get; set; } = default!;
        public int AdvertisementSourceBaseId { get; set; } = default!;
        public int Reputation { get; set; } = default!;
        public int HeadLineNumberBaseId { get; set; } = default!;
        public int FeedbackNumber { get; set; } = default!;
        public int Expense { get; set; } = default!;
        public DateTime StartDateTime { get; set; } = default!;
        public DateTime EndDateTime { get; set; } = default!;
        public string? Descriptions { get; set; }


        public string AdvertisementSourceBaseTitle { get; set; } = default!;
        public string AdvertisementTypeBaseTitle { get; set; } = default!;
        public string HeadLineNumberBaseTitle { get; set; } = default!;
        public string HeadLineNumberBaseValue { get; set; } = default!;
        public virtual string CreatedByFullName { get; set; }
        public virtual string ModifiedByFullName { get; set; }
        public virtual string OwnerRoleTitle { get; set; }
        public int FeedbackCounter { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Advertisement, AdvertisementSourceModel>()
                .ForMember(x => x.AdvertisementSourceBaseTitle, opt => opt.MapFrom(x => x.AdvertisementSourceBase.Title))
                .ForMember(x => x.AdvertisementTypeBaseTitle, opt => opt.MapFrom(x => x.AdvertisementTypeBase.Title))
                .ForMember(x => x.HeadLineNumberBaseTitle, opt => opt.MapFrom(x => x.HeadLineNumberBase.Title))
                .ForMember(x => x.HeadLineNumberBaseValue, opt => opt.MapFrom(x => x.HeadLineNumberBase.Value))
                .ForMember(x => x.CreatedByFullName, opt => opt.MapFrom(x => x.CreatedBy.Person.FirstName + " " + x.CreatedBy.Person.LastName))
                .ForMember(x => x.ModifiedByFullName, opt => opt.MapFrom(x => x.ModifiedBy.Person.FirstName + " " + x.ModifiedBy.Person.LastName))
                .ForMember(x => x.FeedbackCounter, opt => opt.MapFrom(x => x.Communications.Count))
                .ForMember(x => x.OwnerRoleTitle, opt => opt.MapFrom(x => x.OwnerRole.Title))
                ;
        }
    }
}
