using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DiveSpecies.Application.Extensions;
public static class SortQueryExtensions
{
    public static IOrderedQueryable<T> SortBy<T, TKey>(this IQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool desc = false)
    {
        return !desc ? query.OrderBy(keySelector) : query.OrderByDescending(keySelector);
    }

    public static IOrderedQueryable<T> ThenSortBy<T, TKey>(this IOrderedQueryable<T> query, Expression<Func<T, TKey>> keySelector, bool desc = false)
    {
        return !desc ? query.ThenBy(keySelector) : query.ThenByDescending(keySelector);
    }
}
