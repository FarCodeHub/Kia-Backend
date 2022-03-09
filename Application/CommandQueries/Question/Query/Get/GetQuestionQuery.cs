using Application.CommandQueries.Question.Model;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;


namespace Application.CommandQueries.Question.Query.Get
{
    public class GetQuestionQuery : IRequest<QuestionModel>, IQuery
    {
        public int Id { get; set; }
    }

    public class GetQuestionQueryHandler : IRequestHandler<GetQuestionQuery, QuestionModel>
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public GetQuestionQueryHandler(IMapper mapper, IQuestionService questionService)
        {
            _mapper = mapper;
            _questionService = questionService;
        }

        public async Task<QuestionModel> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
        {
            var entity = _questionService.FindById(request.Id);

            return await entity
                .ProjectTo<QuestionModel>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
