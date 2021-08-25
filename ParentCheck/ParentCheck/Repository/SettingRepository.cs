using Microsoft.EntityFrameworkCore;
using ParentCheck.BusinessObject;
using ParentCheck.Common;
using ParentCheck.Data;
using ParentCheck.Repository.Intreface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public async Task<List<InstituteUserDTO>> GeInstituteUsers(string searchValue, int? roleId, long userId)
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
                                            where iu.InstituteId == user.InstituteId &&
                                           (string.IsNullOrEmpty(searchValue) || (iu.FileName.Contains(searchValue)) || (iu.LastName.Contains(searchValue)) || (iu.Username.Contains(searchValue)) || (iu.DateOfBirth.ToString().Contains(searchValue)) || (iu.IndexNo.Contains(searchValue))) &&
                                            ((roleId==null && iu.RoleId!=(int)EnumRole.Parent) || (iu.RoleId== roleId))
                                            select new
                                            {
                                                iu.Id,
                                                iu.FirstName,
                                                iu.LastName,
                                                iu.DateOfBirth,
                                                iu.IsActive,
                                                iu.IndexNo,
                                                r.RoleText,
                                                iu.Username,
                                                iu.RoleId,
                                                iu.ParentUserid
                                            }).ToListAsync();

                foreach (var instituteUser in instituteUsers)
                {
                    var iUser = new InstituteUserDTO
                    {
                        UserId = instituteUser.Id,
                        Role = instituteUser.RoleText,
                        RoleId = instituteUser.RoleId,
                        FirstName = instituteUser.FirstName,
                        LastName = instituteUser.LastName,
                        UserName = instituteUser.Username,
                        DateOfBirth = instituteUser.DateOfBirth,
                        Admission = instituteUser.IndexNo,
                        IsActive = instituteUser.IsActive
                    };

                    var uContacts = await (from uc in _parentcheckContext.UserContact 
                                           where uc.InstituteUserId== instituteUser.Id &&  uc.IsPrimary
                                           && uc.IsActive
                                           select new
                                           {
                                               uc.ContactTypeId,
                                               uc.ContactValue
                                           }).ToListAsync();

                    foreach (var uContact in uContacts)
                    {
                        if (uContact.ContactTypeId == (int)EnumContactType.Mobile)
                        {
                            iUser.Mobile = uContact.ContactValue;
                        }
                    }

                    if (instituteUser.RoleId == (int)EnumRole.Student)
                    {
                        var parentInstituteUser = await _parentcheckContext.InstituteUser.FirstOrDefaultAsync(i => i.Id == instituteUser.ParentUserid && i.IsActive);
                        if (parentInstituteUser != null)
                        {
                            iUser.ParentId = parentInstituteUser.Id;
                            iUser.ParentFirstName = parentInstituteUser.FirstName;
                            iUser.ParentLastName = parentInstituteUser.LastName;
                            iUser.ParentUsername = parentInstituteUser.Username;
                            iUser.ParentDateOfBirth = parentInstituteUser.DateOfBirth;

                            var parentUContacts = await (from uc in _parentcheckContext.UserContact
                                                   where uc.InstituteUserId == parentInstituteUser.Id && uc.IsPrimary
                                                   && uc.IsActive
                                                   select new
                                                   {
                                                       uc.ContactTypeId,
                                                       uc.ContactValue
                                                   }).ToListAsync();

                            foreach (var parentUContact in parentUContacts)
                            {
                                if (parentUContact.ContactTypeId == (int)EnumContactType.Mobile)
                                {
                                    iUser.ParentMobile = parentUContact.ContactValue;
                                }
                            }
                        }
                    }

                    instituteUserDTOs.Add(iUser);
                }
            }

            return instituteUserDTOs;
        }

        public async Task<bool> SaveInstituteUser(
            long id,
            string firstName,
            string lastName,
            int roleId,
            string username,
            DateTime dateOfBirth,
            string mobile,
            string admission,
            string password,
            long parentId,
            string parentFirstName,
            string parentLastName,
            string parentUsername,
            DateTime? parentDateOfBirth,
            string parentMobile,
            string parentPassword,
            bool isActive,
            long userId)
        {
            var user = await _parentcheckContext.InstituteUser.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                try
                {
                    var iUserId = await SaveUsers(id, firstName, lastName, roleId, username, dateOfBirth, mobile, admission, password, isActive, user);

                    if (roleId == (int)EnumRole.Student)
                    {
                        var parentUserId=await SaveUsers(parentId, parentFirstName, parentLastName, (int)EnumRole.Parent, parentUsername, parentDateOfBirth.Value, parentMobile, admission, parentPassword, isActive, user);
                        if (parentUserId > 0)
                        {
                            var updateStudent = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == iUserId);
                            if (updateStudent != null)
                            {
                                updateStudent.ParentUserid = parentUserId;
                                _parentcheckContext.Entry(updateStudent).State = EntityState.Modified;
                                await _parentcheckContext.SaveChangesAsync();
                            }
                        }
                    }
                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> ResetPassword(
           long id, string password,long userId)
        {
            var user = await _parentcheckContext.InstituteUser.FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                try
                {
                    if (id > 0)
                    {
                        var iUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == id);
                        if (iUser != null)
                        {
                            iUser.Password = password;
                            iUser.UpdateOn = DateTime.UtcNow;
                            iUser.UpdatedBy = $"{user.FirstName} {user.LastName}";
                            _parentcheckContext.Entry(iUser).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        private async Task<long> SaveUsers(
            long id,
            string firstName,
            string lastName,
            int roleId,
            string username,
            DateTime dateOfBirth,
            string mobile,
            string admission,
            string password,
            bool isActive,
            InstituteUser user)
        {
            InstituteUser instituteUser;
            if (id > 0)
            {
                var iUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == id);

                if (iUser != null)
                {
                    instituteUser = iUser;
                    if (!string.IsNullOrEmpty(password))
                    {
                        instituteUser.Password = password;
                    }
                    instituteUser.FirstName = firstName;
                    instituteUser.LastName = lastName;
                    instituteUser.Username = username;
                    instituteUser.DateOfBirth = dateOfBirth;
                    instituteUser.IndexNo = admission;
                    instituteUser.RoleId = roleId;
                    instituteUser.CommunicationGroup = GetCommunicationGroup(roleId);
                    instituteUser.IsActive = isActive;
                    instituteUser.UpdateOn = DateTime.UtcNow;
                    instituteUser.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.Entry(instituteUser).State = EntityState.Modified;
                    await _parentcheckContext.SaveChangesAsync();

                    var userMobileContact = await _parentcheckContext.UserContact.FirstOrDefaultAsync(i => i.InstituteUserId == instituteUser.Id && i.ContactTypeId == (int)EnumContactType.Mobile);
                    if (userMobileContact != null)
                    {
                        userMobileContact.ContactValue = mobile;
                        _parentcheckContext.Entry(userMobileContact).State = EntityState.Modified;
                        await _parentcheckContext.SaveChangesAsync();
                    }
                    else
                    {
                        UserContact userContact = new UserContact();
                        userContact.ContactTypeId = (int)EnumContactType.Mobile;
                        userContact.ContactValue = mobile;
                        userContact.IsPrimary = true;
                        userContact.IsActive = true;
                        userContact.InstituteUserId = instituteUser.Id;
                        userContact.CreatedOn = DateTime.UtcNow;
                        userContact.CreatedBy = $"{user.FirstName} {user.LastName}";
                        userContact.UpdateOn = DateTime.UtcNow;
                        userContact.UpdatedBy = $"{user.FirstName} {user.LastName}";

                        _parentcheckContext.UserContact.Add(userContact);
                        await _parentcheckContext.SaveChangesAsync();
                    }

                    var userEmailContact = await _parentcheckContext.UserContact.FirstOrDefaultAsync(i => i.InstituteUserId == instituteUser.Id && i.ContactTypeId == (int)EnumContactType.Email);
                    if (userEmailContact != null)
                    {
                        userEmailContact.ContactValue = username;
                        _parentcheckContext.Entry(userEmailContact).State = EntityState.Modified;
                        await _parentcheckContext.SaveChangesAsync();
                    }
                    else
                    {
                        UserContact userContact = new UserContact();
                        userContact.ContactTypeId = (int)EnumContactType.Email;
                        userContact.ContactValue = username;
                        userContact.IsPrimary = true;
                        userContact.IsActive = true;
                        userContact.InstituteUserId = instituteUser.Id;
                        userContact.CreatedOn = DateTime.UtcNow;
                        userContact.CreatedBy = $"{user.FirstName} {user.LastName}";
                        userContact.UpdateOn = DateTime.UtcNow;
                        userContact.UpdatedBy = $"{user.FirstName} {user.LastName}";

                        _parentcheckContext.UserContact.Add(userContact);
                        await _parentcheckContext.SaveChangesAsync();
                    }

                    return instituteUser.Id;
                }
            }

            instituteUser = new InstituteUser();

            instituteUser.FirstName = firstName;
            instituteUser.LastName = lastName;
            instituteUser.Username = username;
            instituteUser.DateOfBirth = dateOfBirth;
            instituteUser.InstituteId = user.InstituteId;
            instituteUser.IndexNo = admission;
            instituteUser.RoleId = roleId;
            instituteUser.Password = password;
            instituteUser.ImageUrl = $"http://storage.parentcheck.lk/profile/default.jpg";
            instituteUser.CommunicationGroup = GetCommunicationGroup(roleId);
            instituteUser.IsActive = isActive;
            instituteUser.CreatedOn = DateTime.UtcNow;
            instituteUser.CreatedBy = $"{user.FirstName} {user.LastName}";
            instituteUser.UpdateOn = DateTime.UtcNow;
            instituteUser.UpdatedBy = $"{user.FirstName} {user.LastName}";

            _parentcheckContext.InstituteUser.Add(instituteUser);
            await _parentcheckContext.SaveChangesAsync();

            var newUserMobileContact = await _parentcheckContext.UserContact.FirstOrDefaultAsync(i => i.InstituteUserId == instituteUser.Id && i.ContactTypeId == (int)EnumContactType.Mobile);
            if (newUserMobileContact == null)
            {
                UserContact userContact = new UserContact();
                userContact.ContactTypeId = (int)EnumContactType.Mobile;
                userContact.ContactValue = mobile;
                userContact.IsPrimary = true;
                userContact.IsActive = true;
                userContact.InstituteUserId = instituteUser.Id;
                userContact.CreatedOn = DateTime.UtcNow;
                userContact.CreatedBy = $"{user.FirstName} {user.LastName}";
                userContact.UpdateOn = DateTime.UtcNow;
                userContact.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.UserContact.Add(userContact);
                await _parentcheckContext.SaveChangesAsync();
            }

            var newUserEmailContact = await _parentcheckContext.UserContact.FirstOrDefaultAsync(i => i.InstituteUserId == instituteUser.Id && i.ContactTypeId == (int)EnumContactType.Email);
            if (newUserEmailContact == null)
            {
                UserContact userContact = new UserContact();
                userContact.ContactTypeId = (int)EnumContactType.Email;
                userContact.ContactValue = username;
                userContact.IsPrimary = true;
                userContact.IsActive = true;
                userContact.InstituteUserId = instituteUser.Id;
                userContact.CreatedOn = DateTime.UtcNow;
                userContact.CreatedBy = $"{user.FirstName} {user.LastName}";
                userContact.UpdateOn = DateTime.UtcNow;
                userContact.UpdatedBy = $"{user.FirstName} {user.LastName}";

                _parentcheckContext.UserContact.Add(userContact);
                await _parentcheckContext.SaveChangesAsync();
            }

            return instituteUser.Id;
        }

        public async Task<bool> SaveDriviceToken(string deviceToken, long userId)
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
                    var iUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == userId);

                    if (iUser != null)
                    {
                        instituteUser = iUser;
                        instituteUser.DeviceToken = deviceToken;
                        instituteUser.UpdateOn = DateTime.UtcNow;
                        instituteUser.UpdatedBy = $"{user.FirstName} {user.LastName}";

                        _parentcheckContext.Entry(instituteUser).State = EntityState.Modified;
                        await _parentcheckContext.SaveChangesAsync();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }
        public async Task<bool> UploadProfileImage(string encryptedFileName, string uploadPath, string fileName, long userId)
        {
            var user = _parentcheckContext.InstituteUser.Where(i => i.Id == userId).FirstOrDefault();

            if (user != null)
            {
                try
                {
                    InstituteUser instituteUser;
                    var iUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == userId);

                    if (iUser != null)
                    {
                        instituteUser = iUser;
                        instituteUser.ImageUrl = $"https://storage.parentcheck.lk/{uploadPath}/{encryptedFileName}";
                        instituteUser.FileName = fileName;
                        instituteUser.EncryptedFileName = encryptedFileName;
                        instituteUser.UpdateOn = DateTime.UtcNow;
                        instituteUser.UpdatedBy = $"{user.FirstName} {user.LastName}";

                        _parentcheckContext.Entry(instituteUser).State = EntityState.Modified;
                        await _parentcheckContext.SaveChangesAsync();
                        return true;
                    }
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<List<PerformanceDTO>> GetPerformance(long userId)
        {
            List<PerformanceDTO> performanceDTOs = new List<PerformanceDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where (u.ParentUserid == userId || u.Id == userId)
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.RoleId
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.RoleId == (int)EnumRole.Student)
                {

                    var userActiveClass = await (from cu in _parentcheckContext.InstituteUserClass
                                                 join c in _parentcheckContext.InstituteClass on cu.InstituteClassId equals c.Id
                                                 join ay in _parentcheckContext.AcademicYear on cu.AcademicYearId equals ay.Id
                                                 where cu.InstituteUserId == user.Id && ay.FromDate <= DateTime.Now && ay.ToDate >= DateTime.Now
                                                 select new
                                                 {
                                                     cu.InstituteClassId,
                                                     c.Class
                                                 }).FirstOrDefaultAsync();

                    if (userActiveClass != null)
                    {
                        List<long> assignmentIds = new List<long>();
                        List<long> submitAssignmentIds = new List<long>();

                        var instituteChapterTopicAssignments = await (from ct in _parentcheckContext.InstituteChapterTopic
                                                                      join sc in _parentcheckContext.InstituteSubjectChapter on ct.InstituteSubjectChapterId equals sc.Id
                                                                      join cs in _parentcheckContext.InstituteClassSubject on sc.InstituteClassSubjectId equals cs.Id
                                                                      join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                                                      where cs.InstituteClassId == userActiveClass.InstituteClassId && ct.InstituteAssignmentId.HasValue
                                                                      select new
                                                                      {
                                                                          ct.InstituteAssignmentId
                                                                      }).ToListAsync();

                        foreach (var instituteChapterTopicAssignment in instituteChapterTopicAssignments)
                        {
                            assignmentIds.Add(instituteChapterTopicAssignment.InstituteAssignmentId.Value);

                            var instituteChapterTopicSubmitAssignments = await (from ias in _parentcheckContext.InstituteAssignmentSubmission
                                                                                where ias.SubmitUserId == user.Id && ias.InstituteAssignmentId == instituteChapterTopicAssignment.InstituteAssignmentId && ias.StatusId == (int)EnumStatus.Completed
                                                                                select new
                                                                                {
                                                                                    ias.InstituteAssignmentId
                                                                                }).ToListAsync();

                            foreach (var instituteChapterTopicSubmitAssignment in instituteChapterTopicSubmitAssignments)
                            {
                                submitAssignmentIds.Add(instituteChapterTopicSubmitAssignment.InstituteAssignmentId);
                            }
                        }

                        var instituteSubjectChapterAssignments = await (from sc in _parentcheckContext.InstituteSubjectChapter
                                                                        join cs in _parentcheckContext.InstituteClassSubject on sc.InstituteClassSubjectId equals cs.Id
                                                                        join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                                                        where cs.InstituteClassId == userActiveClass.InstituteClassId && sc.InstituteAssignmentId.HasValue
                                                                        select new
                                                                        {
                                                                            sc.InstituteAssignmentId
                                                                        }).ToListAsync();

                        foreach (var instituteSubjectChapterAssignment in instituteSubjectChapterAssignments)
                        {
                            assignmentIds.Add(instituteSubjectChapterAssignment.InstituteAssignmentId.Value);

                            var instituteSubjectChapterSubmitAssignments = await (from ias in _parentcheckContext.InstituteAssignmentSubmission
                                                                                  where ias.SubmitUserId == user.Id && ias.InstituteAssignmentId == instituteSubjectChapterAssignment.InstituteAssignmentId && ias.StatusId == (int)EnumStatus.Completed
                                                                                  select new
                                                                                  {
                                                                                      ias.InstituteAssignmentId
                                                                                  }).ToListAsync();

                            foreach (var instituteSubjectChapterSubmitAssignment in instituteSubjectChapterSubmitAssignments)
                            {
                                submitAssignmentIds.Add(instituteSubjectChapterSubmitAssignment.InstituteAssignmentId);
                            }
                        }



                        var totalCount = (double)assignmentIds.Count;
                        var completeCount = (double)submitAssignmentIds.Count;
                        var nonCompleteCount = (double)(totalCount - completeCount);
                        var presentage = (int)Math.Round((double)(completeCount / totalCount) * 100);

                        performanceDTOs.Add(new PerformanceDTO
                        {
                            PerformanceType = "Assignment",
                            Completed = completeCount.ToString(),
                            NotComplete = nonCompleteCount.ToString(),
                            Percentage = presentage.ToString()
                        });
                    }
                }
                else if (user.RoleId == (int)EnumRole.Administrator)
                {
                    List<long> totalStudentCount = new List<long>();
                    List<long> todalAttendanceCount = new List<long>();
                    var totalStudents = await (from sa in _parentcheckContext.InstituteUserClass
                                                        join c in _parentcheckContext.InstituteClass on sa.InstituteClassId equals c.Id
                                                        where c.InstituteId == user.InstituteId
                                                        select new
                                                        {
                                                            sa.InstituteUserId
                                                        }).ToListAsync();

                    foreach (var totalStudent in totalStudents)
                    {
                        totalStudentCount.Add(totalStudent.InstituteUserId);
                    }

                    var studentTodayAttendances = await (from sa in _parentcheckContext.StudentAttendance
                                                        join c in _parentcheckContext.InstituteClass on sa.InstituteClassId equals c.Id
                                                        where sa.RecordDate.Date == DateTime.Now.Date
                                                        && c.InstituteId == user.InstituteId
                                                        && sa.IsAttendance == true
                                                        select new
                                                        {
                                                            sa.InstituteUserId
                                                        }).ToListAsync();
                    foreach (var studentTodayAttendance in studentTodayAttendances)
                    {
                        todalAttendanceCount.Add(studentTodayAttendance.InstituteUserId);
                    }
                    var totalCount = (double)totalStudentCount.Count;
                    var completeCount = (double)todalAttendanceCount.Count;
                    var nonCompleteCount = (double)(totalCount - completeCount);
                    var presentage = (int)Math.Round((double)(completeCount / totalCount) * 100);
                    performanceDTOs.Add(new PerformanceDTO
                    {
                        PerformanceType = "Total Attendance",
                        Completed = completeCount.ToString(),
                        NotComplete = nonCompleteCount.ToString(),
                        Percentage = presentage.ToString()
                    });
                }
            }

            return performanceDTOs;
        }

        private long GetCommunicationGroup(int roleId)
        {
            if (roleId == (int)EnumRole.Student)
            {
                return (int)EnumCommunicationGroup.Student;
            }

            if (roleId == (int)EnumRole.Parent)
            {
                return (int)EnumCommunicationGroup.Parent;
            }
            if (roleId == (int)EnumRole.Staff)
            {
                return (int)EnumCommunicationGroup.Staff;
            }
            return 0;
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
                                       orderby a.YearAcademic,a.FromDate,a.ToDate
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
                            academicYear.YearAcademic = yearAcademic;
                            academicYear.IsActive = isActive;
                            academicYear.FromDate = fromDate;
                            academicYear.ToDate = toDate;
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
                                           orderby t.Term
                                           where a.InstituteId == user.InstituteId
                                           select new
                                           {
                                               t.Id,
                                               t.Term,
                                               t.AcademicYearId,
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
                        YearAcademicId = academicTerm.AcademicYearId,
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
                            academicTerm.Term = term;
                            academicTerm.AcademicYearId = yearAcademic;
                            academicTerm.FromDate = fromDate;
                            academicTerm.ToDate = toDate;
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
                                             orderby t.Class
                                             where a.InstituteId == user.InstituteId
                                             select new
                                             {
                                                 t.Id,
                                                 t.Class,
                                                 a.YearAcademic,
                                                 u.FirstName,
                                                 u.LastName,
                                                 t.IsActive,
                                                 t.ResponsibleUserId,
                                                 a.FromDate,
                                                 a.ToDate,
                                                 t.AcademicYearId
                                             }).ToListAsync();

                foreach (var academicClasse in academicClasses)
                {
                    academicTermDTOs.Add(new AcademicClassDTO
                    {
                        Id = academicClasse.Id,
                        Class = academicClasse.Class,
                        YearAcademic = academicClasse.YearAcademic,
                        YearAcademicId = academicClasse.AcademicYearId,
                        YearAcademicDetail = $"{academicClasse.YearAcademic.ToString()} {academicClasse.FromDate.ToString("dd/MM/yyyy")} - {academicClasse.ToDate.ToString("dd/MM/yyyy")}",
                        ResponsibleUserId = academicClasse.ResponsibleUserId,
                        ResponsibleUser = $"{ academicClasse.FirstName} { academicClasse.LastName}",
                        IsActive = academicClasse.IsActive
                    });
                }
            }

            return academicTermDTOs;
        }

        public async Task<AcademicClassDTO> GetAcademicClass(long yearAcademic,string academicClass, long userId)
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
                var aClass = await (from t in _parentcheckContext.InstituteClass
                                             join a in _parentcheckContext.AcademicYear on t.AcademicYearId equals a.Id
                                             join u in _parentcheckContext.InstituteUser on t.ResponsibleUserId equals u.Id
                                             where a.InstituteId == user.InstituteId && t.Class==academicClass && t.AcademicYearId== yearAcademic
                                             select new
                                             {
                                                 t.Id,
                                                 t.Class,
                                                 a.YearAcademic,
                                                 u.FirstName,
                                                 u.LastName,
                                                 t.IsActive,
                                                 t.ResponsibleUserId
                                             }).FirstOrDefaultAsync();

                if(aClass != null)
                {
                    return new AcademicClassDTO
                    {
                        Id = aClass.Id,
                        Class = aClass.Class,
                        YearAcademic = aClass.YearAcademic,
                        ResponsibleUserId = aClass.ResponsibleUserId,
                        ResponsibleUser = $"{ aClass.FirstName} { aClass.LastName}",
                        IsActive = aClass.IsActive
                    };
                }
            }

            return null;
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
                            instituteClass.Class = academicClass;
                            instituteClass.AcademicYearId = yearAcademic;
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

        #region AcademicSubject
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
                                      orderby s.Subject
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

        public async Task<SubjectDTO> GetSubject(string subject, long userId)
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
                var aSubject = await (from s in _parentcheckContext.InstituteSubject
                                      where s.InstituteId == user.InstituteId &&
                                      s.Subject==subject
                                      select new
                                      {
                                          s.Id,
                                          s.Subject,
                                          s.DescriptionText,
                                          s.IsActive
                                      }).FirstOrDefaultAsync();

                if (aSubject != null)
                {
                    return new SubjectDTO
                    {
                        Id = aSubject.Id,
                        Subject = aSubject.Subject,
                        DescriptionText = aSubject.DescriptionText,
                        IsActive = aSubject.IsActive
                    };
                }
            }

            return null;
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

        public async Task<StudentEnrollDTO> GetStudentEnroll(long classId, long studentId, long academicYear, long userId)
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
                var studentEnroll = await (from ic in _parentcheckContext.InstituteUserClass
                                            where ic.InstituteClassId == classId &&
                                            ic.AcademicYearId == academicYear &&
                                            ic.InstituteUserId== studentId
                                            select new
                                            {
                                                ic.Id,
                                                ic.InstituteUserId,
                                                ic.InstituteClassId,
                                                ic.AcademicYearId,
                                                ic.IsActive
                                            }).FirstOrDefaultAsync();

                if(studentEnroll!=null)
                {
                    var acadamicYear = _parentcheckContext.AcademicYear.FirstOrDefault(i => i.Id == studentEnroll.AcademicYearId);
                    var acadamicClass = _parentcheckContext.InstituteClass.FirstOrDefault(i => i.Id == studentEnroll.InstituteClassId);
                    var instituteUser = _parentcheckContext.InstituteUser.FirstOrDefault(i => i.Id == studentEnroll.InstituteUserId);

                    return new StudentEnrollDTO
                    {
                        Id = studentEnroll.Id,
                        ClassId = studentEnroll.InstituteClassId,
                        ClassName = acadamicClass.Class,
                        StudentUserId = studentEnroll.InstituteUserId,
                        StudentUserName = $"{instituteUser.FirstName} {instituteUser.LastName}",
                        IsActive = studentEnroll.IsActive,
                        AcademicYearId = studentEnroll.AcademicYearId
                    };
                }
            }

            return null;
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

        #region ClassSubject
        public async Task<List<AcademicClassSubjectDTO>> GetClassSubject(long classId,long userId)
        {
            List<AcademicClassSubjectDTO> academicClassSubjectDTOs = new List<AcademicClassSubjectDTO>();

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
                var classSubjects = await (from cs in _parentcheckContext.InstituteClassSubject
                                       join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                       join u in _parentcheckContext.InstituteUser on cs.ResponsibleUserId equals u.Id
                                       orderby s.Subject
                                       where cs.InstituteClassId == classId
                                       select new
                                       {
                                           cs.Id,
                                           cs.InstituteClassId,
                                           cs.InstituteSubjectId,
                                           cs.ResponsibleUserId,
                                           s.Subject,
                                           cs.BgColor,
                                           cs.FontColor,
                                           u.FirstName,
                                           u.LastName,
                                           cs.IsActive
                                       }).ToListAsync();

                foreach (var classSubject in classSubjects)
                {
                    academicClassSubjectDTOs.Add(new AcademicClassSubjectDTO
                    {
                        Id = classSubject.Id,
                        Subject= classSubject.Subject,
                        SubjectId= classSubject.InstituteSubjectId,
                        ResponsibleUserId = classSubject.ResponsibleUserId,
                        ResponsibleUser = $"{ classSubject.FirstName} { classSubject.LastName}",
                        IsActive = classSubject.IsActive
                    });
                }
            }

            return academicClassSubjectDTOs;
        }

        public async Task<bool> SaveClassSubject(long id, long classId, long subjectId, long responsibleUserId, bool isActive, long userId)
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
                    InstituteClassSubject instituteClassSubject;
                    if (id > 0)
                    {
                        var classSubjects = _parentcheckContext.InstituteClassSubject.FirstOrDefault(i => i.Id == id);

                        if (classSubjects != null)
                        {
                            instituteClassSubject = classSubjects;
                            instituteClassSubject.InstituteSubjectId = subjectId;
                            instituteClassSubject.ResponsibleUserId = responsibleUserId;
                            instituteClassSubject.IsActive = isActive;
                            instituteClassSubject.UpdateOn = DateTime.UtcNow;
                            instituteClassSubject.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteClassSubject).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteClassSubject = new InstituteClassSubject();

                    instituteClassSubject.InstituteClassId = classId;
                    instituteClassSubject.InstituteSubjectId = subjectId;
                    instituteClassSubject.ResponsibleUserId = responsibleUserId;
                    instituteClassSubject.BgColor = "#4287f5";
                    instituteClassSubject.FontColor = "#ffffff";
                    instituteClassSubject.IsActive = isActive;
                    instituteClassSubject.CreatedOn = DateTime.UtcNow;
                    instituteClassSubject.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteClassSubject.UpdateOn = DateTime.UtcNow;
                    instituteClassSubject.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteClassSubject.Add(instituteClassSubject);
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

        #region ClassSubject
        public async Task<List<SubjectChapterDTO>> GetSubjectChapter(long subjectId, long userId)
        {
            List<SubjectChapterDTO> subjectChapterDTOs = new List<SubjectChapterDTO>();

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
                var subjectChapters = await (from sc in _parentcheckContext.InstituteSubjectChapter
                                           where sc.InstituteClassSubjectId==subjectId
                                           select new
                                           {
                                               sc.Id,
                                               sc.Chapter,
                                               sc.IsActive,
                                               sc.InstituteAssignmentId
                                           }).ToListAsync();

                foreach (var subjectChapter in subjectChapters)
                {
                    subjectChapterDTOs.Add(new SubjectChapterDTO
                    {
                        InstituteSubjectChapterId = subjectChapter.Id,
                        Chapter= subjectChapter.Chapter,
                        InstituteAssignmentId= subjectChapter.InstituteAssignmentId,
                        IsAssignmentAssigned= subjectChapter.InstituteAssignmentId.HasValue,
                        IsActive = subjectChapter.IsActive
                    });
                }
            }

            return subjectChapterDTOs;
        }

        public async Task<bool> SaveSubjectChapter(long id, long subjectId, string chapter, bool isActive, long userId)
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
                    InstituteSubjectChapter instituteSubjectChapter;
                    if (id > 0)
                    {
                        var subjectChapter = _parentcheckContext.InstituteSubjectChapter.FirstOrDefault(i => i.Id == id);

                        if (subjectChapter != null)
                        {
                            instituteSubjectChapter = subjectChapter;
                            instituteSubjectChapter.IsActive = isActive;
                            instituteSubjectChapter.Chapter = chapter;
                            instituteSubjectChapter.UpdateOn = DateTime.UtcNow;
                            instituteSubjectChapter.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteSubjectChapter).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteSubjectChapter = new InstituteSubjectChapter();

                    instituteSubjectChapter.Chapter = chapter;
                    instituteSubjectChapter.InstituteClassSubjectId = subjectId;
                    instituteSubjectChapter.IsActive = isActive;
                    instituteSubjectChapter.CreatedOn = DateTime.UtcNow;
                    instituteSubjectChapter.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteSubjectChapter.UpdateOn = DateTime.UtcNow;
                    instituteSubjectChapter.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteSubjectChapter.Add(instituteSubjectChapter);
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

        #region ChapterTopic
        public async Task<List<ChapterTopicsDTO>> GetChapterTopic(long chapterId, long userId)
        {
            List<ChapterTopicsDTO> chapterTopicsDTOs = new List<ChapterTopicsDTO>();

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
                var chapterTopices = await (from ct in _parentcheckContext.InstituteChapterTopic
                                            where ct.InstituteSubjectChapterId == chapterId
                                             orderby ct.CreatedOn descending
                                             select new
                                             {
                                                 ct.Id,
                                                 ct.Topic,
                                                 ct.DescriptionText,
                                                 ct.IsActive,
                                                 ct.InstituteAssignmentId,
                                                 ct.CreatedOn
                                             }).ToListAsync();

                foreach (var chapterTopice in chapterTopices)
                {
                    chapterTopicsDTOs.Add(new ChapterTopicsDTO
                    {
                        InstituteChapterTopicId = chapterTopice.Id,
                        Topic = chapterTopice.Topic,
                        Description = chapterTopice.DescriptionText,
                        SubmitDate = chapterTopice.CreatedOn.Value.ToShortDateString(),
                        InstituteAssignmentId = chapterTopice.InstituteAssignmentId,
                        IsAssignmentAssigned = chapterTopice.InstituteAssignmentId.HasValue,
                        IsActive = chapterTopice.IsActive
                    });
                }
            }

            return chapterTopicsDTOs;
        }

        public async Task<bool> SaveChapterTopic(long id, long chapterId, string topic, string description, bool isActive, long userId)
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
                    InstituteChapterTopic instituteChapterTopic;
                    if (id > 0)
                    {
                        var chapterTopic = _parentcheckContext.InstituteChapterTopic.FirstOrDefault(i => i.Id == id);

                        if (chapterTopic != null)
                        {
                            instituteChapterTopic = chapterTopic;
                            instituteChapterTopic.IsActive = isActive;
                            instituteChapterTopic.Topic = topic;
                            instituteChapterTopic.DescriptionText = description;
                            instituteChapterTopic.UpdateOn = DateTime.UtcNow;
                            instituteChapterTopic.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteChapterTopic).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteChapterTopic = new InstituteChapterTopic();

                    instituteChapterTopic.Topic = topic;
                    instituteChapterTopic.DescriptionText = description;
                    instituteChapterTopic.InstituteSubjectChapterId = chapterId;
                    instituteChapterTopic.IsActive = isActive;
                    instituteChapterTopic.CreatedOn = DateTime.UtcNow;
                    instituteChapterTopic.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteChapterTopic.UpdateOn = DateTime.UtcNow;
                    instituteChapterTopic.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteChapterTopic.Add(instituteChapterTopic);
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


        #region TopicContent
        public async Task<List<TopicContentDTO>> GetTopicContent(long topicId, long userId)
        {
            List<TopicContentDTO> topicContentDTOs = new List<TopicContentDTO>();

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
                var topicContents = await (from tc in _parentcheckContext.InstituteTopicContent
                                           join ct in _parentcheckContext.ContentType on tc.ContentTypeId equals ct.Id
                                           where tc.InstituteChapterTopicId == topicId
                                           orderby tc.ContentOrder
                                           select new
                                           {
                                               tc.Id,
                                               tc.ContentOrder,
                                               tc.ContentText,
                                               tc.ContentType,
                                               tc.ContentTypeId,
                                               tc.IsActive,
                                               ct.TypeText
                                           }).ToListAsync();

                foreach (var topicContent in topicContents)
                {
                    var content = new TopicContentDTO
                    {
                        InstituteTopicContentId = topicContent.Id,
                        ContentText = topicContent.ContentText,
                        ContentType = topicContent.TypeText,
                        ContentTypeId = topicContent.ContentTypeId,
                        ContentOrder = topicContent.ContentOrder,
                        IsActive = topicContent.IsActive
                    };

                    var topicContentDocuments = await (from tcd in _parentcheckContext.InstituteTopicContentDocument
                                                       where tcd.InstituteTopicContentId == topicContent.Id
                                                       select new
                                                       {
                                                           tcd.Id,
                                                           tcd.FileName,
                                                           tcd.EncryptedFileName,
                                                           tcd.ContentUrl
                                                       }).ToListAsync();
                    foreach (var document in topicContentDocuments)
                    {
                        content.ContentDocuments.Add(new ContentDocumentDTO
                        {
                            Id = document.Id,
                            FileName = document.FileName,
                            EncryptedFileName = document.EncryptedFileName,
                            Url = document.ContentUrl,
                            InstituteTopicContentId = topicContent.Id
                        });
                    }
                    topicContentDTOs.Add(content);
                }
            }

            return topicContentDTOs;
        }

        public async Task<bool> SaveTopicContent(long id, long topicId, string contentText, int contentTypeId, int orderId, bool isActive, long userId)
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
                    InstituteTopicContent instituteTopicContent;
                    if (id > 0)
                    {
                        var topicContent = _parentcheckContext.InstituteTopicContent.FirstOrDefault(i => i.Id == id);

                        if (topicContent != null)
                        {
                            instituteTopicContent = topicContent;
                            instituteTopicContent.IsActive = isActive;
                            instituteTopicContent.ContentText = contentText;
                            instituteTopicContent.ContentTypeId = contentTypeId;
                            instituteTopicContent.ContentOrder = orderId;
                            instituteTopicContent.UpdateOn = DateTime.UtcNow;
                            instituteTopicContent.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteTopicContent).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteTopicContent = new InstituteTopicContent();

                    instituteTopicContent.InstituteChapterTopicId = topicId;
                    instituteTopicContent.ContentOrder = orderId;
                    instituteTopicContent.ContentText = contentText;
                    instituteTopicContent.ContentTypeId = contentTypeId;
                    instituteTopicContent.IsActive = isActive;
                    instituteTopicContent.CreatedOn = DateTime.UtcNow;
                    instituteTopicContent.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteTopicContent.UpdateOn = DateTime.UtcNow;
                    instituteTopicContent.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteTopicContent.Add(instituteTopicContent);
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

        public async Task<bool> UploadTopicContent(long topicContentId, string encryptedFileName, string uploadPath, string fileName, long userId)
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
                    InstituteTopicContentDocument instituteTopicContentDocument = new InstituteTopicContentDocument();

                    instituteTopicContentDocument.InstituteTopicContentId = topicContentId;
                    instituteTopicContentDocument.IsActive = true;
                    instituteTopicContentDocument.FileName = fileName;
                    instituteTopicContentDocument.EncryptedFileName = encryptedFileName;
                    instituteTopicContentDocument.ContentUrl = $"https://storage.parentcheck.lk/{uploadPath}/{encryptedFileName}";
                    instituteTopicContentDocument.CreatedOn = DateTime.UtcNow;
                    instituteTopicContentDocument.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteTopicContentDocument.UpdateOn = DateTime.UtcNow;
                    instituteTopicContentDocument.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteTopicContentDocument.Add(instituteTopicContentDocument);
                    await _parentcheckContext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        public async Task<bool> RemoveTopicContent(long topicContentId, long id)
        {
            var contentDocument = _parentcheckContext.InstituteTopicContentDocument.Where(i => i.Id == id).FirstOrDefault();
            if (contentDocument != null)
            {
                _parentcheckContext.InstituteTopicContentDocument.Remove(contentDocument);
                await _parentcheckContext.SaveChangesAsync();

                return true;
            }
            return false;
        }

        #endregion

        public async Task<List<AssociateClassDTO>> GetAssociateClass(long userId)
        {
            List<AssociateClassDTO> associateClassDTOs = new List<AssociateClassDTO>();

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
                var classSubjects = await (from cs in _parentcheckContext.InstituteClassSubject
                                             join c in _parentcheckContext.InstituteClass on cs.InstituteClassId equals c.Id
                                             join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                             where cs.ResponsibleUserId==user.Id
                                             select new
                                             {
                                                 cs.Id,
                                                 c.Class,
                                                 s.Subject
                                             }).ToListAsync();

                foreach (var classSubject in classSubjects)
                {
                    associateClassDTOs.Add(new AssociateClassDTO
                    {
                        Id = classSubject.Id,
                        AssociateClass = classSubject.Class,
                        Subject = classSubject.Subject
                    });
                }
            }

            return associateClassDTOs;
        }


        public async Task<List<WeekDayDTO>> GetTimeTable(bool isOnlyToday, long userId)
        {
            List<WeekDayDTO> weekDayDTOs = new List<WeekDayDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where (u.ParentUserid == userId || u.Id == userId)
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.RoleId
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                if (user.RoleId == (int)EnumRole.Staff)
                {
                    var userClasSubjects = await (from cu in _parentcheckContext.InstituteClassSubject
                                                  join c in _parentcheckContext.InstituteClass on cu.InstituteClassId equals c.Id
                                                  join ay in _parentcheckContext.AcademicYear on c.AcademicYearId equals ay.Id
                                                  where cu.ResponsibleUserId == user.Id && ay.FromDate <= DateTime.Now && ay.ToDate >= DateTime.Now
                                                  select new
                                                  {
                                                      cu.Id,
                                                      cu.InstituteClassId,
                                                      cu.InstituteSubjectId,
                                                      c.Class
                                                  }).ToListAsync();

                    var todayDayOfWeek = DateTime.Now.DayOfWeek;

                    var weekdays = await _parentcheckContext.WeekDay.Where(i => (isOnlyToday && i.DayOfWeek == (int)todayDayOfWeek) || (!isOnlyToday)).ToListAsync();

                    foreach (var weekday in weekdays)
                    {
                        var weekDayDTO = new WeekDayDTO();
                        weekDayDTO.Weekday = weekday.WeekDayText;
                        foreach (var userClasSubject in userClasSubjects)
                        {
                            

                            var timeTables = await (from t in _parentcheckContext.InstituteClassTimeTable
                                                    join c in _parentcheckContext.InstituteClass on t.InstituteClassId equals c.Id
                                                    join cs in _parentcheckContext.InstituteClassSubject on t.InstituteClassSubjectId equals cs.Id
                                                    join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                                    where t.InstituteClassId == userClasSubject.InstituteClassId && t.InstituteClassSubjectId== userClasSubject .Id && t.WeekDayId == weekday.Id
                                                    select new
                                                    {
                                                        c.Class,
                                                        s.Subject,
                                                        t.FromTime,
                                                        t.ToTime
                                                    }).ToListAsync();

                            foreach (var timeTable in timeTables)
                            {
                                weekDayDTO.TimeTables.Add(new TimeTableDTO
                                {
                                    ClassName = timeTable.Class,
                                    Subject = $"{timeTable.Class} - {timeTable.Subject}",                                    
                                    Weekday = weekday.WeekDayText,
                                    FromTime = timeTable.FromTime,
                                    ToTime = timeTable.ToTime
                                });
                            }
                            
                        }
                        weekDayDTOs.Add(weekDayDTO);
                    }

                    return weekDayDTOs;
                }

                var userActiveClass = await (from cu in _parentcheckContext.InstituteUserClass
                                             join c in _parentcheckContext.InstituteClass on cu.InstituteClassId equals c.Id
                                             join ay in _parentcheckContext.AcademicYear on cu.AcademicYearId equals ay.Id
                                             where cu.InstituteUserId == user.Id && ay.FromDate <= DateTime.Now && ay.ToDate >= DateTime.Now
                                             select new
                                             {
                                                 cu.InstituteClassId,
                                                 c.Class
                                             }).FirstOrDefaultAsync();
                if (userActiveClass != null)
                {
                    
                    var todayDayOfWeek = DateTime.Now.DayOfWeek;

                    var weekdays = _parentcheckContext.WeekDay.Where(i => (isOnlyToday && i.DayOfWeek == (int)todayDayOfWeek) || (!isOnlyToday)).ToList();

                    foreach (var weekday in weekdays)
                    {
                        var weekDayDTO = new WeekDayDTO();
                        weekDayDTO.Weekday = weekday.WeekDayText;

                        var timeTables = await (from t in _parentcheckContext.InstituteClassTimeTable
                                                   join c in _parentcheckContext.InstituteClass on t.InstituteClassId equals c.Id
                                                   join cs in _parentcheckContext.InstituteClassSubject on t.InstituteClassSubjectId equals cs.Id
                                                   join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                                   where t.InstituteClassId== userActiveClass.InstituteClassId && t.WeekDayId== weekday.Id
                                                select new
                                                   {
                                                       c.Class,
                                                       s.Subject,
                                                       t.FromTime,
                                                       t.ToTime
                                                   }).ToListAsync();

                        foreach (var timeTable in timeTables)
                        {
                            weekDayDTO.TimeTables.Add(new TimeTableDTO
                            {
                                ClassName= timeTable.Class,
                                Subject = timeTable.Subject,
                                Weekday = weekday.WeekDayText,
                                FromTime= timeTable.FromTime,
                                ToTime= timeTable.ToTime
                            });
                        }
                        weekDayDTOs.Add(weekDayDTO);
                    }                    
                }
            }

            return weekDayDTOs;
        }

        public async Task<List<WeekDayDTO>> GetAllTimeTable(long classId, long userId)
        {
            List<WeekDayDTO> weekDayDTOs = new List<WeekDayDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where (u.ParentUserid == userId || u.Id == userId)
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId,
                                  u.RoleId
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                

                    var todayDayOfWeek = DateTime.Now.DayOfWeek;

                    var weekdays = _parentcheckContext.WeekDay.ToList();

                    foreach (var weekday in weekdays)
                    {
                        var weekDayDTO = new WeekDayDTO();
                        weekDayDTO.Weekday = weekday.WeekDayText;

                        var timeTables = await (from t in _parentcheckContext.InstituteClassTimeTable
                                                join c in _parentcheckContext.InstituteClass on t.InstituteClassId equals c.Id
                                                join cs in _parentcheckContext.InstituteClassSubject on t.InstituteClassSubjectId equals cs.Id
                                                join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                                where t.InstituteClassId == classId && t.WeekDayId == weekday.Id
                                                select new
                                                {
                                                    c.Class,
                                                    s.Subject,
                                                    t.FromTime,
                                                    t.ToTime
                                                }).ToListAsync();

                        foreach (var timeTable in timeTables)
                        {
                            weekDayDTO.TimeTables.Add(new TimeTableDTO
                            {
                                ClassName = timeTable.Class,
                                Subject = timeTable.Subject,
                                Weekday = weekday.WeekDayText,
                                FromTime = timeTable.FromTime,
                                ToTime = timeTable.ToTime
                            });
                        }
                        weekDayDTOs.Add(weekDayDTO);
                    }
            }

            return weekDayDTOs;
        }

        public async Task<bool> SaveTimeTable(long id, long classId, long subjectId, string fromTime, string toTime, int weekDayId, long userId)
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
                    InstituteClassTimeTable instituteClassTimeTable;
                    
                        var timeTable = _parentcheckContext.InstituteClassTimeTable.FirstOrDefault(i => i.WeekDayId == weekDayId && i.InstituteClassSubjectId==subjectId && i.InstituteClassId==classId);

                    if (timeTable != null)
                    {
                        var timeTableFromDate = DateTime.Parse(timeTable.FromTime);
                        var timeTableToDate = DateTime.Parse(timeTable.ToTime);

                        var commandFromDate = DateTime.Parse(fromTime);
                        var commandToDate = DateTime.Parse(toTime);

                        if ((commandFromDate.Ticks >= timeTableFromDate.Ticks && commandFromDate.Ticks <= timeTableToDate.Ticks)
                            ||
                            (commandToDate.Ticks >= timeTableFromDate.Ticks && commandToDate.Ticks <= timeTableToDate.Ticks))

                        {
                            _parentcheckContext.Remove(timeTable);
                            await _parentcheckContext.SaveChangesAsync();
                        }
                    }

                    instituteClassTimeTable = new InstituteClassTimeTable();

                    instituteClassTimeTable.InstituteClassId = classId;
                    instituteClassTimeTable.InstituteClassSubjectId = subjectId;
                    instituteClassTimeTable.FromTime = fromTime;
                    instituteClassTimeTable.ToTime = toTime;
                    instituteClassTimeTable.WeekDayId = weekDayId;
                    instituteClassTimeTable.CreatedOn = DateTime.UtcNow;
                    instituteClassTimeTable.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteClassTimeTable.UpdateOn = DateTime.UtcNow;
                    instituteClassTimeTable.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteClassTimeTable.Add(instituteClassTimeTable);
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
    }
}
