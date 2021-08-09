using ParentCheck.BusinessObject;
using ParentCheck.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public interface ISettingDomain
    {
        Task<List<InstituteUserDTO>> GeInstituteUsers(long userId);
        Task<bool> SaveInstituteUser(long id, string firstName, string lastName, int roleId, long studentUserId, long parentUserid, long classTeacherUserId, long headTeacherUserId, long communicationGroup, string username, DateTime dateOfBirth, string password, bool isActive, long userId);

        Task<List<AcademicDTO>> GetAcademicYear(long userId);
        Task<bool> SaveAcademicYear(long id, int yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId);

        Task<List<AcademicTermDTO>> GetAcademicTerm(long userId);
        Task<bool> SaveAcademicTerm(long id, string term, long yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId);

        Task<List<AcademicClassDTO>> GetAcademicClass(long userId);
        Task<bool> SaveAcademicClass(long id, string academicClass, long yearAcademic, long responsibleUserId, bool isActive, long userId);

        Task<List<SubjectDTO>> GetSubject(long userId);
        Task<bool> SaveSubject(long id, string subject, string descriptionText, bool isActive, long userId);

        Task<List<StudentEnrollDTO>> GetStudentEnroll(long classId, long academicYear, long userId);
        Task<bool> SaveStudentEnroll(long id, long yearAcademic, long classId, long studentId, bool isActive, long userId);
    }
}
