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
    public class ReferenceRepository : IReferenceRepository
    {
        private ParentCheckContext _parentcheckContext;

        public ReferenceRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<List<ReferenceDTO>> GetReferenceByTypeAsync(int referenceTypeId, long userId)
        {
            List<ReferenceDTO> references = new List<ReferenceDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId
                              }).FirstOrDefaultAsync();

            if ((int)EnumReferenceType.Subject == referenceTypeId)
            {
                var userActiveClass = await (from cu in _parentcheckContext.InstituteUserClass
                                             where cu.InstituteUserId == userId
                                             select new
                                             {
                                                 cu.InstituteClassId
                                             }).FirstOrDefaultAsync();

                if (userActiveClass != null)
                {
                    var userClassSubjects = await (from cs in _parentcheckContext.InstituteClassSubject
                                                   join s in _parentcheckContext.InstituteSubject on cs.InstituteSubjectId equals s.Id
                                                   where cs.InstituteClassId == userActiveClass.InstituteClassId
                                                   select new
                                                   {
                                                       cs.Id,
                                                       s.Subject,
                                                       s.DescriptionText,
                                                       cs.BgColor,
                                                       cs.FontColor
                                                   }).ToListAsync();

                    foreach (var userClassSubject in userClassSubjects)
                    {
                        references.Add(new ReferenceDTO
                        {
                            Id = userClassSubject.Id,
                            ValueText = userClassSubject.Subject
                        });
                    }
                }
            }

            if ((int)EnumReferenceType.Term == referenceTypeId)
            {
                var instituteTerms = await (from t in _parentcheckContext.InstituteTerm
                                             where t.InstituteId == user.InstituteId
                                             select new
                                             {
                                                 t.Id,
                                                 t.Term
                                             }).ToListAsync();

                foreach (var instituteTerm in instituteTerms)
                {
                    references.Add(new ReferenceDTO
                    {
                        Id = instituteTerm.Id,
                        ValueText = instituteTerm.Term
                    });
                }
            }

            if ((int)EnumReferenceType.CommunicationGroup == referenceTypeId)
            {
                var communicationGroups = await (from cg in _parentcheckContext.CommunicationGroup
                                            where cg.IsActive
                                            select new
                                            {
                                                cg.Id,
                                                cg.GroupName
                                            }).ToListAsync();

                foreach (var communicationGroup in communicationGroups)
                {
                    references.Add(new ReferenceDTO
                    {
                        Id = communicationGroup.Id,
                        ValueText = communicationGroup.GroupName
                    });
                }
            }

            if ((int)EnumReferenceType.UserClass == referenceTypeId)
            {
                var classes = await (from ic in _parentcheckContext.InstituteClass
                                                 where ic.ResponsibleUserId==userId && ic.IsActive==true
                                                 select new
                                                 {
                                                     ic.Id,
                                                     ic.Class
                                                 }).ToListAsync();

                foreach (var classe in classes)
                {
                    references.Add(new ReferenceDTO
                    {
                        Id = classe.Id,
                        ValueText = classe.Class
                    });
                }
            }

            if ((int)EnumReferenceType.Institute == referenceTypeId)
            {
                if (user != null)
                {
                    var classes = await (from ic in _parentcheckContext.InstituteUser
                                         join i in _parentcheckContext.Institute on ic.InstituteId equals i.Id
                                         where ic.Id == userId
                                         select new
                                         {
                                             ic.Id,
                                             i.InstituteName
                                         }).ToListAsync();

                    foreach (var classe in classes)
                    {
                        references.Add(new ReferenceDTO
                        {
                            Id = classe.Id,
                            ValueText = classe.InstituteName
                        });
                    }
                }
            }

            if ((int)EnumReferenceType.InvoiceType == referenceTypeId)
            {
                if (user != null)
                {
                    var invoiceTypes = await (from it in _parentcheckContext.InstituteInvoiceType
                                         where it.InstituteId == user.InstituteId && it.IsActive==true
                                         select new
                                         {
                                             it.Id,
                                             it.InvoiceTypeText
                                         }).ToListAsync();

                    foreach (var invoiceType in invoiceTypes)
                    {
                        references.Add(new ReferenceDTO
                        {
                            Id = invoiceType.Id,
                            ValueText = invoiceType.InvoiceTypeText
                        });
                    }
                }
            }

            if ((int)EnumReferenceType.Role == referenceTypeId)
            {
                var roles = await (from r in _parentcheckContext.Role
                                   select new
                                   {
                                       r.Id,
                                       r.RoleText
                                   }).ToListAsync();

                foreach (var role in roles)
                {
                    references.Add(new ReferenceDTO
                    {
                        Id = role.Id,
                        ValueText = role.RoleText
                    });
                }
            }

            if ((int)EnumReferenceType.AcademicYear == referenceTypeId)
            {
                if (user != null)
                {
                    var academicYears = await (from a in _parentcheckContext.AcademicYear
                                              where a.InstituteId == user.InstituteId && a.IsActive == true
                                              select new
                                              {
                                                  a.Id,
                                                  a.YearAcademic
                                              }).ToListAsync();

                    foreach (var academicYear in academicYears)
                    {
                        references.Add(new ReferenceDTO
                        {
                            Id = academicYear.Id,
                            ValueText = academicYear.YearAcademic.ToString()
                        });
                    }
                }
            }

            if ((int)EnumReferenceType.Teacher == referenceTypeId)
            {
                if (user != null)
                {
                    var teachers = await (from a in _parentcheckContext.InstituteUser
                                               where a.InstituteId == user.InstituteId && a.IsActive == true && a.RoleId==(int)EnumRole.Teacher
                                               select new
                                               {
                                                   a.Id,
                                                   a.FirstName,
                                                   a.LastName
                                               }).ToListAsync();

                    foreach (var teacher in teachers)
                    {
                        references.Add(new ReferenceDTO
                        {
                            Id = teacher.Id,
                            ValueText = $"{teacher.FirstName} {teacher.LastName}"
                        });
                    }
                }
            }

            if ((int)EnumReferenceType.AllClasses == referenceTypeId)
            {
                var classes = await (from ic in _parentcheckContext.InstituteClass
                                     where ic.IsActive == true
                                     select new
                                     {
                                         ic.Id,
                                         ic.Class
                                     }).ToListAsync();

                foreach (var classe in classes)
                {
                    references.Add(new ReferenceDTO
                    {
                        Id = classe.Id,
                        ValueText = classe.Class
                    });
                }
            }

            if ((int)EnumReferenceType.Student == referenceTypeId)
            {
                if (user != null)
                {
                    var teachers = await (from a in _parentcheckContext.InstituteUser
                                          where a.InstituteId == user.InstituteId && a.IsActive == true && a.RoleId == (int)EnumRole.Student
                                          select new
                                          {
                                              a.Id,
                                              a.FirstName,
                                              a.LastName
                                          }).ToListAsync();

                    foreach (var teacher in teachers)
                    {
                        references.Add(new ReferenceDTO
                        {
                            Id = teacher.Id,
                            ValueText = $"{teacher.FirstName} {teacher.LastName}"
                        });
                    }
                }
            }

            return references;
        }

        public async Task<List<UserContactDTO>> GetUserContactAsync(string name, long userId)
        {
            List<UserContactDTO> userContacts = new List<UserContactDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var uContacts= await (from u in _parentcheckContext.InstituteUser
                                      join uc in _parentcheckContext.UserContact on u.Id equals uc.InstituteUserId
                            where (u.FirstName.Contains(name) || u.LastName.Contains(name)) 
                            && u.InstituteId== user.InstituteId
                            && uc.IsPrimary
                            && uc.IsActive
                            select new
                            {
                                u.FirstName,
                                u.LastName,
                                u.Id,
                                u.InstituteId,
                                uc.ContactTypeId,
                                uc.ContactValue
                            }).ToListAsync();

                foreach (var uContact in uContacts)
                {
                    UserContactDTO userContactDTO = new UserContactDTO();
                    userContactDTO.UserId = uContact.Id;
                    userContactDTO.UserFullName = $"{uContact.FirstName} {uContact.LastName}";

                    if (uContact.ContactTypeId == (int)EnumContactType.Email)
                    {
                        userContactDTO.Email = uContact.ContactValue;
                    }

                    if (uContact.ContactTypeId == (int)EnumContactType.Mobile)
                    {
                        userContactDTO.Mobile = uContact.ContactValue;
                    }

                    userContacts.Add(userContactDTO);
                }
            }

            return userContacts;
        }

        public async Task<List<UserContactDTO>> GetAllUserContactAsync(int sendType, long userId)
        {
            List<UserContactDTO> userContacts = new List<UserContactDTO>();

            var user = await (from u in _parentcheckContext.InstituteUser
                              where u.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  u.Id,
                                  u.InstituteId
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var uContacts = await (from u in _parentcheckContext.InstituteUser
                                       join uc in _parentcheckContext.UserContact on u.Id equals uc.InstituteUserId
                                       where u.InstituteId == user.InstituteId
                                       && uc.ContactTypeId== sendType
                                       && uc.IsPrimary
                                       && uc.IsActive
                                       select new
                                       {
                                           u.FirstName,
                                           u.LastName,
                                           u.Id,
                                           u.InstituteId,
                                           uc.ContactTypeId,
                                           uc.ContactValue
                                       }).ToListAsync();

                foreach (var uContact in uContacts)
                {
                    UserContactDTO userContactDTO = new UserContactDTO();
                    userContactDTO.UserId = uContact.Id;
                    userContactDTO.UserFullName = $"{uContact.FirstName} {uContact.LastName}";

                    if (uContact.ContactTypeId == (int)EnumContactType.Email)
                    {
                        userContactDTO.Email = uContact.ContactValue;
                    }

                    if (uContact.ContactTypeId == (int)EnumContactType.Mobile)
                    {
                        userContactDTO.Mobile = uContact.ContactValue;
                    }

                    userContacts.Add(userContactDTO);
                }
            }

            return userContacts;
        }
    }
}
