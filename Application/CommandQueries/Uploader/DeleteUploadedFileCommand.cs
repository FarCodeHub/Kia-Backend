using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Infrastructure.Mappings;
using Infrastructure.Models;
using Infrastructure.Utilities;
using MediatR;

namespace Application.CommandQueries.Uploader
{
    public class DeleteUploadedFileCommand : CommandBase, IRequest<string>, IMapFrom<DeleteUploadedFileCommand>, ICommand
    {
        public string FileName { get; set; }
    }

    public class DeleteUploadedFileCommandHandler : IRequestHandler<DeleteUploadedFileCommand, string>
    {
        private readonly IUpLoader _upLoader;

        public DeleteUploadedFileCommandHandler(IUpLoader upLoader)
        {
            _upLoader = upLoader;
        }


        public async Task<string> Handle(DeleteUploadedFileCommand request, CancellationToken cancellationToken)
        {
            //var profileUrl = await _upLoader.UpLoadAsync(request.File, Guid.NewGuid().ToString(),
            //    CustomPath.Temp, cancellationToken);

            //return profileUrl.ReletivePath;
            return null;
        }
    }
}
