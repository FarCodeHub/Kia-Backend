using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.Task.Model
{
    public class SessionSurveyModel : IMapFrom<Domain.Entities.SessionSurvey>,IMapFrom<SessionSurveyModel>
    {
        public int Id { get; set; }
        public int TaskId { get; set; } = default!;
        public int QuestionId { get; set; } = default!;
        public string QuestionTitle { get; set; }
        public int? AnswerId { get; set; } = default!;
        public string? AnswerTitle { get; set; } = default!;
        public string? Answer { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SessionSurvey, SessionSurveyModel>()
                .ForMember(x => x.AnswerTitle, opt => opt.MapFrom(x => x.AnswerNavigation.Title))
                .ForMember(x => x.QuestionTitle, opt => opt.MapFrom(x => x.Question.Title))
                ;
            profile.CreateMap<SessionSurveyModel, Domain.Entities.SessionSurvey>();
        }
    }
}
