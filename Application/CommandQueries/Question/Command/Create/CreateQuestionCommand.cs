using Application.CommandQueries.Question.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.Question.Command.Create
{
    public class CreateQuestionCommand : CommandBase, IRequest<QuestionModel>, IMapFrom<CreateQuestionCommand>, ICommand
    {
        public int QuestionTypBaseId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int AnswerTypeBaseId { get; set; } = default!;
        public int? AnswerOptionBaseTypeId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateQuestionCommand, Domain.Entities.Question>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, QuestionModel>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        private readonly IRepository _repository;

        public CreateQuestionCommandHandler(IMapper mapper, IQuestionService questionService, IRepository repository)
        {
            _mapper = mapper;
            _questionService = questionService;
            _repository = repository;
        }


        public async Task<QuestionModel> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _questionService.Add(_mapper.Map<Domain.Entities.Question>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<QuestionModel>(entity.Entity);
            }

            return null;
        }
    }
}
