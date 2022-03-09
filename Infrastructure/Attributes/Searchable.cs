using System;
using System.Linq;

namespace Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class Searchable : Attribute
    {
        public string TableName { get; set; }
        public string Schema { get; set; }
        public SearchTypes[] _searchType;
        public enum SearchTypes
        {
            Equal,
            Between,
            NotEqual,
            GreaterThan,
            GreaterThanOrEqual,
            LessThan,
            LessThanOrEqual,
            Contains,
            StartsWith,
            EndsWith
        }
        public Searchable(string tabelName,string schema,SearchTypes[] searchType)
        {
            TableName = tabelName;
            Schema = schema;
            _searchType = searchType;
        }

        public override string ToString()
        {
            var res = _searchType.Aggregate(string.Empty, (current, v) => current + (v + ","));

            if (res.EndsWith(','))
            {
                res = res.Remove(res.Length - 1, 1);
            }
            return res;
        }
    }
}