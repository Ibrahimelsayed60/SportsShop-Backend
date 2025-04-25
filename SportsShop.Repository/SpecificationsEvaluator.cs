using SportsShop.Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SportsShop.Repository
{
    public class SpecificationsEvaluator<T> where T : class
    {

        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> spec)
        {
            var query = inputQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); // x => x.Brand == brand
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            if (spec.IsDistinct)
            {
                query = query.Distinct();
            }

            if (spec.IsPaginationEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));

            return query;
        }

        public static IQueryable<TResult> GetQuery<TSpec, TResult>(IQueryable<T> query,
        ISpecifications<T, TResult> spec)
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria); // x => x.Brand == brand
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDesc != null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }

            var selectQuery = query as IQueryable<TResult>;

            if (spec.Select != null)
            {
                selectQuery = query.Select(spec.Select);
            }

            if (spec.IsDistinct)
            {
                selectQuery = selectQuery?.Distinct();
            }

            if (spec.IsPaginationEnabled)
            {
                selectQuery = selectQuery?.Skip(spec.Skip).Take(spec.Take);
            }

            return selectQuery ?? query.Cast<TResult>();
        }

    }


}
