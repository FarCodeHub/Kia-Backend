using Application.CommandQueries.Question.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Question.Command.Delete
{
    public class DeleteQuestionCommand : CommandBase, IRequest<QuestionModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteQuestionCommandHandler : IRequestHandler<DeleteQuestionCommand, QuestionModel>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        private readonly IRepository _repository;

        public DeleteQuestionCommandHandler(IMapper mapper, IQuestionService questionService, IRepository repository)
        {
            _mapper = mapper;
            _questionService = questionService;
            _repository = repository;
        }

        public async Task<QuestionModel> Handle(DeleteQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _questionService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<QuestionModel>(entity.Entity);
            }
            return null;
        }
    }
}
