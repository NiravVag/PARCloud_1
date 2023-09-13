using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Handlers.Routers.Queries.GetRoutersByTenant;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Routers.Commands.Register
{
    public class RegisterRouterCommandHandler : IRequestHandler<RegisterRouterCommand, RegisterRouterResponse>
    {
        private readonly ILogger<RegisterRouterCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;


        public RegisterRouterCommandHandler(IApplicationDbContext dbContext, ICurrentUserService currentUserService, IMapper mapper, ILogger<RegisterRouterCommandHandler> logger)
        {
            _dbContext = dbContext;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RegisterRouterResponse> Handle(RegisterRouterCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Registering router ({0}).", request.TenantId);           

            var router = new Router();
            string newRouterAddress = string.Empty;
            string? newServiceName = null;
            string? newServiceDisplayName = null;




            var tenant = await (from t in _dbContext.Tenants
                                where t.Id == request.TenantId
                                where !t.Deleted
                                select new Tenant
                                {
                                    Id = t.Id,
                                    Name = t.Name
                                }).FirstOrDefaultAsync(cancellationToken)
                            .ConfigureAwait(false);

            if (tenant == null)
            {
                throw new Exception($"We can't find the tenant with tenant Id {request.TenantId} in the backend store");
            }

            // Check if router address was supplied with the request.
            // If was supplied, check if it's 14 character long.
            // If wasn't supplied, generate new router address (Has to be 14 characters and end in 'CF):
            //      1. Get tenant routers include the deleted.
            //      2. Generate new Random Router Number in this format {tenantId}{randomNumber}{CF}
            //      3. check router address is not previously generated, if exists generate another number.

            if (request.Address?.Length > 0)
            {
                if (request.Address.Length > 14)
                {
                    throw new Exception($"Router address must not exceed 14 character.");
                }

                newRouterAddress = request.Address;
                newServiceName = request.ServiceName;
                newServiceDisplayName = request.ServiceDisplayName;
            }
            else
            {
                var tenantRouters = await _dbContext.Routers
                    .Where(r => r.TenantId == request.TenantId)
                    .OrderByDescending(r => r.Created)
                     .ProjectTo<RouterModel>(_mapper.ConfigurationProvider)
                     .ToListAsync()
                     .ConfigureAwait(false);

                tenantRouters = (from r in tenantRouters
                                select new RouterModel()
                                {
                                    Id = r.Id,
                                    Address = r.Address.Replace("CF", string.Empty)
                                }).ToList();

                var regex = new Regex(@"^[0-9]*$");

                //tenantRouters = tenantRouters.Any(x => System.Text.RegularExpressions.Regex.IsMatch(x.Address, @"\d"))
                tenantRouters = (from r in tenantRouters
                                 where regex.IsMatch(r.Address)
                                 select new RouterModel()
                                 {
                                     Id = r.Id,
                                     Address = r.Address
                                 }).ToList();

                var lastRouterAddressIndex = 0;

                var checkRouterAddress = true;

                while (checkRouterAddress)
                {
                    RouterModel lastRouter = null;
                    var lastRouterAddress = "";

                    ////var random = RandomNumberGenerator.GetInt32(1, 10000);
                    if (tenantRouters.Any())
                    {
                        lastRouter = tenantRouters.ElementAt(lastRouterAddressIndex);
                        if (lastRouter != null)
                        {
                            lastRouterAddress = lastRouter.Address;
                        }
                    }

                    ++lastRouterAddressIndex;

                    if (lastRouter != null && lastRouterAddress.StartsWith(request.TenantId.ToString()))
                    {
                        newRouterAddress = lastRouterAddress.Replace("CF", string.Empty);

                        newRouterAddress = newRouterAddress.Remove(0, request.TenantId.ToString().Length);

                        newRouterAddress = newRouterAddress.TrimStart('0');



                        newRouterAddress = $"{request.TenantId}{(Convert.ToInt32(newRouterAddress) + 1).ToString().PadLeft(12 - request.TenantId.ToString().Length, '0')}CF";
                    }
                    else
                    {
                        newRouterAddress = $"{request.TenantId}{1.ToString().PadLeft(12 - request.TenantId.ToString().Length, '0')}CF";
                    }

                    // check one more time if the new router address was used before.
                    checkRouterAddress = tenantRouters.Any(x => x.Address == newRouterAddress);
                }

                newServiceName = $"parcloudrouter{newRouterAddress}";

                newServiceDisplayName = $"PAR Cloud Router {newRouterAddress}";
            }

            router.Address = newRouterAddress;
            router.ServiceName = newServiceName;
            router.ServiceDisplayName = newServiceDisplayName;

            // Check one more time in DB          
            Router routerRecord = await _dbContext.Routers.FirstOrDefaultAsync(x => x.Address == router.Address).ConfigureAwait(false);

            if (routerRecord != null)
            {
                string message = $"Router already exists with address '{routerRecord.Address}'";
                throw new Exception(message);
            }
            

            if (request.IsPcRouter && string.IsNullOrWhiteSpace(request.ComputerName))
            {
                throw new ArgumentNullException(nameof(request), "When the IsPcRouter is set to true, you must provide a computer name");
                
            }
            else
            {
                router.ComputerName = request.ComputerName;

                ////router.ServiceDisplayName = null;
                ////router.ServiceName = null;
            }

            // Get last virtual machine in DB.

            var lastVM = await _dbContext.VirtualMachines.OrderByDescending(vm => vm.Id).FirstOrDefaultAsync();            
           

            // Register the router
            router.TenantId = request.TenantId;
            router.FirmwareVersion = "1.0.0.1";
            router.IsRunning = false;
            router.Created = DateTime.Now;
            router.CreatedUserId = _currentUserService.UserId;
            router.Modified = DateTime.Now;
            router.ModifiedUserId = _currentUserService.UserId;
            router.DeviceTypeId = (request.IsPcRouter) ? (byte)DeviceType.CloudRouterOnPC : (byte) DeviceType.CloudRouter;
           
            
            if(lastVM != null)
            {
                router.VirtualMachineId = lastVM.Id;
            }
            

            _dbContext.Routers.Add(router);

            await _dbContext.SaveChangesAsync(cancellationToken);

            if (router.Id > 0)
            {
                router = await _dbContext.Routers.SingleOrDefaultAsync(x => x.Id == router.Id);
            }


            if (router == null)
            {
                throw new Exception($"Router with address '{router.Address}' not found.");
            }

            return new RegisterRouterResponse()
            {
                RouterId = router.Id,
                RouterAddress = router.Address,
            };
        }
    }
}
