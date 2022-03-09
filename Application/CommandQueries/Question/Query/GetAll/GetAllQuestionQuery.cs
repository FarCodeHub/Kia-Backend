using System.Collections.Generic;
using Application.CommandQueries.Question.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Utilities;
using Persistence.SqlServer.QueryProvider;

namespace Application.CommandQueries.Question.Query.GetAll
{
    public class GetAllQuestionQuery : Pagination, IRequest<PagedList<List<QuestionModel>>>, ISearchableRequest, IQuery
    {
        public Condition[] Conditions { get; set; }
    }

    public class GetAllQuestionQueryHandler : IRequestHandler<GetAllQuestionQuery, PagedList<List<QuestionModel>>>
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public GetAllQuestionQueryHandler(IMapper mapper, IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        public async Task<PagedList<List<QuestionModel>>> Handle(GetAllQuestionQuery request, CancellationToken cancellationToken)
        {
            var entities = _questionService.GetAll()
                .ProjectTo<QuestionModel>(_mapper.ConfigurationProvider)
                .OrderByMultipleColumns(request.OrderByProperty)
                .MakeStringSearchQuery(request.Conditions);


            return new PagedList<List<QuestionModel>>()
            {
                Data = await entities
                    .Paginate(request.Paginator())
                    .ToListAsync(cancellationToken),
                TotalCount = request.PageIndex <= 1
                    ? await entities
                        .CountAsync(cancellationToken)
                    : 0
            };
        }
    }
}
