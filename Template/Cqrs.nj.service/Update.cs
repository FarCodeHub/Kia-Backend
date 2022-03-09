using Application.$fileinputname$.Model;
using Application.Common.Common.Interfaces;
using Application.Common.Common.Model;
using Infrastructure.Mappings;
using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.SqlServer;


namespace $rootnamespace$.$fileinputname$.Command.Update
{
    public class Update$fileinputname$Command : CommandBase, IRequest<ServiceResult>, IMapFrom<Domain.Entities.$fileinputname$>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Update$fileinputname$Command, Domain.Entities.$fileinputname$>()
                .IgnoreAllNonExisting();
        }
    }


    public class Update$fileinputname$CommandHandler : IRequestHandler<Update$fileinputname$Command, ServiceResult>
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

        public async Task<ServiceResult> Handle(Update$fileinputname$Command request, CancellationToken cancellationToken)
        {
            var entity = await _$fileinputname$Service.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            // update properties

            var updatedEntity = await _$fileinputname$Service.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return ServiceResult.Set(_mapper.Map<$fileinputname$Model>(entity.Entity));
            }
            return ServiceResult.Set(false);             }
    }
}
