using SportsShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Specifications.Products
{
    public class ProductSpecification : BaseSpecification<Product>
    {

        public ProductSpecification(ProductSpecParams productSpecParams): base( x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search)) &&
            (productSpecParams.Brands.Count == 0 || productSpecParams.Brands.Contains(x.Brand)) &&
            (productSpecParams.Types.Count == 0 || productSpecParams.Types.Contains(x.Type))
            )
        {
            ApplyPagination(productSpecParams.PageSize * (productSpecParams.PageIndex - 1), productSpecParams.PageSize);

            switch (productSpecParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(x => x.Price);
                    break;
                case "priceDesc":
                    AddOrderByDesc(x => x.Price);
                    break;
                default:
                    AddOrderBy(x => x.Name);
                    break;
            }
        }

    }
}
