using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.UserRole.Model;
using AutoMapper;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Application.CommandQueries.UserRole.Command.Update
{
    public class UpdateUserRoleCommand : CommandBase, IRequest<UserRoleModel>, IMapFrom<Domain.Entities.UserRole>, ICommand
    {
        public int Id { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserRoleCommand, Domain.Entities.UserRole>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, UserRoleModel>
    {
        private readonly IMapper _mapper;
        private readonly IUserRoleService _userRoleService;
        private readonly IRepository _repository;

        public UpdateUserRoleCommandHandler(IMapper mapper, IUserRoleService userRoleService, IRepository repository)
        {
            _mapper = mapper;
            _userRoleService = userRoleService;
            _repository = repository;
        }

        public async Task<UserRoleModel> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
        {
            var entity = await _userRoleService.FindById(request.Id).FirstOrDefaultAsync(cancellationToken);

            // update properties

            var updatedEntity = await _userRoleService.Update(entity, cancellationToken);

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {
                return _mapper.Map<UserRoleModel>(updatedEntity.Entity);
            }
            return null;
        }
    }
}
