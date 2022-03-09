using Application.CommandQueries.BaseValueType.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.BaseValueType.Command.Delete
{
    public class DeleteBaseValueTypeCommand : CommandBase, IRequest<BaseValueTypeModel>, ICommand
    {
        public int Id { get; set; }
    }

    public class DeleteBaseValueTypeCommandHandler : IRequestHandler<DeleteBaseValueTypeCommand, BaseValueTypeModel>
    {
        private readonly IMapper _mapper;
        private readonly IBaseValueTypeService _baseValueTypeService;
        private readonly IRepository _repository;

        public DeleteBaseValueTypeCommandHandler(IMapper mapper, IBaseValueTypeService baseValueTypeService, IRepository repository)
        {
            _mapper = mapper;
            _baseValueTypeService = baseValueTypeService;
            _repository = repository;
        }

        public async Task<BaseValueTypeModel> Handle(DeleteBaseValueTypeCommand request, CancellationToken cancellationToken)
        {
            var entity = await _baseValueTypeService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<BaseValueTypeModel>(entity.Entity);
            }

            return null;
        }
    }
}
