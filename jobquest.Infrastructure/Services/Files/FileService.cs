using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace jobquest.Infrastructure.Services.Files;

public class FileService
{
    private readonly IGridFSBucket _gridFSBucket;

    public FileService(IMongoDatabase database)
    {
        _gridFSBucket = new GridFSBucket(database);
    }

    // Method to upload files (e.g., profile pictures)
    public async Task<ObjectId> UploadFileAsync(Stream fileStream, string fileName, string contentType, string userId)
    {
        var gridFSOptions = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "contentType", contentType },
                { "userId", userId }  // Dodajemo userId u metapodatke
            }
        };

        var fileId = await _gridFSBucket.UploadFromStreamAsync(fileName, fileStream, gridFSOptions);
        return fileId;
    }

    // Method to download files by ID
    public async Task<byte[]> DownloadFileAsync(ObjectId fileId)
    {
        using (var memoryStream = new MemoryStream())
        {
            await _gridFSBucket.DownloadToStreamAsync(fileId, memoryStream);
            return memoryStream.ToArray();
        }
    }
    
    public async Task<GridFSFileInfo> GetFileMetadataAsync(ObjectId fileId)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);  // Koristi "_id" umesto "Id"
        var fileInfo = await _gridFSBucket.Find(filter).FirstOrDefaultAsync();
        return fileInfo;
    }
}