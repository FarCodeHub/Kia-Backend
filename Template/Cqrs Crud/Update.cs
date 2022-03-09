using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Model;
using Application.$fileinputname$.Model;
using Infrastructure.Utilities;
using Persistence.SqlServer;
using AutoMapper;
using AutoMapper.QueryableExtensions;


namespace $rootnamespace$.$fileinputname$.Command.Update
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult> , IMapFrom<Domain.Entities.$fileinputname$>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<$safeitemname$, Domain.Entities.$fileinputname$>()
                .IgnoreAllNonExisting();
        }
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
            var entity = await _repository
                .Find<Domain.Entities.$fileinputname$>(c =>
            c.ObjectId(request.Id))
            .FirstOrDefaultAsync(cancellationToken);

            entity = new DynamicUpdator(_repository.Model)
            .Update<Domain.Entities.$fileinputname$>(entity, _mapper.Map<Domain.Entities.$fileinputname$>(request));

            _repository.Update(entity);

            return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity));
        }
    }
}
