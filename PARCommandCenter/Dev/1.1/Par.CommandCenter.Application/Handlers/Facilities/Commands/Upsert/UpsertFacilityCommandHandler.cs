using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Common.Exceptions;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Entities.HealthCheck;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Facilities.Commands.Upsert
{
    public class UpsertFacilityCommandHandler : IRequestHandler<UpsertFacilityCommand, UpsertFacilityResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpsertFacilityCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpsertFacilityCommandHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IMapper mapper, ILogger<UpsertFacilityCommandHandler> logger)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpsertFacilityResponse> Handle(UpsertFacilityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Facility entity = null;

                var userName = string.IsNullOrWhiteSpace(_currentUserService.UPN) ? _currentUserService.PreferredUsername: _currentUserService.UPN;
                var sessionUser = await _dbContext.SetSessionUserAsync(userName).ConfigureAwait(false);

                using (sessionUser)
                {
                    if (request.VPNConnectionName != null && !string.IsNullOrEmpty(request.VPNConnectionName))
                    {
                        request.VPNConnectionName = request.VPNConnectionName.Trim();
                    }

                    if (request.FacilityId.HasValue && request.FacilityId.Value > 0)
                    {
                        entity = await _dbContext.Facilities.FirstOrDefaultAsync(f => f.Id == request.FacilityId.Value);

                        HealthCheckVPN vpnHealthCheck = null;
                        if (entity.VPNConnectionName != request.VPNConnectionName)
                        {
                            _logger.LogInformation("entity.VPNConnectionName != request.VPNConnectionName is true");
                            vpnHealthCheck = await _dbContext.HealthCheckVPNs.FirstOrDefaultAsync(hc => hc.ConnectionName.Trim().ToLower() == (string.IsNullOrWhiteSpace(request.VPNConnectionName) ? string.Empty : request.VPNConnectionName.Trim().ToLower()));

                            if (vpnHealthCheck == null)
                            {
                                _logger.LogInformation("vpnHealthCheck == null is true");
                                vpnHealthCheck = await _dbContext.HealthCheckVPNs.FirstOrDefaultAsync(hc => hc.ConnectionName.Trim().ToLower() == entity.VPNConnectionName.Trim().ToLower());

                                // Check if there is other facilities using the same connection, if there is any do not update the tenant.
                                var facilities = await _dbContext.Facilities.Where(f => f.VPNConnectionName.Trim().ToLower() == entity.VPNConnectionName.Trim().ToLower() && f.Id != entity.Id).ToListAsync();

                                if (!facilities.Any())
                                {
                                    _logger.LogInformation("!facilities.Any() is true");
                                    vpnHealthCheck.TenantId = null;
                                }
                            }
                            else
                            {
                                // Check if there is other facilities using the same connection, if there is any do not update the tenant.
                                var facilities = await _dbContext.Facilities.Where(f => f.VPNConnectionName.Trim().ToLower() == (string.IsNullOrWhiteSpace(entity.VPNConnectionName) ? string.Empty : entity.VPNConnectionName.Trim().ToLower()) && f.Id != entity.Id).ToListAsync();

                                if (!facilities.Any())
                                {
                                    _logger.LogInformation("!facilities.Any() is true");
                                    vpnHealthCheck.TenantId = null;
                                }

                                _logger.LogInformation("vpnHealthCheck == null is false");
                                vpnHealthCheck.TenantId = entity.TenantId;
                            }

                            _dbContext.HealthCheckVPNs.Update(vpnHealthCheck);
                        }

                        if (entity != null && entity.Id > 0)
                        {
                            _mapper.Map(request, entity);

                            entity.Modified = DateTime.Now;
                            entity.ModifiedUserId = _currentUserService.UserId;

                            _dbContext.Facilities.Update(entity);
                        }
                    }
                    else
                    {
                        if (_dbContext.Facilities.Any(x => x.TenantId == request.TenantId && x.Name.ToLower() == request.Name.ToLower()))
                        {
                            throw new BadRequestException("Facility Name Already Exists");
                        }

                        entity = _mapper.Map<Facility>(request);

                        HealthCheckVPN vpnHealthCheck = null;
                        if (!string.IsNullOrEmpty(request.VPNConnectionName))
                        {
                            vpnHealthCheck = await _dbContext.HealthCheckVPNs.FirstOrDefaultAsync(f => f.ConnectionName == request.VPNConnectionName);
                        }

                        if (vpnHealthCheck != null)
                        {
                            vpnHealthCheck.TenantId = entity.TenantId;
                            _dbContext.HealthCheckVPNs.Update(vpnHealthCheck);
                        }

                        entity.Created = DateTime.Now;
                        entity.CreatedUserId = _currentUserService.UserId;
                        _dbContext.Facilities.Add(entity);
                    }

                    await _dbContext.SaveChangesAsync(cancellationToken);

                    if (entity != null)
                    {
                        return new UpsertFacilityResponse()
                        {
                            Id = entity.Id
                        };
                    }

                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing UpsertFacilityCommandHandler. Error Message" + ex.Message);

                throw;
            }
        }
    }
}
