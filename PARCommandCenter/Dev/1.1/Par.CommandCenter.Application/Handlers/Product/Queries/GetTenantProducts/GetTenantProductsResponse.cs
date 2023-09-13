using System.Collections.Generic;

namespace Par.CommandCenter.Application.Handlers.Product.Queries.GetTenantProducts
{
    public class GetTenantProductsResponse
    {
        public IEnumerable<GetAllProductsDto> Products { get; set; }

        public int PageNumber
        {
            get;
            set;
        }

        public int TotalPages
        {
            get;
            set;
        }

        
        public int TotalCount
        {
            get;
            set;
        }
        
        public bool HasPreviousPage
        {
            get;
            set;
        }
        
        public bool HasNextPage
        {
            get;
            set;
        }
    }
}
