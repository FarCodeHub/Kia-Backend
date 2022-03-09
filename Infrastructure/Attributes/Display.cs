using System;

namespace Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class Display : Attribute
    {
        private readonly string _title;
        public Display(string title)
        {
            _title = title;
        }

        public override string ToString()
        {
            return _title;
        }
    }
}