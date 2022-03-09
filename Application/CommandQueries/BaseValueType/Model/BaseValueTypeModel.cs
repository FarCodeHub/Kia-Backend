using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.BaseValueType.Model
{
    public class BaseValueTypeModel : IMapFrom<Domain.Entities.BaseValueType>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string LevelCode { get; set; } = default!;
        public int? ParentId { get; set; }
        public string GroupName { get; set; }
        public bool IsReadOnly { get; set; }
        public string SubSystem { get; set; }


        public string ParentTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.BaseValueType, BaseValueTypeModel>()
                .ForMember(x => x.ParentTitle, opt => opt.MapFrom(x => x.Parent.Title))




                ;
        }
    }
}
