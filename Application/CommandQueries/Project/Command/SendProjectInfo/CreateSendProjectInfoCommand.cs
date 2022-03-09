using System.Linq;
using System.Threading;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;
using Service.Services.SmsService;

namespace Application.CommandQueries.Project.Command.SendProjectInfo
{
    public class CreateSendProjectInfoCommand : CommandBase, IRequest<bool>, IMapFrom<CreateSendProjectInfoCommand>, ICommand
    {
        public int ProjectId { get; set; }
        public int CustomerId { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<CreateSendProjectInfoCommand, Domain.Entities.Project>()
                .IgnoreAllNonExisting();
        }
    }

    public class CreateSendProjectInfoCommandHandler : IRequestHandler<CreateSendProjectInfoCommand, bool>
    {
        private readonly IProjectService _projectService;
        private readonly IRepository _repository;
        private readonly ISmsService _smsService;
        public CreateSendProjectInfoCommandHandler(IProjectService projectService, IRepository repository, ISmsService smsService)
        {
            _projectService = projectService;
            _repository = repository;
            _smsService = smsService;
        }


        public async System.Threading.Tasks.Task<bool> Handle(CreateSendProjectInfoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var project = await _projectService.FindById(request.ProjectId).FirstOrDefaultAsync(cancellationToken);

                var customer = await _repository.GetQuery<Communication>().OrderBy(x => x.Id).Include(x => x.Customer)
                    .ThenInclude(x => x.Person)
                    .LastOrDefaultAsync(cancellationToken);

                var fullname = customer?.Customer?.Person?.FirstName + " " + customer?.Customer?.Person?.LastName;

                var res = _smsService.SendQuickMessage(customer?.CustomerConnectedNumber,
                    SmsService.QuickMessageType.ProjectInfo, new SmsService.ProjectInfoQuickMessage()
                    {
                        CustomerFullName = fullname,
                        ProjectTitle = project.Title,
                        ProjectFileLink = project.FileUrl
                    });
                return res;

            }
            catch
            {
                return false;
            }
        }
    }
}