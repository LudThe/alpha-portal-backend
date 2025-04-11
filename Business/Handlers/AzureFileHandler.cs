using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace Business.Handlers;

public class AzureFileHandler(string connectionString, string containerName) : IFileHandler
{
    private readonly BlobContainerClient _containerClient = new(connectionString, containerName);
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return null!;

        var fileExtension = Path.GetExtension(file.Name);
        var fileName = $"{Guid.NewGuid()}{fileExtension}";

        string contentType = !string.IsNullOrEmpty(file.ContentType)
            ? file.ContentType
            : "application/octet-stream";

        if ((contentType == "application/octet-stream" || string.IsNullOrEmpty(contentType)) && fileExtension.Equals(".svg", StringComparison.OrdinalIgnoreCase))
            contentType = "image/svg+xml";

        BlobClient blobClient = _containerClient.GetBlobClient(fileName);
        var uploadOptions = new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } };

        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, uploadOptions);

        return blobClient.Uri.ToString();
    }


    public async Task RemoveFileAsync(string blobUri)
    {
        if (string.IsNullOrEmpty(blobUri))
            return;

        Uri blobUriObj = new Uri(blobUri);
        string blobName = blobUriObj.Segments[blobUriObj.Segments.Length - 1];
        BlobClient blobClient = _containerClient.GetBlobClient(blobName);

        await blobClient.DeleteIfExistsAsync();
    }
}
