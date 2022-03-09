using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.BaseValue.Command.Create
{
    public class CreateBaseValueCommand : CommandBase, IRequest<BaseValueModel>, IMapFrom<CreateBaseValueCommand>, ICommand
    {
        public int BaseValueTypeId { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string Value { get; set; }
        public int OrderIndex { get; set; }
        public bool IsReadOnly { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBaseValueCommand, Domain.Entities.BaseValue>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBaseValueCommandHandler : IRequestHandler<CreateBaseValueCommand, BaseValueModel>
    {
        private readonly IMapper _mapper;
        private readonly IBaseValueService _baseValueService;
        private readonly IRepository _repository;

        public CreateBaseValueCommandHandler(IMapper mapper, IBaseValueService baseValueService, IRepository repository)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
            _repository = repository;
        }


        public async Task<BaseValueModel> Handle(CreateBaseValueCommand request, CancellationToken cancellationToken)
        {
            var entity = await _baseValueService.Add(_mapper.Map<Domain.Entities.BaseValue>(request));

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<BaseValueModel>(entity.Entity);
            }
            return null;
        }
    }
}
