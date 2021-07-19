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

            var user = (from u in _parentcheckContext.User
                        join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                        where iu.Id == userId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            iu.Id,
                            iu.InstituteId,
                            iu
                        }).FirstOrDefault();

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


            return references;
        }

        public async Task<List<UserContactDTO>> GetUserContactAsync(string name, long userId)
        {
            List<UserContactDTO> userContacts = new List<UserContactDTO>();

            var user = (from u in _parentcheckContext.User
                        join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                        where iu.Id == userId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            iu.Id,
                            iu.InstituteId,
                            iu
                        }).FirstOrDefault();

            if (user != null)
            {
                var uContacts= await (from u in _parentcheckContext.User
                            join uc in _parentcheckContext.UserContact on u.Id equals uc.UserId
                            join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                            where (u.FirstName.Contains(name) || u.LastName.Contains(name)) 
                            && iu.InstituteId== user.InstituteId
                            && uc.IsPrimary
                            && uc.IsActive
                            select new
                            {
                                u.FirstName,
                                u.LastName,
                                iu.Id,
                                iu.InstituteId,
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
