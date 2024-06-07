using Azure.Storage.Blobs;
using SixLabors.ImageSharp.Formats.Webp;

public interface IBlobService {
    Task<string> UploadFileAsync(IFormFile file);
    Task<bool> DeleteBlobsByUrlAsync(string imageUrl);
    Task<bool> DeleteBlobsByUrlAsync(string imageUrl, string thumbnailImageUrl);
}

