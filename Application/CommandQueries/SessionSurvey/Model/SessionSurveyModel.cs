using AutoMapper;
using Infrastructure.Mappings;

namespace Application.CommandQueries.SessionSurvey.Model
{
    public class SessionSurveyModel : IMapFrom<Domain.Entities.SessionSurvey>
    {
        public int Id { get; set; }
        public int TaskId { get; set; } = default!;
        public int QuestionId { get; set; } = default!;
        public int AnswerId { get; set; } = default!;
        public string? Answer { get; set; }
        public string AnswerBaseTitle { get; set; }
        public string QuestionTitle { get; set; }
        public string QuestionTypeBaseTitle { get; set; }
        public int QuestionTypeId { get; set; }
        public int CaseId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Entities.SessionSurvey, SessionSurveyModel>()
                .ForMember(x=>x.AnswerBaseTitle,opt=>opt.MapFrom(x=>x.AnswerNavigation.Title))
                .ForMember(x=>x.CaseId,opt=>opt.MapFrom(x=>x.Task.CaseId))
                .ForMember(x=>x.QuestionTypeBaseTitle,opt=>opt.MapFrom(x=>x.Question.QuestionTypBase.Title))
                .ForMember(x=>x.QuestionTypeId,opt=>opt.MapFrom(x=>x.Question.QuestionTypBaseId))
                .ForMember(x=>x.QuestionTitle,opt=>opt.MapFrom(x=>x.Question.Title));
        }
    }
}
