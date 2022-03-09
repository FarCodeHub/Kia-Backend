using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Unit.Model;
using Persistence.SqlServer;
using Application.Common.Model;
using Application.$fileinputname$.Model;

namespace $rootnamespace$.$fileinputname$.Command.Delete
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult>, ICommand
    {
        public int Id {get;set;}
    }

    public class $safeitemname$Handler : IRequestHandler<$safeitemname$, ServiceResult>
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public $safeitemname$Handler(IRepository repository, IMapper mapper)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<ServiceResult> Handle($safeitemname$ request, CancellationToken cancellationToken)
        {
            var entity =await _repository
                .Find<Domain.Entities.$fileinputname$>(c =>
             c.ObjectId(request.Id))
             .FirstOrDefaultAsync(cancellationToken);
             
            _repository.Delete(entity);
            return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity));       
        }
    }
}
