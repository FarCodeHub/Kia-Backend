using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.CommandQueries.User.Model;
using AutoMapper;
using Infrastructure.ConfigurationAccessor;
using Infrastructure.Exceptions;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
namespace Application.CommandQueries.User.Command.Update
{
    public class UpdateUserCommand : CommandBase, IRequest<bool>, IMapFrom<UpdateUserCommand>, ICommand
    {
        public int Id { get; set; }
        public string Username { get; set; } = default!;
        public bool IsBlocked { get; set; } = default!;
        public string? BlockedReason { get; set; }
        public IList<int> RolesIdList { get; set; }
        public string Password { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<UpdateUserCommand, Domain.Entities.User>()
                .IgnoreAllNonExisting();
        }
    }


    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, bool>
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserRoleService _userRoleService;
        private readonly IConfigurationAccessor _configurationAccessor;
        private readonly IHandledErrorManager _handledErrorManager;
        public UpdateUserCommandHandler(IMapper mapper, IUserService userService, IRepository repository, IUserRoleService userRoleService, IConfigurationAccessor configurationAccessor, IHandledErrorManager handledErrorManager)
        {
            _mapper = mapper;
            _userService = userService;
            _repository = repository;
            _userRoleService = userRoleService;
            _configurationAccessor = configurationAccessor;
            _handledErrorManager = handledErrorManager;
        }

        public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.Username))
            {
                request.Username = request.Username.ToLower();
            }

            var entity = await _userService.FindById(request.Id).Include(x => x.UserRoleUsers).FirstOrDefaultAsync(cancellationToken);

            var updatedEntity = await _userService.Update(_mapper.Map<Domain.Entities.User>(request), cancellationToken);


            if (request.RolesIdList != null && request.RolesIdList.Count != 0)
            {
                foreach (var removedRole in entity.UserRoleUsers.Select(x => x.RoleId).Except(request.RolesIdList))
                {
                    var deletingRole = await _userRoleService.FindById(entity.UserRoleUsers
                            .First(x => x.RoleId == removedRole && x.UserId == entity.Id).Id)
                        .FirstOrDefaultAsync(cancellationToken);
                    await _userRoleService.SoftDelete(deletingRole.Id, cancellationToken);
                }

                foreach (var addedRole in request.RolesIdList.Except(entity.UserRoleUsers.Select(x => x.RoleId)))
                {
                    await _userRoleService.Add(new Domain.Entities.UserRole() { User = entity, RoleId = addedRole });
                }
            }

            if (await _repository.SaveChangesAsync(cancellationToken) > 0)
            {

                return true;
            }
            return false;
        }
    }
}
