using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Interfaces;
using Infrastructure.Resources;
using Message = Infrastructure.Models.Message;

namespace Infrastructure.Exceptions
{
    public class HandledErrorManager : IHandledErrorManager
    {
        private readonly IValidationFactory _validationFactory;

        public HandledErrorManager(IValidationFactory validationFactory)
        {
            _validationFactory = validationFactory;
        }

        public Exception Throw<T>(List<string> value = null) where T : System.Exception, IHandledException
        {
            var message = ValidationMessageBuilder(new Message()
            {
                Key = typeof(T).Name,
                Values = value
            });

            var someResult = (T)Activator.CreateInstance(
                typeof(T)
                , new object[] { message }
            );
             throw someResult ?? throw new Exception(message);
        }


        public string ValidationMessageBuilder(Message message)
        {
            var validationMessage = _validationFactory.Get(message.Key);
            if (string.IsNullOrEmpty(validationMessage))
            {
                return message.Key;
            }
            var messageBuilder = new StringBuilder();
            var i = 1;
            foreach (var s in validationMessage.Split(' '))
            {
                if (s == $"[{i}]")
                {
                    messageBuilder.Append(message.Values[i - 1]);
                    i++;
                }
                else
                {
                    messageBuilder.Append(s);
                }

                messageBuilder.Append(" ");
            }

            return messageBuilder.ToString();
        }
    }
}