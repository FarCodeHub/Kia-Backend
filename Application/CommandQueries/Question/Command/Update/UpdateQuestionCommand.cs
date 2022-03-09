using Application.CommandQueries.Question.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.Question.Command.Update
{
    public class UpdateQuestionCommand : CommandBase, IRequest<QuestionModel>, IMapFrom<Domain.Entities.Question>, ICommand
    {
        public int Id { get; set; }
        public int QuestionTypBaseId { get; set; } = default!;
        public string Title { get; set; } = default!;
        public int AnswerTypeBaseId { get; set; } = default!;
        public int? AnswerOptionBaseTypeId { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateQuestionCommand, Domain.Entities.Question>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateQuestionCommandHandler : IRequestHandler<UpdateQuestionCommand, QuestionModel>
    {
        private readonly IMapper _mapper;
        private readonly IQuestionService _questionService;
        private readonly IRepository _repository;

        public UpdateQuestionCommandHandler(IMapper mapper, IQuestionService questionService, IRepository repository)
        {
            _mapper = mapper;
            _questionService = questionService;
            _repository = repository;
        }

        public async Task<QuestionModel> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
        {
            var updatedEntity = await _questionService.Update(_mapper.Map<Domain.Entities.Question>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<QuestionModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
