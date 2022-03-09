using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Models
{
    public class StatisticModel
    {
        public string Label { get; set; }
        public double Value { get; set; }

        public static StatisticModel ToStatisticModel(string label, double value)
        {
            return new StatisticModel() { Label = label, Value = value };
        }


    }

    public static class StatisticModelExtention
    {
        public static ICollection<StatisticModel> ToCollectionStatisticModel<TSource>(this IEnumerable<TSource> source, Func<TSource, string> labelSelector, Func<TSource, double> valueSelector)
        {
            
            return source.Select(element => new StatisticModel() { Label = labelSelector(element), Value = valueSelector(element) }).ToList();
        }
    }
}