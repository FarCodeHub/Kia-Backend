using Application.Common.Common.Interfaces;
using Application.Common.Common.Model;
using Infrastructure.Mappings;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Service.Interfaces;
using Persistence.SqlServer;

namespace $rootnamespace$.$fileinputname$.Command.Delete
{
    public class Delete$fileinputname$Command : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id { get; set; }
    }

    public class Delete$fileinputname$CommandHandler : IRequestHandler<Delete$fileinputname$Command, ServiceResult>
    {
          private readonly IMapper _mapper;
        private readonly I$fileinputname$Service _$fileinputname$Service;
        private readonly IRepository _repository;

        public Create$fileinputname$CommandHandler(IMapper mapper, I$fileinputname$Service $fileinputname$Service, IRepository repository)
        {
            _mapper = mapper;
            _$fileinputname$Service = $fileinputname$Service;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle(Delete$fileinputname$Command request, CancellationToken cancellationToken)
        {
            var entity = await _$fileinputname$Service.SoftDelete(request.Id, cancellationToken);
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity.Entity));
            }
            return ServiceResult.Set(false);     
        }
    }
}
