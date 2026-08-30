using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Attachments
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);

        bool Delete(string fileName , string folderName );

        (Stream stream, string contentType)? GetFile(string fileName, string folderName);
    }
}
