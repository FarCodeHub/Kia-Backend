using System;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.CommandQueries.Uploader
{
    public class CreateUploaderCommand : CommandBase, IRequest<string>, IMapFrom<CreateUploaderCommand>, ICommand
    {
        public IFormFile File { get; set; }
    }

    public class CreateUploaderCommandHandler : IRequestHandler<CreateUploaderCommand, string>
    {
        private readonly IUpLoader _upLoader;

        public CreateUploaderCommandHandler(IUpLoader upLoader)
        {
            _upLoader = upLoader;
        }


        public async Task<string> Handle(CreateUploaderCommand request, CancellationToken cancellationToken)
        {
            var profileUrl = await _upLoader.UpLoadAsync(request.File, Guid.NewGuid().ToString(),
                CustomPath.Temp, cancellationToken);

            return profileUrl.ReletivePath;
        }
    }
}
