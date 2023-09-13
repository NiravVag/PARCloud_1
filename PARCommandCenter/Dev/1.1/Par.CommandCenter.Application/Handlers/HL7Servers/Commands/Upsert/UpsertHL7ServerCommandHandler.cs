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

namespace Par.CommandCenter.Application.Handlers.HL7Servers.Commands.Upsert
{
    public class UpsertHL7ServerCommandHandler : IRequestHandler<UpsertHL7ServerCommand, UpsertHL7ServerResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<UpsertHL7ServerCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public UpsertHL7ServerCommandHandler(ICurrentUserService currentUserService, IApplicationDbContext dbContext, IMapper mapper, ILogger<UpsertHL7ServerCommandHandler> logger)
        {
            _currentUserService = currentUserService;
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UpsertHL7ServerResponse> Handle(UpsertHL7ServerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                HL7Server entity;

                if (request.Id.HasValue && request.Id.Value > 0)
                {
                    entity = await _dbContext.HL7Servers.SingleOrDefaultAsync(c => c.Id == request.Id.Value).ConfigureAwait(false);

                    if (entity != null && entity.Id > 0)
                    {
                        _mapper.Map(request, entity);

                        entity.Modified = DateTime.UtcNow;
                        entity.ModifiedUserId = _currentUserService.UserId;

                        _dbContext.HL7Servers.Update(entity);
                    }
                }
                else
                {
                    if (await _dbContext.HL7Servers.AnyAsync(x => x.Id == request.Id).ConfigureAwait(false))
                    {
                        throw new BadRequestException("HL7Server Already Exists");
                    }

                    entity = _mapper.Map<HL7Server>(request);

                    entity.Created = DateTime.UtcNow;
                    entity.CreatedUserId = _currentUserService.UserId;


                    _dbContext.HL7Servers.Add(entity);
                }

                var result = await _dbContext.SaveChangesAsync(cancellationToken).ConfigureAwait(false);


                if (result > 0 && entity != null)
                {
                    return new UpsertHL7ServerResponse()
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
