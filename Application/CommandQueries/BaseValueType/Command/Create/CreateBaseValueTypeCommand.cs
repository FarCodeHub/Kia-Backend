using Application.CommandQueries.BaseValueType.Model;
using AutoMapper;
using Infrastructure.Mappings;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.BaseValueType.Command.Create
{
    public class CreateBaseValueTypeCommand : CommandBase, IRequest<BaseValueTypeModel>, IMapFrom<CreateBaseValueTypeCommand>, ICommand
    {
        public string Title { get; set; }
        public string UniqueName { get; set; }
        public string GroupName { get; set; }
        public int? ParentId { get; set; }
        public bool IsReadOnly { get; set; }
        public string SubSystem { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateBaseValueTypeCommand, Domain.Entities.BaseValueType>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateBaseValueTypeCommandHandler : IRequestHandler<CreateBaseValueTypeCommand, BaseValueTypeModel>
    {
        private readonly IMapper _mapper;
        private readonly IBaseValueTypeService _baseValueTypeService;
        private readonly IRepository _repository;

        public CreateBaseValueTypeCommandHandler(IMapper mapper, IBaseValueTypeService baseValueTypeService, IRepository repository)
        {
            _mapper = mapper;
            _baseValueTypeService = baseValueTypeService;
            _repository = repository;
        }


        public async Task<BaseValueTypeModel> Handle(CreateBaseValueTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _baseValueTypeService.Add(_mapper.Map<Domain.Entities.BaseValueType>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<BaseValueTypeModel>(entity.Entity);
            }
            return null;
        }
    }
}
