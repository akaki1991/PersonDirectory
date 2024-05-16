using Microsoft.AspNetCore.Http;
using PersonDirectory.Application.Shared;
using System.IO.Abstractions;
using System.Drawing;
using PersonDirectory.Shared;

namespace PersonDirectory.Application.Services;

internal class FileService(IFileSystem fileSystem) : IFileService
{
    private readonly IFileSystem _fileSystem = fileSystem;

    public void Delete(string key)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos/", key);
        var fileInfo = _fileSystem.FileInfo.New(path);

        if (fileInfo.Exists)
            fileInfo.Delete();
    }

    public async Task<(string FileName, int Width, int Height)> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(file.FileName))
            throw new AppException(ErrorCodes.InvalidFileName);

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var newFileName = Path.GetRandomFileName() + extension;

        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "photos");

        var filePath = Path.Combine(folderPath, newFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        int width, height;

        using (var image = Image.FromFile(filePath))
        {
            width = image.Width;
            height = image.Height;
        }

        return (newFileName, width, height);
    }
}
