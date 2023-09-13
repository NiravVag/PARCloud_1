using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Tenants.Commands.UpsertTenantApplicationNotificationSettings
{
    public class UpsertTenantApplicationNotificationSettingsHandler : IRequestHandler<UpsertTenantApplicationNotificationSettingsCommand, UpsertTenantApplicationNotificationSettingsResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpsertTenantApplicationNotificationSettingsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpsertTenantApplicationNotificationSettingsHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IMapper mapper, ILogger<UpsertTenantApplicationNotificationSettingsHandler> logger)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpsertTenantApplicationNotificationSettingsResponse> Handle(UpsertTenantApplicationNotificationSettingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_currentUserService.UserId <= 0)
                {
                    throw new ArgumentException("Current User Id can't be less than or equal to zero");
                }

                // 1. Get the Command Center Application Id.
                var commandCenterApplicationId = (short)ApplicationType.PARCommand;

                // 2. Get tenants application notification settings for the command center from the DB
                var tenantApplicationNotificationSettings = await _dbContext.TenantApplicationNotificationSettings.Where(ut => ut.ApplicationId == commandCenterApplicationId).ToListAsync();

                // 3. Mark all the old setting as deleted.
                tenantApplicationNotificationSettings.ForEach(x =>
                {
                    x.Deleted = true;
                    _dbContext.TenantApplicationNotificationSettings.Update(x);
                });

                TenantApplicationNotificationSetting entity;

                // 4. Loop on the new tenants ids (add the new ones to DB, and update the existing ones deleted flag to false)
                foreach (var tenantId in request.TenantIds)
                {
                    entity = tenantApplicationNotificationSettings.FirstOrDefault(x => x.TenantId == tenantId);

                    // if there is a record for tenantId in the DB.
                    if (entity != null)
                    {
                        // Update the deleted field.
                        entity.Deleted = false;
                        _dbContext.TenantApplicationNotificationSettings.Update(entity);
                        continue;

                    }
                    else //No record in the DB for the TenantId, Insert the tenant notification setting in the database, because there is no recored.
                    {
                        entity = new TenantApplicationNotificationSetting()
                        {
                            ApplicationId = commandCenterApplicationId,
                            TenantId = tenantId,
                            Created = DateTime.UtcNow,
                            CreatedUserId = _currentUserService.UserId,
                        };

                        _dbContext.TenantApplicationNotificationSettings.Add(entity);
                        continue;
                    }
                }

                var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


                return new UpsertTenantApplicationNotificationSettingsResponse()
                {
                    Successful = result > 0
                };

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
