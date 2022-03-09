using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using Infrastructure.CurrentUserAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Resources;
using MediatR;

namespace Application.Behaviors
{
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, ICommand
    {

        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly IRepository _repository;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IValidationFactory _validationFactory;
        private readonly IHandledErrorManager _handledErrorManager;
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators, IRepository repository, ICurrentUserAccessor currentUserAccessor, IValidationFactory validationFactory, IHandledErrorManager handledErrorManager)
        {
            _validators = validators;
            _repository = repository;
            _currentUserAccessor = currentUserAccessor;
            _validationFactory = validationFactory;
            _handledErrorManager = handledErrorManager;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);

                var failures = _validators
                    .Select(validator => validator.ValidateAsync(context, cancellationToken))
                    .SelectMany(validationResult => validationResult.GetAwaiter().GetResult().Errors)
                    .Where(validationFailure => validationFailure != null)
                    .ToList();

                foreach (var validationFailure in failures)
                {
                    var splitedMessage = validationFailure.ErrorMessage.Split(':');
                    var key = splitedMessage.FirstOrDefault();
                    string[] values = null;
                    if (splitedMessage.LastOrDefault() != splitedMessage.FirstOrDefault())
                    {
                        values = splitedMessage.LastOrDefault()?.Split(',') ?? null;
                    }

                    var message = new Message() { Key = key, Values = values?.ToList() };

                    validationFailure.ErrorMessage = _handledErrorManager.ValidationMessageBuilder(message);
                }

                if (failures.Count != 0)
                    throw new Infrastructure.Exceptions.ValidationException(failures);
            }
            return await next();
        }

    }
}