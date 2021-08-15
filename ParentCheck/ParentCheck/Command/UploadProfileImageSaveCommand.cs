using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class UploadProfileImageSaveCommand : IRequest<RequestSaveEnvelop>
    {
        public UploadProfileImageSaveCommand(string encryptedFileName, string uploadPath, string fileName,long userId)
        {
            this.EncryptedFileName = encryptedFileName;
            this.UploadPath = uploadPath;
            this.FileName = fileName;
            this.UserId = userId;
        }

        public string EncryptedFileName { get; set; }
        public string UploadPath { get; set; }
        public string FileName { get; set; }
        public long UserId { get; set; }
    }
}
