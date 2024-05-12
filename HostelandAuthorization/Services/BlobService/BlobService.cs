using Azure.Storage;
using Azure.Storage.Blobs;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

public class BlobService : IBlobService {

    private readonly BlobServiceClient _blobServiceClient;
    private static readonly IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();

    private readonly string ImageFolder = "EXE";
    private readonly string ContainerClient = "files";
    private readonly string _storageAccount = config["Azure:Blob:StorageAccount"];
    private readonly string _accessKey = config["Azure:Blob:AccessKey"];
    public BlobService() {
        var credential = new StorageSharedKeyCredential(_storageAccount, _accessKey);
        var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
        _blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
    }

    public async Task<string> UploadFileAsync(IFormFile file) {
        string fileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        Stream stream = file.OpenReadStream();
        string absoluteUrl = "";
        string webpFileName = Path.ChangeExtension(fileName, "webp");
        using (Image image = Image.Load(stream)) {
            int maxWidth = 2000;
            int maxHeight = 2000;
            if (image.Width > maxWidth || image.Height > maxHeight) {
                image.Mutate(x => x.Resize(new ResizeOptions {
                    Size = new Size(maxWidth, maxHeight),
                    Mode = ResizeMode.Max
                }));
            }
            using (MemoryStream webpStream = new MemoryStream()) {
                await image.SaveAsync(webpStream, new WebpEncoder());
                webpStream.Position = 0;
                var blobContainer = _blobServiceClient.GetBlobContainerClient(ContainerClient);
                var blob = blobContainer.GetBlobClient($"{ImageFolder}/{webpFileName}");
                await blob.UploadAsync(webpStream, true);
                absoluteUrl = blob.Uri.AbsoluteUri;
            }
        }
        return absoluteUrl;
    }

    public async Task<bool> DeleteBlobsByUrlAsync(string imageUrl) {
        try {
            var imageBlobName = GetBlobNameFromUrl(imageUrl, ImageFolder);
            var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerClient);
            var imageBlobClient = containerClient.GetBlobClient(imageBlobName);
            if (await imageBlobClient.ExistsAsync())
                await imageBlobClient.DeleteAsync();
            return true;
        } catch (Exception ex) {
            return false;
        }
    }

    public async Task<bool> DeleteBlobsByUrlAsync(string imageUrl, string thumbnailImageUrl) {
        try {
            var imageBlobName = GetBlobNameFromUrl(imageUrl, ImageFolder);
            var thumbnailImageBlobName = GetBlobNameFromUrl(thumbnailImageUrl, ImageFolder);
            var containerClient = _blobServiceClient.GetBlobContainerClient(ContainerClient);
            var imageBlobClient = containerClient.GetBlobClient(imageBlobName);
            var thumbnailImageBlobClient = containerClient.GetBlobClient(thumbnailImageBlobName);
            if (await imageBlobClient.ExistsAsync())
                await imageBlobClient.DeleteAsync();
            if (await thumbnailImageBlobClient.ExistsAsync())
                await thumbnailImageBlobClient.DeleteAsync();
            return true;
        } catch (Exception ex) {
            return false;
        }
    }

    public static string GetBlobNameFromUrl(string url, string folder) {
        Uri uri = new Uri(url);
        string blobName = Uri.UnescapeDataString(uri.Segments.Last());
        return folder + "/" + blobName;
    }
}

