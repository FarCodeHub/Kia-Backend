using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface IAdvertisementService : ICrudService<Domain.Entities.Advertisement>, ISearchService<Domain.Entities.Advertisement>
    {
        IRepository Repository { get; set; }

        Task<bool> IsUsableFeedbackNumber(int feedbackNumber, DateTime startDateTime, DateTime endDateTime); 
        IQueryable<Domain.Entities.Advertisement> FindByFeedBackNumber(int feedbackNumber);

    }
}