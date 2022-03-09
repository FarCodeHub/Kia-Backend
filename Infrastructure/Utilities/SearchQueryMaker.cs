using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Infrastructure.Models;
using System.Linq.Dynamic.Core;

namespace Infrastructure.Utilities
{
    public static class SearchQueryMaker
    {
        public static IQueryable<TEntity> MakeStringSearchQuery<TEntity>(this IQueryable<TEntity> query, IEnumerable<Condition> conditions)
        {
            try
            {
                if (conditions is null)
                {
                    return query;
                }

                foreach (var condition in conditions)
                {

                    if (condition.Comparison == "between")
                    {
                        query = query.Where($"{condition.PropertyName} >= @0 && {condition.PropertyName} <= @1",
                            condition.Values);
                    }
                    else if (condition.Comparison == "in")
                    {
                        if (condition.Values is null || condition.Values.Length == 0)
                        {
                            throw new Exception("invalid condition");
                        }

                        var or = condition.Values
                            .Aggregate("",
                                (current, conditionValue) =>
                                    current + $" {condition.PropertyName} == {conditionValue} ||");

                        if (or?.EndsWith("||") ?? false)
                        {
                            or = or.Remove(or.LastIndexOf('|') - 1, 2);
                        }

                        if (string.IsNullOrEmpty(or) || string.IsNullOrWhiteSpace(or))
                        {
                            continue;
                        }

                        query = query.Where(or);
                    }
                    else
                    {
                        query = query.Where($"{condition.PropertyName} {condition.Comparison}(@0)",
                            condition.Values);
                    }
                }

                return query;
            }
            catch
            {
                throw new Exception("invalid condition");
            }
        }

        public static Expression<Func<TEntity, bool>> MakeSearchQuery<TEntity>(List<SearchQueryItem> queries)
        {
            Expression<Func<TEntity, bool>> exp = null;
            foreach (var query in queries)
            {
                Expression<Func<TEntity, bool>> partExp = null;
                foreach (var queryPart in query.Parts)
                {
                    foreach (var condition in queryPart.Value)
                    {
                        if (partExp == null)
                        {
                            //partExp = ExpressionUtils.BuildPredicate<TEntity>(condition.PropertyName,
                            //    condition.Comparison, condition.Value);
                        }
                        else
                        {

                            //partExp = ExpressionUtils.BuildPredicate<TEntity>(partExp,
                            //    condition.PropertyName, condition.Comparison, condition.Value, condition.Operand);
                        }
                    }

                    partExp = ExpressionUtils.BuildPredicate<TEntity>(partExp, queryPart.Key);
                }

                if (exp == null)
                {
                    exp = partExp;
                }
                else
                {
                    exp.BuildPredicate<TEntity>(partExp, query.NextOperand);
                }
            }

            return exp;
        }
    }
}