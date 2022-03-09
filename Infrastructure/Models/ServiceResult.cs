using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class ServiceResult<T> 
    {
        public static ServiceResult<T> Set(T data, IDictionary<string, List<string>> exceptions = default) => new (false,data,exceptions);

        private T _objResult;
        public bool Succeed { get; set; } = false;
        public IDictionary<string, List<string>> Exceptions { get; set; }

        public T ObjResult
        {
            get => _objResult;
            set
            {
                if (value is not null) Succeed = true;
                _objResult = value;
            }
        }

        public ServiceResult(bool succeed,T data, IDictionary<string, List<string>> exceptions = null)
        {
            Succeed = succeed;
            ObjResult = data;
            Exceptions = exceptions;
        }

    }
}