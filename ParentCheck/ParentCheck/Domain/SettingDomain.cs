using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Domain
{
    public class SettingDomain : ISettingDomain
    {
        private readonly ISettingRepository supportTicketRepository;

        public SettingDomain(ISettingRepository supportTicketRepository)
        {
            this.supportTicketRepository = supportTicketRepository;
        }

        public async Task<List<InstituteUserDTO>> GeInstituteUsers( long userId)
        {
            return await supportTicketRepository.GeInstituteUsers(userId);
        }

        public async Task<bool> SaveInstituteUser(long id, string firstName, string lastName, int roleId, long studentUserId, long parentUserid, long classTeacherUserId, long headTeacherUserId, long communicationGroup, string username, DateTime dateOfBirth, string password, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveInstituteUser(id, firstName, lastName, roleId, studentUserId, parentUserid, classTeacherUserId, headTeacherUserId, communicationGroup, username, dateOfBirth, password, isActive, userId);
        }

        public async Task<List<AcademicDTO>> GetAcademicYear(long userId)
        {
            return await supportTicketRepository.GetAcademicYear(userId);
        }

        public async Task<bool> SaveAcademicYear(long id, int yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveAcademicYear(id, yearAcademic, fromDate, toDate, isActive, userId);
        }

        public async Task<List<AcademicTermDTO>> GetAcademicTerm(long userId)
        {
            return await supportTicketRepository.GetAcademicTerm(userId);
        }

        public async Task<bool> SaveAcademicTerm(long id,string term, long yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveAcademicTerm(id, term, yearAcademic, fromDate, toDate, isActive, userId);
        }

        public async Task<List<AcademicClassDTO>> GetAcademicClass(long userId)
        {
            return await supportTicketRepository.GetAcademicClass(userId);
        }

        public async Task<bool> SaveAcademicClass(long id, string academicClass, long yearAcademic, long responsibleUserId, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveAcademicClass(id, academicClass, yearAcademic, responsibleUserId, isActive, userId);
        }

        public async Task<List<SubjectDTO>> GetSubject(long userId)
        {
            return await supportTicketRepository.GetSubject(userId);
        }

        public async Task<bool> SaveSubject(long id, string subject, string descriptionText, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveSubject(id, subject, descriptionText, isActive, userId);
        }

        public async Task<List<StudentEnrollDTO>> GetStudentEnroll(long classId, long academicYear, long userId)
        {
            return await supportTicketRepository.GetStudentEnroll(classId, academicYear, userId);
        }
        public async Task<bool> SaveStudentEnroll(long id, long yearAcademic, long classId, long studentId, bool isActive, long userId)
        {
            return await supportTicketRepository.SaveStudentEnroll(id, yearAcademic, classId, studentId, isActive, userId);
        }
    }
}
