using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Question.Model
{
    public class AnswerModel : IMapFrom<Domain.Entities.BaseValue>
    {
        public int Id { get; set; }
        public int BaseValueTypeId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string Value { get; set; }
        public int OrderIndex { get; set; }
        public string LevelCode { get; set; } = default!;
        public bool IsReadOnly { get; set; }

        public string BaseValueTypeTitle { get; set; }
        public string BaseValueTypeUniqueName { get; set; }
        public int AnswerdInQuestion { get; set; }
        public string ParentTitle { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.BaseValue, AnswerModel>()
                .ForMember(x => x.BaseValueTypeTitle, opt => opt.MapFrom(x => x.BaseValueType.Title))
                .ForMember(x => x.AnswerdInQuestion, opt => opt.MapFrom(x => x.SessionSurveys.Count))
                .ForMember(x => x.BaseValueTypeUniqueName, opt => opt.MapFrom(x => x.BaseValueType.UniqueName))
                .ForMember(x => x.ParentTitle, opt => opt.MapFrom(x => x.Parent.Title))
                ;
        }
    }
}
