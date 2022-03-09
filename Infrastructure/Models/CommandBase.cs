
using Infrastructure.Interfaces;

namespace Infrastructure.Models
{
    public abstract class CommandBase : ICommand
    {
        public bool SaveChanges { get; set; } = true;
    }
}
