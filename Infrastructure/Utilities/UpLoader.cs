﻿using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Infrastructure.ConfigurationAccessor;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Utilities
{
    public class UpLoader : IUpLoader
    {
        private readonly IConfigurationAccessor _configurationAccessor;

        public UpLoader(IConfigurationAccessor configurationAccessor)
        {
            _configurationAccessor = configurationAccessor;
        }

        public async Task<IoResult> UpLoadAsync(IFormFile file, string fileName,
            CustomPath customPath, CancellationToken cancellationToken = new CancellationToken())
        {
            if (file is null)
            {
                return new IoResult();
            }
            if (string.IsNullOrEmpty(fileName)) fileName = Guid.NewGuid().ToString();

            var extention = Path.GetExtension(file.FileName);
            var path = GetCustomePath(customPath);
            var reletivePath = Path.Combine(path, $"{fileName}{extention}");
            var fullPath = Path.Combine(_configurationAccessor.GetIoPaths().Root, reletivePath);
            try
            {
                var dir = new System.IO.FileInfo(fullPath);
                dir?.Directory?.Create();

                await using var myFile = File.Create(fullPath);
                await file.CopyToAsync(myFile, cancellationToken);
            }
            catch (Exception e)
            {
                return new IoResult();
            }
            return new IoResult() { FullPath = "assets/" + fullPath.Replace('\\', '/'), ReletivePath = "assets/" + reletivePath.Replace('\\', '/'), FileName = fileName };
        }


        public async Task<IoResult> UpLoadAsync(string sourceFileReletiveAddress, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var path = GetCustomePath(customPath);
            var fileName = Path.GetFileName(sourceFileReletiveAddress);
            var sourcePath = Path.Combine(_configurationAccessor.GetIoPaths().Root, sourceFileReletiveAddress.Remove(0, 7));
            var reletiveDestinationPath = Path.Combine(path, fileName);
            var destinationFullPath = Path.Combine(_configurationAccessor.GetIoPaths().Root, reletiveDestinationPath);
            sourcePath = sourcePath.Replace('/', '\\');
            destinationFullPath = destinationFullPath.Replace('/', '\\');

            await using (var source = File.Open(sourcePath, FileMode.OpenOrCreate))
            {
                var dir = new System.IO.FileInfo(destinationFullPath);
                dir?.Directory?.Create();

                await using (var destination = File.Create(destinationFullPath))
                {
                    await source.CopyToAsync(destination, cancellationToken);
                }
            }

            return new IoResult() { FullPath = "assets/" + destinationFullPath.Replace('\\', '/'), ReletivePath = "assets/" + reletiveDestinationPath.Replace('\\', '/'), FileName = fileName };
        }



        public async Task<IoResult> DeleteAsync(string sourceFileReletiveAddress, CustomPath customPath,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var path = GetCustomePath(customPath);
            var fileName = Path.GetFileName(sourceFileReletiveAddress);
            var sourcePath = Path.Combine(_configurationAccessor.GetIoPaths().Root, sourceFileReletiveAddress.Remove(0, 7));
            var reletiveDestinationPath = Path.Combine(path, fileName);
            var destinationFullPath = Path.Combine(_configurationAccessor.GetIoPaths().Root, reletiveDestinationPath);


            await using (var source = File.Open(sourcePath.Replace('/', '\\'), FileMode.OpenOrCreate))
            {
                await using (var destination = File.Create(destinationFullPath.Replace('/', '\\')))
                {
                    await source.CopyToAsync(destination, cancellationToken);
                }
            }

            return new IoResult() { FullPath = "assets/" + destinationFullPath.Replace('\\', '/'), ReletivePath = "assets/" + reletiveDestinationPath.Replace('\\', '/'), FileName = fileName };
        }



        private string GetCustomePath(CustomPath customPath)
        {
            string path;
            switch (customPath)
            {
                case CustomPath.PersonProfile:
                    path = _configurationAccessor.GetIoPaths().PersonProfile;
                    break;
                case CustomPath.ContractAttachment:
                    path = _configurationAccessor.GetIoPaths().ContractAttachment + "\\" + DateTime.Now.ToString("yyyy-MM");
                    break;
                case CustomPath.Temp:
                    path = _configurationAccessor.GetIoPaths().Temp + "\\" + DateTime.Now.ToString("yyyy-MM");
                    break;
                case CustomPath.SmsAttachment:
                    path = _configurationAccessor.GetIoPaths().SmsAttachment + "\\" + DateTime.Now.ToString("yyyy-MM");
                    break;
                case CustomPath.ProjectFile:
                    path = _configurationAccessor.GetIoPaths().ProjectFile;
                    break;
                default:
                    path = _configurationAccessor.GetIoPaths().Root;
                    break;
            }
            return path;
        }
    }
}