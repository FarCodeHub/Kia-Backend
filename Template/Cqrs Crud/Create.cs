using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Application.Common;
using Application.Common.Interfaces;
using Persistence.SqlServer;
using Application.Common.Model;
using Application.Common.Mappings;

namespace $rootnamespace$.$fileinputname$.Command.Create
{
    public class $safeitemname$ : CommandBase, IRequest<ServiceResult> , IMapFrom<$safeitemname$> , ICommand
    {
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
            var entity = await _repository.Insert(_mapper.Map<Domain.Entities.$fileinputname$>(request));
            return ServiceResult.Set(true);
        }
    }
}
