using Microsoft.AspNetCore.Http;

namespace PersonDirectory.Application.Services;

public interface IFileService
{
    Task<(string FileName, int Width, int Height)> Upload(IFormFile file, CancellationToken cancellationToken);
    void Delete(string key);
}