using System;
using System.Collections.Generic;
using System.Linq;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Infrastructure.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, List<string>> Failures { get; }

        public ValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, List<string>>();
        }

        public ValidationException(List<ValidationFailure> failures)
            : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures.ToList());
            }
        }

    
    }
}