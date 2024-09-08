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
        // Pretraži GridFS po userId
        var filter = Builders<GridFSFileInfo>.Filter.Eq("metadata.userId", userId);
        var fileInfos = await _gridFSBucket.Find(filter).ToListAsync();

        if (fileInfos == null || !fileInfos.Any())
        {
            return NotFound("No files found for this user.");
        }

        // Napravi listu fajlova sa osnovnim informacijama
        var files = fileInfos.Select(fileInfo => new
        {
            FileId = fileInfo.Id.ToString(),
            FileName = fileInfo.Filename,
            UploadDate = fileInfo.UploadDateTime,
            ContentType = fileInfo.Metadata.GetValue("contentType").AsString
        });

        return Ok(files);
    }

}