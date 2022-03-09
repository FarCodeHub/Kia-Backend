using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IQuestionService : ICrudService<Domain.Entities.Question>, ISearchService<Domain.Entities.Question>
    {

    }
}