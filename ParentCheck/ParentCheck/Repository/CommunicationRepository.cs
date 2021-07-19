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
    public class CommunicationRepository : ICommunicationRepository
    {
        private ParentCheckContext _parentcheckContext;

        public CommunicationRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<List<CommunicationDTO>> GetCommunicationInboxAsync( long userId)
        {
            List<CommunicationDTO> communicationInboxes = new List<CommunicationDTO>();

            var user = await (from u in _parentcheckContext.User
                        join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                        where iu.Id == userId
                        select new
                        {
                            u.FirstName,
                            u.LastName,
                            iu.Id,
                            iu.InstituteId,
                            iu
                        }).FirstOrDefaultAsync();

            if (user != null)
            {
                var inboxMessages = await(from cr in _parentcheckContext.InstituteCommunicationReceiver
                                    join c in _parentcheckContext.InstituteCommunication on cr.CommunicationId equals c.Id
                                    join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                    join u in _parentcheckContext.User on iu.UserId equals u.Id
                                    where cr.ToUserId==user.Id && !c.AssociatedCommunicationId.HasValue
                                    select new
                                    {
                                        c.Message,
                                        c.SendDate,
                                        c.Subject,
                                        c.CommunicationType,
                                        c.Id,
                                        u.FirstName,
                                        u.LastName
                                    }).ToListAsync();

                foreach (var inboxMessage in inboxMessages)
                {
                    communicationInboxes.Add(new CommunicationDTO
                    {
                        Id= inboxMessage.Id,
                        Subject= inboxMessage.Subject,
                        Message= inboxMessage.Message,
                        CommunicationType= inboxMessage.CommunicationType,
                        SendDate= inboxMessage.SendDate,
                        FromUser= $"{inboxMessage.FirstName} {inboxMessage.LastName}",
                        ToUser= $"{user.FirstName} {user.LastName}"
                    });
                }
            }

            return communicationInboxes;
        }

        public async Task<List<CommunicationDTO>> GetCommunicationOutboxAsync(long userId)
        {
            List<CommunicationDTO> communicationOutboxes = new List<CommunicationDTO>();

            var user = await (from u in _parentcheckContext.User
                              join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                              where iu.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  iu.Id,
                                  iu.InstituteId,
                                  iu
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                var messages = await (from c in _parentcheckContext.InstituteCommunication 
                                           join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                           join u in _parentcheckContext.User on iu.UserId equals u.Id
                                           where c.FromUserid == user.Id && c.AssociatedCommunicationId.HasValue
                                           select new
                                           {
                                               c.Message,
                                               c.SendDate,
                                               c.Subject,
                                               c.CommunicationType,
                                               c.Id
                                           }).ToListAsync();

                foreach (var message in messages)
                {
                    string toList = string.Empty;
                    var toUsers = await (from cr in _parentcheckContext.InstituteCommunicationReceiver
                                               join iu in _parentcheckContext.InstituteUser on cr.ToUserId equals iu.Id
                                               join u in _parentcheckContext.User on iu.UserId equals u.Id
                                               where cr.CommunicationId== message.Id
                                               select new
                                               {
                                                   u.FirstName,
                                                   u.LastName
                                               }).ToListAsync();

                    if (toUsers.Any())
                    {
                        toList = string.Join(',', toUsers.Select(i=>$"{i.FirstName} {i.LastName}"));
                    }

                    communicationOutboxes.Add(new CommunicationDTO
                    {
                        Id = message.Id,
                        Subject = message.Subject,
                        Message = message.Message,
                        CommunicationType = message.CommunicationType,
                        SendDate = message.SendDate,
                        FromUser = $"{user.FirstName} {user.LastName}",
                        ToUser = toList
                    }); ;
                }
            }

            return communicationOutboxes;
        }

        public async Task<CommunicationDTO> GetCommunicationDetailAsync(long id, int type, long userId)
        {
            CommunicationDTO communication= new CommunicationDTO();

            var user = await (from u in _parentcheckContext.User
                              join iu in _parentcheckContext.InstituteUser on u.Id equals iu.UserId
                              where iu.Id == userId
                              select new
                              {
                                  u.FirstName,
                                  u.LastName,
                                  iu.Id,
                                  iu.InstituteId,
                                  iu
                              }).FirstOrDefaultAsync();

            if (user != null)
            {
                if (type == (int)EnumCommunicationBoxType.Inbox)
                {
                    var message = await (from cr in _parentcheckContext.InstituteCommunicationReceiver
                                         join c in _parentcheckContext.InstituteCommunication on cr.CommunicationId equals c.Id
                                         join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                         join u in _parentcheckContext.User on iu.UserId equals u.Id
                                         where cr.ToUserId == user.Id && !c.AssociatedCommunicationId.HasValue && c.Id == id
                                         select new
                                         {
                                             c.Message,
                                             c.SendDate,
                                             c.Subject,
                                             c.CommunicationType,
                                             c.Id,
                                             u.FirstName,
                                             u.LastName
                                         }).FirstOrDefaultAsync();

                    if (message != null)
                    {
                        communication = new CommunicationDTO
                        {
                            Id = message.Id,
                            Subject = message.Subject,
                            Message = message.Message,
                            CommunicationType = message.CommunicationType,
                            SendDate = message.SendDate,
                            FromUser = $"{message.FirstName} {message.LastName}",
                            ToUser = $"{user.FirstName} {user.LastName}"
                        };
                    }
                }

                if (type == (int)EnumCommunicationBoxType.Outbox)
                {
                    var message = await (from c in _parentcheckContext.InstituteCommunication
                                          join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                          join u in _parentcheckContext.User on iu.UserId equals u.Id
                                          where c.FromUserid == user.Id && c.AssociatedCommunicationId.HasValue && c.Id==id
                                          select new
                                          {
                                              c.Message,
                                              c.SendDate,
                                              c.Subject,
                                              c.CommunicationType,
                                              c.Id
                                          }).FirstOrDefaultAsync();

                    if(message != null)
                    {
                        string toList = string.Empty;
                        var toUsers = await (from cr in _parentcheckContext.InstituteCommunicationReceiver
                                             join iu in _parentcheckContext.InstituteUser on cr.ToUserId equals iu.Id
                                             join u in _parentcheckContext.User on iu.UserId equals u.Id
                                             where cr.CommunicationId == message.Id
                                             select new
                                             {
                                                 u.FirstName,
                                                 u.LastName
                                             }).ToListAsync();

                        if (toUsers.Any())
                        {
                            toList = string.Join(',', toUsers.Select(i => $"{i.FirstName} {i.LastName}"));
                        }

                        communication = new CommunicationDTO
                        {
                            Id = message.Id,
                            Subject = message.Subject,
                            Message = message.Message,
                            CommunicationType = message.CommunicationType,
                            SendDate = message.SendDate,
                            FromUser = $"{user.FirstName} {user.LastName}",
                            ToUser = toList
                        };
                    }
                }

                
            }

            return communication;
        }
    }
}
