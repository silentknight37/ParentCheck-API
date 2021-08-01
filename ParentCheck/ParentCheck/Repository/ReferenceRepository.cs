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
    }
}
