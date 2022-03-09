using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Service.Interfaces;
using Application.$fileinputname$.Model;
using Persistence.SqlServer;
using Application.Common.Common.Interfaces;
using Application.Common.Common.Model;
using Infrastructure.Mappings;

namespace $rootnamespace$.$fileinputname$.Command.Create
{
    public class Create$fileinputname$Command : CommandBase, IRequest<ServiceResult>, IMapFrom<Create$fileinputname$Command>, ICommand
    {
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Create$fileinputname$Command, Domain.Entities.$fileinputname$>()
                .IgnoreAllNonExisting();
        }
    }

    public class Create$fileinputname$CommandHandler : IRequestHandler<Create$fileinputname$Command, ServiceResult>
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


        public async Task<ServiceResult> Handle(Create$fileinputname$Command request, CancellationToken cancellationToken)
        {
            var entity = await _$fileinputname$Service.Add(_mapper.Map<Domain.Entities.$fileinputname$>(request));
            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity.Entity));
            }
            return ServiceResult.Set(false);
        }
    }
}
