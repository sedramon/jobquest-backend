using jobquest.Infrastructure.Services.Files;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace jobquest_backend.Controllers;

[ApiController]
[Route("api/files")]
public class FileController : ControllerBase
{
    private readonly FileService _fileService;
    private readonly IGridFSBucket _gridFSBucket;  // Dodaj ovo

    public FileController(FileService fileService, IGridFSBucket gridFSBucket)  // Injektuj GridFSBucket
    {
        _fileService = fileService;
        _gridFSBucket = gridFSBucket;  // Inicijalizuj GridFSBucket
    }

    // Example to upload a file
    [HttpPost("upload")]
    public async Task<IActionResult> UploadFile(IFormFile file, [FromForm] string userId)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (string.IsNullOrEmpty(userId))
        {
            return BadRequest("User ID is required.");
        }

        using (var stream = file.OpenReadStream())
        {
            var fileId = await _fileService.UploadFileAsync(stream, file.FileName, file.ContentType, userId);
            return Ok(new { FileId = fileId.ToString() });
        }
    }
    
    [HttpPost("upload-application-file")]
    public async Task<IActionResult> UploadApplicationFile(IFormFile file, string userId, string jobPostId)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        try
        {
            using (var fileStream = file.OpenReadStream())
            {
                // Add metadata for userId and jobPostId
                var fileId = await _fileService.UploadFileForApplicationAsync(fileStream, file.FileName, file.ContentType, userId, jobPostId);

                // Return the fileId and optionally metadata
                return Ok(new { fileId });
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }

    // Example to download a file by ObjectId
    [HttpGet("download/{id}")]
    public async Task<IActionResult> DownloadFile(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId fileId))
        {
            return BadRequest("Invalid file ID.");
        }

        // Preuzmi fajl
        var fileBytes = await _fileService.DownloadFileAsync(fileId);

        // Preuzmi metapodatke o fajlu, uključujući contentType
        var fileInfo = await _fileService.GetFileMetadataAsync(fileId);

        // Proveri da li fajl ima validan contentType u metapodacima, ako nema koristi default
        var contentType = fileInfo.Metadata.GetValue("contentType").AsString ?? "application/octet-stream";

        // Vraćanje fajla sa odgovarajućim content-type
        return new FileStreamResult(new MemoryStream(fileBytes), contentType)
        {
            FileDownloadName = fileInfo.Filename  // Postavi naziv fajla prilikom preuzimanja
        };
    }
    
    [HttpGet("user-files/{userId}")]
    public async Task<IActionResult> GetFilesByUserId(string userId)
    {
        // Filter to find files by userId and exclude those that have a jobPostId in metadata
        var filter = Builders<GridFSFileInfo>.Filter.And(
            Builders<GridFSFileInfo>.Filter.Eq("metadata.userId", userId),
            Builders<GridFSFileInfo>.Filter.Or(
                Builders<GridFSFileInfo>.Filter.Exists("metadata.jobPostId", false),  // Files without jobPostId
                Builders<GridFSFileInfo>.Filter.Eq("metadata.jobPostId", BsonNull.Value)  // Files with null jobPostId
            )
        );

        var fileInfos = await _gridFSBucket.Find(filter).ToListAsync();

        if (fileInfos == null || !fileInfos.Any())
        {
            return NotFound("No files found for this user.");
        }

        // Create a list of file information
        var files = fileInfos.Select(fileInfo => new
        {
            FileId = fileInfo.Id.ToString(),
            FileName = fileInfo.Filename,
            UploadDate = fileInfo.UploadDateTime,
            ContentType = fileInfo.Metadata.GetValue("contentType").AsString
        });

        return Ok(files);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteFile(string id)
    {
        if (!ObjectId.TryParse(id, out ObjectId fileId))
        {
            return BadRequest("Invalid file ID.");
        }

        try
        {
            // Call the service to delete the file
            await _fileService.DeleteFileAsync(fileId);
            return Ok($"File with ID {id} deleted successfully.");
        }
        catch (GridFSFileNotFoundException)
        {
            return NotFound("File not found.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting the file.");
        }
    }


    
    [HttpGet("download/{userId}/{jobPostId}")]
    public async Task<IActionResult> DownloadFileByUserAndJobPostId(string userId, string jobPostId)
    {
        try
        {
            // Download the file
            var fileBytes = await _fileService.DownloadFileByUserAndJobPostAsync(userId, jobPostId);

            // Get the file metadata for the content type and file name
            var filter = Builders<GridFSFileInfo>.Filter.And(
                    Builders<GridFSFileInfo>.Filter.Eq("metadata.userId", userId),
                Builders<GridFSFileInfo>.Filter.Eq("metadata.jobPostId", jobPostId)
                );

            var fileInfo = await _gridFSBucket.Find(filter).FirstOrDefaultAsync();
            var contentType = fileInfo.Metadata.GetValue("contentType").AsString ?? "application/octet-stream";
            var fileName = fileInfo.Filename;

            // Return the file with the correct content type and file name
            return new FileStreamResult(new MemoryStream(fileBytes), contentType)
            {
                FileDownloadName = fileName
            };
        }
        catch (FileNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error.");
        }
    }


}