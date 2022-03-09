using System;

namespace Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HasUniqueIndex : Attribute
    {

    }
}