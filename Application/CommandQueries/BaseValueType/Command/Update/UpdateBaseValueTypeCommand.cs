using Application.CommandQueries.BaseValueType.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;


namespace Application.CommandQueries.BaseValueType.Command.Update
{
    public class UpdateBaseValueTypeCommand : CommandBase, IRequest<BaseValueTypeModel>, IMapFrom<Domain.Entities.BaseValueType>, ICommand
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public int? ParentId { get; set; }
        public string GroupName { get; set; }
        public bool IsReadOnly { get; set; }
        public string SubSystem { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateBaseValueTypeCommand, Domain.Entities.BaseValueType>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateBaseValueTypeCommandHandler : IRequestHandler<UpdateBaseValueTypeCommand, BaseValueTypeModel>
    {
        private readonly IMapper _mapper;
        private readonly IBaseValueTypeService _baseValueTypeService;
        private readonly IRepository _repository;

        public UpdateBaseValueTypeCommandHandler(IMapper mapper, IBaseValueTypeService baseValueTypeService, IRepository repository)
        {
            _mapper = mapper;
            _baseValueTypeService = baseValueTypeService;
            _repository = repository;
        }

        public async Task<BaseValueTypeModel> Handle(UpdateBaseValueTypeCommand request, CancellationToken cancellationToken)
        {

            var updatedEntity = await _baseValueTypeService.Update(_mapper.Map<Domain.Entities.BaseValueType>(request), cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<BaseValueTypeModel>(updatedEntity.Entity);
            }

            return null;
        }
    }
}
