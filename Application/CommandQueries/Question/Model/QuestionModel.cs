using System.Collections.Generic;
using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Question.Model
{
    public class QuestionModel : IMapFrom<Domain.Entities.Question>
    {
        public int Id { get; set; }
        public int QuestionTypBaseId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int AnswerTypeBaseId { get; set; } = default!;
        public int? AnswerOptionBaseTypeId { get; set; }

        public string QuestionTypBaseTitle { get; set; } = default!;
        public string AnswerTypeBaseTitle { get; set; } = default!;
        public string QuestionTypBaseUniqueName { get; set; } = default!;
        public string AnswerTypeBaseUniqueName { get; set; } = default!;
        public ICollection<AnswerModel> AnswerOptionBases { get; set; }
        public int UsedInSurvey { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.Question, QuestionModel>()
                .ForMember(x => x.QuestionTypBaseTitle, opt => opt.MapFrom(x => x.QuestionTypBase.Title))
                .ForMember(x => x.AnswerTypeBaseTitle, opt => opt.MapFrom(x => x.AnswerTypeBase.Title))
                .ForMember(x => x.UsedInSurvey, opt => opt.MapFrom(x => x.SessionSurveys.Count))
                .ForMember(x => x.QuestionTypBaseUniqueName, opt => opt.MapFrom(x => x.QuestionTypBase.UniqueName))
                .ForMember(x => x.AnswerTypeBaseUniqueName, opt => opt.MapFrom(x => x.AnswerTypeBase.UniqueName))
                .ForMember(x=>x.AnswerOptionBases,opt=>opt.MapFrom(x=>x.AnswerOptionBaseType.BaseValues))
                ;
        }
    }
}
