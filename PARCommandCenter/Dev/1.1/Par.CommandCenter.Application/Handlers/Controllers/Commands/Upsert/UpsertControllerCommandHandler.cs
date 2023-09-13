using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Common.Exceptions;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Controllers.Commands.Upsert
{
    public class UpsertControllerCommandHandler : IRequestHandler<UpsertControllerCommand, UpsertControllerResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpsertControllerCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpsertControllerCommandHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IMapper mapper, ILogger<UpsertControllerCommandHandler> logger)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpsertControllerResponse> Handle(UpsertControllerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Controller entity;

                if (request.ControllerId.HasValue && request.ControllerId.Value > 0)
                {
                    entity = await _dbContext.Controllers.SingleOrDefaultAsync(c => c.Id == request.ControllerId.Value).ConfigureAwait(false);

                    if (entity != null && entity.Id > 0)
                    {
                        _mapper.Map(request, entity);

                        entity.FirmwareVersion = "0.5.0";

                        entity.Modified = DateTime.Now;
                        entity.ModifiedUserId = _currentUserService.UserId;

                        _dbContext.Controllers.Update(entity);
                    }
                }
                else
                {
                    if (await _dbContext.Controllers.AnyAsync(x => x.PortName.ToLower() == request.PortName.ToLower() && x.RouterId == request.RouterId).ConfigureAwait(false))
                    {
                        throw new BadRequestException("Controller Already Exists");
                    }

                    entity = _mapper.Map<Controller>(request);

                    entity.FirmwareVersion = "0.5.0";
                    entity.Created = DateTime.Now;
                    entity.Modified = DateTime.Now;
                    entity.CreatedUserId = _currentUserService.UserId;
                    entity.ModifiedUserId = _currentUserService.UserId;

                    _dbContext.Controllers.Add(entity);
                }

                var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


                if (result > 0 && entity != null)
                {
                    return new UpsertControllerResponse()
                    {
                        Id = entity.Id
                    };
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
