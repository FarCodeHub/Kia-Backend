using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.BaseValue.Command.Update
{
    public class UpdateBaseValueCommand : CommandBase, IRequest<BaseValueModel>, IMapFrom<Domain.Entities.BaseValue>, ICommand
    {
        public int Id { get; set; }
        public int BaseValueTypeId { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string Value { get; set; }
        public int OrderIndex { get; set; }
        public bool IsReadOnly { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBaseValueCommand, Domain.Entities.BaseValue>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBaseValueCommandHandler : IRequestHandler<UpdateBaseValueCommand, BaseValueModel>
    {
        private readonly IMapper _mapper;
        private readonly IBaseValueService _baseValueService;
        private readonly IRepository _repository;

        public UpdateBaseValueCommandHandler(IMapper mapper, IBaseValueService baseValueService, IRepository repository)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
            _repository = repository;
        }

        public async Task<BaseValueModel> Handle(UpdateBaseValueCommand request, CancellationToken cancellationToken)
        {

            var updatedEntity = await _baseValueService.Update(_mapper.Map<Domain.Entities.BaseValue>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<BaseValueModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
