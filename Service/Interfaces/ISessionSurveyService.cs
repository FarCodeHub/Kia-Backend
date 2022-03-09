using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface ISessionSurveyService : ICrudService<Domain.Entities.SessionSurvey>, ISearchService<Domain.Entities.SessionSurvey>
    {

    }
}