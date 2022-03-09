using System.Threading;
using System.Threading.Tasks;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Utilities
{
    public interface IUpLoader : IService
    {
        Task<IoResult> UpLoadAsync(IFormFile file, string fileName, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken());

        Task<IoResult> UpLoadAsync(string sourceFileReletiveAddress, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken());

       Task<IoResult> DeleteAsync(string sourceFileReletiveAddress, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken());
    }
}