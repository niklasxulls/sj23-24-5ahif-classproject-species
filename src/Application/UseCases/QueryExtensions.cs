using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DiveSpecies.Domain.Entities;

namespace DiveSpecies.Application.UseCases;
public static class QueryExtensions
{
    public static IQueryable<T> FilterNumber<T>(this IQueryable<T> query, Expression<Func<T, int>> numberExp, int? than, NumberParameterOperator? use)
    {
        if (than.GetValueOrDefault() < 1 || use == null) return query;

        // Parameter expression for the lambda parameter of the numberExp
        var parameter = numberExp.Parameters[0];

        // Build the comparison expression based on the specified use
        Expression comparisonExpression;
        switch (use)
        {
            case NumberParameterOperator.GreaterThanOrEqual:
                comparisonExpression = Expression.GreaterThanOrEqual(numberExp.Body, Expression.Constant(than.Value));
                break;
            case NumberParameterOperator.LessThanOrEqual:
                comparisonExpression = Expression.LessThanOrEqual(numberExp.Body, Expression.Constant(than.Value));
                break;
            default:
                throw new ArgumentException("Invalid NumberParameterUse value", nameof(use));
        }

        // Combine the comparison expression with the original query using 'Where' method
        var lambda = Expression.Lambda<Func<T, bool>>(comparisonExpression, parameter);
        return query.Where(lambda);

    }
}
