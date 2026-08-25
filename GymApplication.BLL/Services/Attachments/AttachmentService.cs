using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long MAXIMUM_FILE_Size = 5 * 1024 * 1024;
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = { ".png", ".jpeg", ".jpg" }; 

        public AttachmentService(ILogger<AttachmentService>logger , IWebHostEnvironment env)
        {
            this._logger = logger;
            this._env = env;
        }
        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream == null || !fileStream.CanRead) return null;
            if (fileStream.Length == 0) return null;

            if (fileStream.Length > MAXIMUM_FILE_Size)
            {
                _logger.LogError($"File rejected : Too large | {fileStream.Length} bytes");
                return null;
            }

            var extension = Path.GetExtension(fileName);
            if(string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension))
            {
                _logger.LogError($"File rejected | the extension {extension} is not allowed");
                return null;
            }

            var uploadsFolder = Path.Combine(_env.ContentRootPath, folderName);

            Directory.CreateDirectory(uploadsFolder); //creates folder if not found 

            var storedFileName = $"{Guid.NewGuid()}{fileName}";

            var filePath = Path.Combine(uploadsFolder, storedFileName);
            try
           {
           using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
           await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
           }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upload file {fileName}");
                return null;
            }

        }
    }
}
