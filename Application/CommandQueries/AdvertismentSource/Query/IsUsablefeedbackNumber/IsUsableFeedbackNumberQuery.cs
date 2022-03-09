using System;
using System.Threading;
using AutoMapper;
using Infrastructure.Interfaces;
using MediatR;
using Service.Interfaces;

namespace Application.CommandQueries.AdvertismentSource.Query.IsUsablefeedbackNumber
{
    public class IsUsableFeedbackNumberQuery : IRequest<bool>, IQuery
    {
        public int FeedbackNumber { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndateTime { get; set; }
    }

    public class IsUsableFeedbackNumberQueryHandler : IRequestHandler<IsUsableFeedbackNumberQuery, bool>
    {
        private readonly IAdvertisementService _advertisementSourceService;
        private readonly IMapper _mapper;

        public IsUsableFeedbackNumberQueryHandler(IMapper mapper, IAdvertisementService advertisementSourceService)
        {
            _mapper = mapper;
            _advertisementSourceService = advertisementSourceService;
        }

        public async System.Threading.Tasks.Task<bool> Handle(IsUsableFeedbackNumberQuery request, CancellationToken cancellationToken)
        {
            var entity = _advertisementSourceService
                .IsUsableFeedbackNumber(request.FeedbackNumber, request.StartDateTime, request.EndateTime);

            return await entity;
        }
    }
}
