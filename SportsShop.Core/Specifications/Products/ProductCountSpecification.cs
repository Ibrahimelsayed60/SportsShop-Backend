using SportsShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Specifications.Products
{
    public class ProductCountSpecification : BaseSpecification<Product>
    {

        public ProductCountSpecification(ProductSpecParams productSpecParams):base(x =>
            (string.IsNullOrEmpty(productSpecParams.Search) || x.Name.ToLower().Contains(productSpecParams.Search)) &&
            (productSpecParams.Brands.Count == 0 || productSpecParams.Brands.Contains(x.Brand)) &&
            (productSpecParams.Types.Count == 0 || productSpecParams.Types.Contains(x.Type))
        )
        {
            
        }

    }
}
