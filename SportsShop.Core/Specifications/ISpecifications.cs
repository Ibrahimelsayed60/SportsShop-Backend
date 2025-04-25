using SportsShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Specifications
{
    public interface ISpecifications<T> 
    {
        public Expression<Func<T, bool>> Criteria { get; set; }

        public List<Expression<Func<T, object>>> Includes { get; set; }

        public Expression<Func<T, object>> OrderBy { get; set; }

        public Expression<Func<T, object>> OrderByDesc { get; set; }

        public bool IsDistinct { get; }

        public int Skip { get; set; }

        public int Take { get; set; }

        public bool IsPaginationEnabled { get; set; }

    }
}
