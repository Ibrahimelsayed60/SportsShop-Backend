using Mapster;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Service.Helpers
{
    public static class MapperHelper
    {

        public static IMapper Mapper { get; set; }

        public static IEnumerable<TResult> Map<TResult>(this IQueryable source)
        {
            return source.ProjectToType<TResult>();
        }

        public static TResult Mapone<TResult>(this object source)
        {
            return source.Adapt<TResult>();
        }



    }
}
