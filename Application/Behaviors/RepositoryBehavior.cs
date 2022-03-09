using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using MediatR;

namespace Application.Behaviors
{
    public class RepositoryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IRepository _repository;

        public RepositoryBehavior(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = await next();
            return response;
        }
    }
}
