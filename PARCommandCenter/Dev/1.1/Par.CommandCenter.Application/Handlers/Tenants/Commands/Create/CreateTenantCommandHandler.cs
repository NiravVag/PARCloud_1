using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Par.CommandCenter.Application.Common.Exceptions;
using Par.CommandCenter.Application.Interfaces;
using Par.CommandCenter.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Par.CommandCenter.Application.Handlers.Tenants.Commands.Create
{
    public class CreateTenantCommandHandler : IRequestHandler<CreateTenantCommand, CreateTenantResponse>
    {
        private readonly ILogger<CreateTenantCommandHandler> _logger;
        private readonly IApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public CreateTenantCommandHandler(IApplicationDbContext dbContext, IMapper mapper, ILogger<CreateTenantCommandHandler> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CreateTenantResponse> Handle(CreateTenantCommand request, CancellationToken cancellationToken)
        {
            if (_dbContext.Tenants.Any(x => x.Name == request.Name))
            {
                throw new BadRequestException("Tenant Name Already Exists");
            }

            var tenant = _mapper.Map<Tenant>(request);
            tenant.Created = DateTime.Now;
            tenant.EmployeeSecurityTypeId = 1;

            _dbContext.Tenants.Add(tenant);

            await _dbContext.SaveChangesAsync(cancellationToken);


            return new CreateTenantResponse()
            {
                Id = tenant.Id
            };
        }
    }
}
