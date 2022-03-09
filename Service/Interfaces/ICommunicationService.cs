using Domain.Entities;
using Infrastructure.Interfaces;

namespace Service.Interfaces
{
    public interface ICommunicationService : ICrudService<Communication>
    {
        IRepository Repository { get; set; }

    }
}