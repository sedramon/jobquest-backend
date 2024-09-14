
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
    
    public async Task<ObjectId> UploadFileForApplicationAsync(Stream fileStream, string fileName, string contentType, string userId, string jobPostId)
    {
        var gridFSOptions = new GridFSUploadOptions
        {
            Metadata = new BsonDocument
            {
                { "contentType", contentType },
                { "userId", userId },        // Track which user uploaded the file
                { "jobPostId", jobPostId }   // Track the associated job post
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
    
    public async Task<byte[]> DownloadFileByUserAndJobPostAsync(string userId, string jobPostId)
    {
        // Create a filter to find files with the specified userId and jobPostId
        var filter = Builders<GridFSFileInfo>.Filter.And(
                Builders<GridFSFileInfo>.Filter.Eq("metadata.userId", userId),
                Builders<GridFSFileInfo>.Filter.Eq("metadata.jobPostId", jobPostId)
            );

        // Find the first matching file
        var fileInfo = await _gridFSBucket.Find(filter).FirstOrDefaultAsync();

        if (fileInfo == null)
        {
            throw new FileNotFoundException($"No file found for user {userId} and job post {jobPostId}.");
        }

        // Use the existing DownloadFileAsync method to get the file's content
        var fileBytes = await DownloadFileAsync(fileInfo.Id);

        return fileBytes;
    }

    
    public async Task<GridFSFileInfo> GetFileMetadataAsync(ObjectId fileId)
    {
        var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", fileId);  // Koristi "_id" umesto "Id"
        var fileInfo = await _gridFSBucket.Find(filter).FirstOrDefaultAsync();
        return fileInfo;
    }
}