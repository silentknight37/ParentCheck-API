using MediatR;
using ParentCheck.Envelope;
using System;

namespace ParentCheck.Query
{
    public class UploadLibrayFileCommand : IRequest<RequestSaveEnvelop>
    {
        public UploadLibrayFileCommand(long instituteId, string libraryDescription, string encryptedFileName, string uploadPath, string fileName,bool isInstituteLevel,int contentType, long userId)
        {
            this.InstituteId = instituteId;
            this.LibraryDescription = libraryDescription;
            this.EncryptedFileName = encryptedFileName;
            this.UploadPath = uploadPath;
            this.FileName = fileName;
            this.IsInstituteLevel = isInstituteLevel;
            this.ContentType = contentType;
            this.UserId = userId;
        }

        public long InstituteId { get; set; }   
        public string LibraryDescription { get; set; }
        public string EncryptedFileName { get; set; }
        public string UploadPath { get; set; }
        public string FileName { get; set; }
        public bool IsInstituteLevel { get; set; }
        public int ContentType { get; set; }
        public long UserId { get; set; }
    }
}
