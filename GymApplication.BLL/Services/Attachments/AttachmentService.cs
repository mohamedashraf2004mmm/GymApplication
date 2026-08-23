using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymApplication.BLL.Services.Attachments
{
    public class AttachmentService : IAttachmentService
    {
        public Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
