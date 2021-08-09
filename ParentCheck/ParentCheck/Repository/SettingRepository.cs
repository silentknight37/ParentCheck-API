using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentCheck.Repository
{
    public class SettingRepository : ISettingRepository
    {
        private ParentCheckContext _parentcheckContext;

        public SettingRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }
        #region InstituteUsers
        public async Task<List<InstituteUserDTO>> GeInstituteUsers(long userId)
        {
            List<InstituteUserDTO> instituteUserDTOs = new List<InstituteUserDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var instituteUsers = await (from iu in _parentcheckContext.InstituteUser
                                            join r in _parentcheckContext.Role on iu.RoleId equals r.Id
                                            where iu.InstituteId == user.InstituteId
                                            select new
                                            {
                                                iu.Id,
                                                iu.FirstName,
                                                iu.LastName,
                                                iu.DateOfBirth,
                                                r.RoleText,
                                                iu.Username
                                            }).ToListAsync();

                foreach (var instituteUser in instituteUsers)
                {
                    instituteUserDTOs.Add(new InstituteUserDTO
                    {
                        UserId = instituteUser.Id,
                        Role = instituteUser.RoleText,
                        FirstName = instituteUser.FirstName,
                        LastName = instituteUser.LastName,
                        UserName = instituteUser.Username,
                        DateOfBirth = instituteUser.DateOfBirth
                    });
                }
            }

            return instituteUserDTOs;
        }

        public async Task<bool> SaveInstituteUser(long id, string firstName, string lastName, int roleId, long studentUserId, long parentUserid, long classTeacherUserId, long headTeacherUserId, long communicationGroup, string username, DateTime dateOfBirth, string password, bool isActive, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                try
                {
                    InstituteUser instituteUser;
                    if (id > 0)
                    {
                        var iUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == id);

                        if (iUser != null)
                        {
                            instituteUser = iUser;
                            instituteUser.Password = password;
                            instituteUser.ClassTeacherUserId = classTeacherUserId;
                            instituteUser.HeadTeacherUserId = headTeacherUserId;
                            instituteUser.CommunicationGroup = communicationGroup;
                            instituteUser.IsActive = isActive;
                            instituteUser.UpdateOn = DateTime.UtcNow;
                            instituteUser.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteUser).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteUser = new InstituteUser();

                    instituteUser.FirstName = firstName;
                    instituteUser.LastName = lastName;
                    instituteUser.Username = username;
                    instituteUser.DateOfBirth = dateOfBirth;
                    instituteUser.RoleId = roleId;
                    instituteUser.StudentUserId = studentUserId;
                    instituteUser.ParentUserid = parentUserid;
                    instituteUser.ClassTeacherUserId = classTeacherUserId;
                    instituteUser.HeadTeacherUserId = headTeacherUserId;
                    instituteUser.CommunicationGroup = communicationGroup;
                    instituteUser.IsActive = isActive;
                    instituteUser.CreatedOn = DateTime.UtcNow;
                    instituteUser.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteUser.UpdateOn = DateTime.UtcNow;
                    instituteUser.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteUser.Add(instituteUser);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion

        #region AcademicYear
        public async Task<List<AcademicDTO>> GetAcademicYear(long userId)
        {
            List<AcademicDTO> academicDTOs = new List<AcademicDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var academics = await (from a in _parentcheckContext.AcademicYear
                                       where a.InstituteId == user.InstituteId
                                       select new
                                       {
                                           a.Id,
                                           a.YearAcademic,
                                           a.FromDate,
                                           a.ToDate,
                                           a.IsActive
                                       }).ToListAsync();

                foreach (var academic in academics)
                {
                    academicDTOs.Add(new AcademicDTO
                    {
                        Id = academic.Id,
                        YearAcademic = academic.YearAcademic,
                        FromDate = academic.FromDate,
                        ToDate = academic.ToDate,
                        IsActive = academic.IsActive
                    });
                }
            }

            return academicDTOs;
        }

        public async Task<bool> SaveAcademicYear(long id, int yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                try
                {
                    AcademicYear academicYear;
                    if (id > 0)
                    {
                        var academic = _parentcheckContext.AcademicYear.FirstOrDefault(i => i.Id == id);

                        if (academic != null)
                        {
                            academicYear = academic;
                            academicYear.IsActive = isActive;
                            academicYear.UpdateOn = DateTime.UtcNow;
                            academicYear.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(academicYear).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    academicYear = new AcademicYear();

                    academicYear.YearAcademic = yearAcademic;
                    academicYear.InstituteId = user.InstituteId;
                    academicYear.FromDate = fromDate;
                    academicYear.ToDate = toDate;
                    academicYear.IsActive = isActive;
                    academicYear.CreatedOn = DateTime.UtcNow;
                    academicYear.CreatedBy = $"{user.FirstName} {user.LastName}";
                    academicYear.UpdateOn = DateTime.UtcNow;
                    academicYear.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.AcademicYear.Add(academicYear);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion

        #region AcademicTerm
        public async Task<List<AcademicTermDTO>> GetAcademicTerm(long userId)
        {
            List<AcademicTermDTO> academicTermDTOs = new List<AcademicTermDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var academicTerms = await (from t in _parentcheckContext.InstituteTerm
                                           join a in _parentcheckContext.AcademicYear on t.AcademicYearId equals a.Id
                                           where a.InstituteId == user.InstituteId
                                           select new
                                           {
                                               t.Id,
                                               t.Term,
                                               a.YearAcademic,
                                               t.FromDate,
                                               t.ToDate,
                                               t.IsActive
                                           }).ToListAsync();

                foreach (var academicTerm in academicTerms)
                {
                    academicTermDTOs.Add(new AcademicTermDTO
                    {
                        Id = academicTerm.Id,
                        Term = academicTerm.Term,
                        YearAcademic = academicTerm.YearAcademic,
                        FromDate = academicTerm.FromDate,
                        ToDate = academicTerm.ToDate,
                        IsActive = academicTerm.IsActive
                    });
                }
            }

            return academicTermDTOs;
        }

        public async Task<bool> SaveAcademicTerm(long id, string term, long yearAcademic, DateTime fromDate, DateTime toDate, bool isActive, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                try
                {
                    InstituteTerm academicTerm;
                    if (id > 0)
                    {
                        var instituteTerm = _parentcheckContext.InstituteTerm.FirstOrDefault(i => i.Id == id);

                        if (instituteTerm != null)
                        {
                            academicTerm = instituteTerm;
                            academicTerm.IsActive = isActive;
                            academicTerm.UpdateOn = DateTime.UtcNow;
                            academicTerm.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(academicTerm).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    academicTerm = new InstituteTerm();

                    academicTerm.Term = term;
                    academicTerm.AcademicYearId = yearAcademic;
                    academicTerm.InstituteId = user.InstituteId;
                    academicTerm.FromDate = fromDate;
                    academicTerm.ToDate = toDate;
                    academicTerm.IsActive = isActive;
                    academicTerm.CreatedOn = DateTime.UtcNow;
                    academicTerm.CreatedBy = $"{user.FirstName} {user.LastName}";
                    academicTerm.UpdateOn = DateTime.UtcNow;
                    academicTerm.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteTerm.Add(academicTerm);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion


        #region AcademicClass
        public async Task<List<AcademicClassDTO>> GetAcademicClass(long userId)
        {
            List<AcademicClassDTO> academicTermDTOs = new List<AcademicClassDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var academicClasses = await (from t in _parentcheckContext.InstituteClass
                                             join a in _parentcheckContext.AcademicYear on t.AcademicYearId equals a.Id
                                             join u in _parentcheckContext.InstituteUser on t.ResponsibleUserId equals u.Id
                                             where a.InstituteId == user.InstituteId
                                             select new
                                             {
                                                 t.Id,
                                                 t.Class,
                                                 a.YearAcademic,
                                                 u.FirstName,
                                                 u.LastName,
                                                 t.IsActive,
                                                 t.ResponsibleUserId
                                             }).ToListAsync();

                foreach (var academicClasse in academicClasses)
                {
                    academicTermDTOs.Add(new AcademicClassDTO
                    {
                        Id = academicClasse.Id,
                        Class = academicClasse.Class,
                        YearAcademic = academicClasse.YearAcademic,
                        ResponsibleUserId = academicClasse.ResponsibleUserId,
                        ResponsibleUser = $"{ academicClasse.FirstName} { academicClasse.LastName}",
                        IsActive = academicClasse.IsActive
                    });
                }
            }

            return academicTermDTOs;
        }

        public async Task<bool> SaveAcademicClass(long id, string academicClass, long yearAcademic, long responsibleUserId, bool isActive, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                try
                {
                    InstituteClass instituteClass;
                    if (id > 0)
                    {
                        var iClass = _parentcheckContext.InstituteClass.FirstOrDefault(i => i.Id == id);

                        if (iClass != null)
                        {
                            instituteClass = iClass;
                            instituteClass.IsActive = isActive;
                            instituteClass.ResponsibleUserId = responsibleUserId;
                            instituteClass.UpdateOn = DateTime.UtcNow;
                            instituteClass.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteClass).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteClass = new InstituteClass();

                    instituteClass.Class = academicClass;
                    instituteClass.AcademicYearId = yearAcademic;
                    instituteClass.InstituteId = user.InstituteId;
                    instituteClass.ResponsibleUserId = responsibleUserId;
                    instituteClass.IsActive = isActive;
                    instituteClass.CreatedOn = DateTime.UtcNow;
                    instituteClass.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteClass.UpdateOn = DateTime.UtcNow;
                    instituteClass.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteClass.Add(instituteClass);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion

        #region AcademicClass
        public async Task<List<SubjectDTO>> GetSubject(long userId)
        {
            List<SubjectDTO> subjectDTOs = new List<SubjectDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var subjects = await (from s in _parentcheckContext.InstituteSubject
                                      where s.InstituteId == user.InstituteId
                                      select new
                                      {
                                          s.Id,
                                          s.Subject,
                                          s.DescriptionText,
                                          s.IsActive
                                      }).ToListAsync();

                foreach (var subject in subjects)
                {
                    subjectDTOs.Add(new SubjectDTO
                    {
                        Id = subject.Id,
                        Subject = subject.Subject,
                        DescriptionText = subject.DescriptionText,
                        IsActive = subject.IsActive
                    });
                }
            }

            return subjectDTOs;
        }

        public async Task<bool> SaveSubject(long id, string subject, string descriptionText, bool isActive, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                try
                {
                    InstituteSubject instituteSubject;
                    if (id > 0)
                    {
                        var iSubject = _parentcheckContext.InstituteSubject.FirstOrDefault(i => i.Id == id);

                        if (iSubject != null)
                        {
                            instituteSubject = iSubject;
                            instituteSubject.IsActive = isActive;
                            instituteSubject.Subject = subject;
                            instituteSubject.DescriptionText = descriptionText;
                            instituteSubject.UpdateOn = DateTime.UtcNow;
                            instituteSubject.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteSubject).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteSubject = new InstituteSubject();

                    instituteSubject.Subject = subject;
                    instituteSubject.DescriptionText = descriptionText;
                    instituteSubject.InstituteId = user.InstituteId;
                    instituteSubject.IsActive = isActive;
                    instituteSubject.CreatedOn = DateTime.UtcNow;
                    instituteSubject.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteSubject.UpdateOn = DateTime.UtcNow;
                    instituteSubject.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteSubject.Add(instituteSubject);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion


        #region StudentEnroll
        public async Task<List<StudentEnrollDTO>> GetStudentEnroll(long classId,long academicYear, long userId)
        {
            List<StudentEnrollDTO> studentEnrollDTOs = new List<StudentEnrollDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var studentEnrolls = await (from ic in _parentcheckContext.InstituteUserClass
                                       where (classId==0 || ic.InstituteClassId== classId) &&
                                       (academicYear==0 || ic.AcademicYearId== academicYear)
                                       select new
                                       {
                                           ic.Id,
                                           ic.InstituteUserId,
                                           ic.InstituteClassId,
                                           ic.AcademicYearId,
                                           ic.IsActive
                                       }).ToListAsync();

                foreach (var studentEnroll in studentEnrolls)
                {
                    var acadamicYear = _parentcheckContext.AcademicYear.FirstOrDefault(i => i.Id == studentEnroll.AcademicYearId);
                    var acadamicClass = _parentcheckContext.InstituteClass.FirstOrDefault(i => i.Id == studentEnroll.InstituteClassId);
                    var instituteUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == studentEnroll.InstituteUserId);

                    studentEnrollDTOs.Add(new StudentEnrollDTO
                    {
                        Id = studentEnroll.Id,
                        ClassId= studentEnroll.InstituteClassId,
                        ClassName= acadamicClass.Class,
                        StudentUserId = studentEnroll.InstituteUserId,
                        StudentUserName= $"{instituteUser.FirstName} {instituteUser.LastName}",
                        IsActive = studentEnroll.IsActive,
                        AcademicYearId= studentEnroll.AcademicYearId
                    });
                }
            }

            return studentEnrollDTOs;
        }

        public async Task<bool> SaveStudentEnroll(long id, long yearAcademic, long classId, long studentId, bool isActive, long userId)
        {
            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                try
                {
                    InstituteUserClass instituteUserClass;
                    if (id > 0)
                    {
                        var userClass = _parentcheckContext.InstituteUserClass.FirstOrDefault(i => i.Id == id);

                        if (userClass != null)
                        {
                            instituteUserClass = userClass;
                            instituteUserClass.InstituteClassId = classId;
                            instituteUserClass.IsActive = isActive;
                            instituteUserClass.UpdateOn = DateTime.UtcNow;
                            instituteUserClass.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteUserClass).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteUserClass = new InstituteUserClass();

                    instituteUserClass.AcademicYearId = yearAcademic;
                    instituteUserClass.InstituteClassId = classId;
                    instituteUserClass.InstituteUserId = studentId;
                    instituteUserClass.IsActive = isActive;
                    instituteUserClass.CreatedOn = DateTime.UtcNow;
                    instituteUserClass.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteUserClass.UpdateOn = DateTime.UtcNow;
                    instituteUserClass.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteUserClass.Add(instituteUserClass);
                    await _parentcheckContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }
        #endregion
    }
}
