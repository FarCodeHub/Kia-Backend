using Application.CommandQueries.BaseValue.Model;
using AutoMapper;
using MediatR;
using Service.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Models;

namespace Application.CommandQueries.BaseValue.Command.Delete
{
    public class DeleteBaseValueCommand : CommandBase, IRequest<BaseValueModel>, ICommand
    {
        public int Id { get; set; }

    }

    public class DeleteBaseValueCommandHandler : IRequestHandler<DeleteBaseValueCommand, BaseValueModel>
    {
        private readonly IMapper _mapper;
        private readonly IBaseValueService _baseValueService;
        private readonly IRepository _repository;

        public DeleteBaseValueCommandHandler(IMapper mapper, IBaseValueService baseValueService, IRepository repository)
        {
            _mapper = mapper;
            _baseValueService = baseValueService;
            _repository = repository;
        }

        public async Task<BaseValueModel> Handle(DeleteBaseValueCommand request, CancellationToken cancellationToken)
        {
            var entity = await _baseValueService.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<BaseValueModel>(entity.Entity);
            }

            return null;
        }
    }
}
