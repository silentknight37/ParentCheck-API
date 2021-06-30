using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class UploadAssignmentFileCommand : IRequest<RequestSaveEnvelop>
    {
        public UploadAssignmentFileCommand(long assignmentId, string encryptedFileName, string uploadPath, string fileName,long userId)
        {
            this.AssignmentId = assignmentId;
            this.EncryptedFileName = encryptedFileName;
            this.UploadPath = uploadPath;
            this.FileName = fileName;
            this.UserId = userId;
        }

        public long AssignmentId { get; set; }
        public string EncryptedFileName { get; set; }
        public string UploadPath { get; set; }
        public string FileName { get; set; }
        public long UserId { get; set; }
    }
}
