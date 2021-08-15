using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class UploadContentFileCommand : IRequest<RequestSaveEnvelop>
    {
        public UploadContentFileCommand(long topicContentId, string encryptedFileName, string uploadPath, string fileName,long userId)
        {
            this.TopicContentId = topicContentId;
            this.EncryptedFileName = encryptedFileName;
            this.UploadPath = uploadPath;
            this.FileName = fileName;
            this.UserId = userId;
        }

        public long TopicContentId { get; set; }
        public string EncryptedFileName { get; set; }
        public string UploadPath { get; set; }
        public string FileName { get; set; }
        public long UserId { get; set; }
    }
}
