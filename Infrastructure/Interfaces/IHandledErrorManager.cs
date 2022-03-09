using System;
using System.Collections.Generic;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Infrastructure.Exceptions
{
    public interface IHandledErrorManager : IService
    {
        public Exception Throw<T>(List<string> value = null) where T : Exception, IHandledException;
        public string ValidationMessageBuilder(Message message);

    }
}