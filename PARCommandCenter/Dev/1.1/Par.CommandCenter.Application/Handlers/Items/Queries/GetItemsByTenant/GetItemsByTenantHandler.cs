using MediatR;
using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Items.Queries.GetItemsByTenant
{
    public class GetItemsByTenantHandler : IRequestHandler<GetItemsByTenantQuery, GetItemsByTenantResponse>
    {
        private readonly IApplicationDbContext _dbContext;

        public GetItemsByTenantHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetItemsByTenantResponse> Handle(GetItemsByTenantQuery request, CancellationToken cancellationToken)
        {
            var query = from li in _dbContext.LocationItems
                        join i in _dbContext.Items on li.ItemId equals i.Id
                        join iu in _dbContext.ItemUnits on i.IssueItemUnitId equals iu.Id
                        join t in _dbContext.Tenants on li.TenantId equals t.Id
                        join l in _dbContext.Locations on li.LocationId equals l.Id
                        join f in _dbContext.Facilities on l.FacilityId equals f.Id
                        join itt in _dbContext.InventoryTrackingTypes on li.InventoryTrackingTypeId equals itt.Id
                        join b in _dbContext.Bins on li.Id equals b.LocationItemId into bx
                        from b in bx.DefaultIfEmpty()
                        join s in _dbContext.Scales on b.Id equals s.BinId
                        where !b.Deleted && !s.Deleted
                        select new
                        {
                            i.Id,
                            ItemName = i.Name,
                            ItemNumber = i.Number,
                            ScaleAddress = s.Address,
                            FacilityName = f.Name,
                            LocationName = l.Name,
                            TenantId = t.Id,
                            TenantName = t.Name,
                            li.Quantity,
                            iu.ReferenceWeight,
                            ItemType = itt.Name,
                        };


            var items = await query
              .Where(r => r.TenantId == request.TenantId)
              .OrderBy(i => i.LocationName)
              .ThenBy(i => i.ScaleAddress)
              .Select(x => new ItemModel
              {
                  Id = x.Id,
                  ItemName = x.ItemName,
                  ItemNumber = x.ItemNumber,
                  ScaleAddress = x.ScaleAddress,
                  FacilityName = x.FacilityName,
                  LocationName = x.LocationName,
                  TenantName = x.TenantName,
                  Quantity = x.Quantity,
                  ReferenceWeight = x.ReferenceWeight,
                  ItemType = x.ItemType,
              })
              .ToListAsync(cancellationToken)
              .ConfigureAwait(false);

            return new GetItemsByTenantResponse
            {
                Items = items
            };
        }
    }
}
