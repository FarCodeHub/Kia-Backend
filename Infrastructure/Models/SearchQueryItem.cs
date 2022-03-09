using System.Collections.Generic;

namespace Infrastructure.Models
{
    public class SearchQueryItem
    {
        public Dictionary<string,List<Condition>> Parts { get; set; }
        public string NextOperand { get; set; }
    }

    public class Condition
    {
        public string PropertyName { get; set; }
        public string Comparison
        {
            get => _comparison ?? "=";
            set
            {
                switch (value)
                {
                    case "equal":
                    case "=":
                        _comparison = "=";
                        break;
                    case "notEqual":
                    case "!=":
                        _comparison = "!=";
                        break;
                    case "contains":
                        _comparison = ".Contains";
                        break;
                    case "in":
                        _comparison = "in";
                        break;
                    case "between":
                        _comparison = "between";
                        break;
                    case "greatherThan":
                    case ">=":
                        _comparison = ">=";
                        break;
                    case "lessThan":
                    case "<=":
                        _comparison = "<=";
                        break;
                    default:
                        _comparison = "=";
                        break;
                }
            }
        }

        private string _comparison { get; set; }


        public object[] Values { get; set; }

    }
}