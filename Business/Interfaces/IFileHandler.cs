using Microsoft.AspNetCore.Http;

namespace Business.Handlers;

public interface IFileHandler
{
    Task RemoveFileAsync(string blobUri);
    Task<string> UploadFileAsync(IFormFile file);
}
