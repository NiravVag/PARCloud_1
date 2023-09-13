using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities.Users;
using Par.CommandCenter.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Users.Commands.UpsertCurrentUserTenantSettings
{
    public class UpsertCurrentUserTenantSettingsHandler : IRequestHandler<UpsertCurrentUserTenantSettingsCommand, UpsertCurrentUserTenantSettingsResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpsertCurrentUserTenantSettingsHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpsertCurrentUserTenantSettingsHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IMapper mapper, ILogger<UpsertCurrentUserTenantSettingsHandler> logger)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpsertCurrentUserTenantSettingsResponse> Handle(UpsertCurrentUserTenantSettingsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (_currentUserService.UserId <= 0)
                {
                    throw new ArgumentException("Current User Id can't be less than or equal to zero");
                }

                // 1. Get the Command Center Application Id.
                var commandCenterApplicationId = (short)ApplicationType.PARCommand;

                // 2. Get current user application tenant settings for the command center from the DB
                var userTenantSettings = await _dbContext.UserApplicationTenantSettings.Where(ut => ut.UserId == _currentUserService.UserId && ut.ApplicationId == commandCenterApplicationId).ToListAsync();

                // 3. Mark all the old user tenant setting as deleted.
                userTenantSettings.ForEach(x =>
                {
                    x.Deleted = true;
                    _dbContext.UserApplicationTenantSettings.Update(x);
                });

                UserApplicationTenantSetting entity;

                // 4. Loop on the new tenants ids (add the new ones to DB, and update the existing ones deleted flag to false)
                foreach (var tenantId in request.TenantIds)
                {
                    entity = userTenantSettings.FirstOrDefault(x => x.TenantId == tenantId);

                    // if there is a record for tenantId in the DB.
                    if (entity != null)
                    {
                        // Update the deleted field.
                        entity.Deleted = false;
                        _dbContext.UserApplicationTenantSettings.Update(entity);
                        continue;

                    }
                    else //No record in the DB for the TenantId, Insert the tenant user setting the database, because there is no recored.
                    {
                        entity = new UserApplicationTenantSetting()
                        {
                            ApplicationId = commandCenterApplicationId,
                            UserId = _currentUserService.UserId,
                            TenantId = tenantId,
                            Created = DateTime.UtcNow,
                            CreatedUserId = _currentUserService.UserId,
                        };

                        _dbContext.UserApplicationTenantSettings.Add(entity);
                        continue;
                    }
                }

                var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


                return new UpsertCurrentUserTenantSettingsResponse()
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
