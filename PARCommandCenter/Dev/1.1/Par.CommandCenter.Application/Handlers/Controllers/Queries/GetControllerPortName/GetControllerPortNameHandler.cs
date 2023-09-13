using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Par.CommandCenter.Application.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Controllers.Queries.GetControllerPortName
{
    public class GetControllerPortNameHandler : IRequestHandler<GetControllerPortNameQuery, GetControllerPortNameResponse>
    {

        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetControllerPortNameHandler(IApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GetControllerPortNameResponse> Handle(GetControllerPortNameQuery request, CancellationToken cancellationToken)
        {
            var query = from r in _dbContext.Routers
                        join c in _dbContext.Controllers on new { RouterId = r.Id, Deleted = r.Deleted } equals new { RouterId = c.RouterId, Deleted = false }
                        select new { c.Id, c.ControllerTypeId, c.RouterId, c.PortName };


            var lastController = await query
              .Where(c => c.RouterId == request.RouterId && c.ControllerTypeId == request.ControllerTypeId)
              .OrderByDescending(c => c.Id)
              .FirstOrDefaultAsync(cancellationToken)
              .ConfigureAwait(false);

            if (lastController == null)
            {
                var portName = request.ControllerTypeId switch
                {
                    3 => "N2C1",
                    4 => "COW1",
                    _ => throw new System.Exception("Controller type is not recognized"),
                };

                return new GetControllerPortNameResponse
                {
                    PortName = portName
                };
            }
            else
            {
                // N2C type
                if (lastController.ControllerTypeId == 3)
                {
                    _ = int.TryParse(lastController.PortName.Replace("N2C", string.Empty), out int number);

                    return new GetControllerPortNameResponse
                    {
                        PortName = $"N2C{++number}"
                    };

                }
                else if (lastController.ControllerTypeId == 4)
                {
                    _ = int.TryParse(lastController.PortName.Replace("COW", string.Empty), out int number);

                    return new GetControllerPortNameResponse
                    {
                        PortName = $"COW{++number}"
                    };
                }

                return new GetControllerPortNameResponse
                {
                    PortName = string.Empty
                };

            }
        }
    }
}
