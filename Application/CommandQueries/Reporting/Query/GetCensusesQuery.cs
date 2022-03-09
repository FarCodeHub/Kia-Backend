using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.reverse;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.Reporting.Query
{
    public class GetCensusesQuery : IRequest<CensusesSpResult>, IQuery
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int? UserId { get; set; }
        public int? BranchId { get; set; }
        public int? UnitId { get; set; }
    }

    public class GetCensusesQueryHandler : IRequestHandler<GetCensusesQuery, CensusesSpResult>
    {
        private readonly IMapper _mapper;
        private readonly IReportingService _reportingService;

        public GetCensusesQueryHandler(IMapper mapper, IReportingService reportingService)
        {
            _mapper = mapper;
            _reportingService = reportingService;
        }

        public async Task<CensusesSpResult> Handle(GetCensusesQuery request, CancellationToken cancellationToken)
        {
            return await _reportingService.Census(request.BranchId,request.UnitId,request.UserId,request.From, request.To);

        }
    }
}