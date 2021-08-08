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
                var inboxMessages = await(from cr in _parentcheckContext.InstituteCommunicationReceiver
                                    join c in _parentcheckContext.InstituteCommunication on cr.CommunicationId equals c.Id
                                    join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                    where cr.ToUserId==user.Id && !c.AssociatedCommunicationId.HasValue
                                    && (c.CommunicationType == (int)EnumCommunication.Email || c.CommunicationType == (int)EnumCommunication.Template)
                                          orderby c.SendDate descending
                                          select new
                                    {
                                        c.Message,
                                        c.SendDate,
                                        c.Subject,
                                        c.CommunicationType,
                                        c.Id,
                                        iu.FirstName,
                                        iu.LastName,
                                        c.FromUserid,
                                        cr.TemplateId
                                    }).ToListAsync();

                foreach (var inboxMessage in inboxMessages)
                {
                    var communicationTemplate = _parentcheckContext.InstituteCommunicationTemplate.FirstOrDefault(i => i.Id == inboxMessage.TemplateId);

                    var communication = new CommunicationDTO
                    {
                        Id = inboxMessage.Id,
                        Subject = inboxMessage.Subject,
                        Message = inboxMessage.Message,
                        CommunicationType = inboxMessage.CommunicationType,
                        SendDate = inboxMessage.SendDate,
                        FromUser = $"{inboxMessage.FirstName} {inboxMessage.LastName}",
                        ToUser = $"{user.FirstName} {user.LastName}",
                        FromUserId= inboxMessage.FromUserid
                    };

                    if (communicationTemplate != null)
                    {
                        communication.CommunicationTemplateId = communicationTemplate.Id;
                        communication.CommunicationTemplateName = communicationTemplate.TemplateName;
                        communication.CommunicationTemplateContent = communicationTemplate.TemplateContent;
                    }

                    communicationInboxes.Add(communication);
                }
            }

            return communicationInboxes;
        }

        public async Task<List<CommunicationDTO>> GetCommunicationOutboxAsync(long userId)
        {
            List<CommunicationDTO> communicationOutboxes = new List<CommunicationDTO>();

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
                var messages = await (from c in _parentcheckContext.InstituteCommunication
                                      join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                      where c.FromUserid == user.Id && !c.AssociatedCommunicationId.HasValue
                                      && (c.CommunicationType == (int)EnumCommunication.Email || c.CommunicationType == (int)EnumCommunication.Template)
                                      orderby c.SendDate descending
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
                                               where cr.CommunicationId== message.Id
                                               select new
                                               {
                                                   iu.FirstName,
                                                   iu.LastName
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

        public async Task<List<CommunicationDTO>> GetSMSCommunicationOutboxAsync(long userId)
        {
            List<CommunicationDTO> communicationOutboxes = new List<CommunicationDTO>();

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
                var messages = await (from c in _parentcheckContext.InstituteCommunication
                                      join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                      where c.FromUserid == user.Id 
                                      && !c.AssociatedCommunicationId.HasValue
                                      && c.CommunicationType==(int)EnumCommunication.SMS
                                      orderby c.SendDate descending
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
                                         where cr.CommunicationId == message.Id
                                         select new
                                         {
                                             iu.FirstName,
                                             iu.LastName
                                         }).ToListAsync();

                    if (toUsers.Any())
                    {
                        toList = string.Join(',', toUsers.Select(i => $"{i.FirstName} {i.LastName}"));
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

        public async Task<List<CommunicationDTO>> GetCommunicationDetailAsync(long id, int type, long userId)
        {
            List<CommunicationDTO> communications = new List<CommunicationDTO>();

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
                if (type == (int)EnumCommunicationBoxType.Inbox)
                {
                    var inboxMessage = await (from cr in _parentcheckContext.InstituteCommunicationReceiver
                                               join c in _parentcheckContext.InstituteCommunication on cr.CommunicationId equals c.Id
                                               join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                               where c.Id==id && !c.AssociatedCommunicationId.HasValue
                                               && (c.CommunicationType == (int)EnumCommunication.Email || c.CommunicationType == (int)EnumCommunication.Template)
                                               //orderby c.SendDate descending
                                               select new
                                               {
                                                   c.Message,
                                                   c.SendDate,
                                                   c.Subject,
                                                   c.CommunicationType,
                                                   c.Id,
                                                   iu.FirstName,
                                                   iu.LastName,
                                                   c.FromUserid,
                                                   cr.TemplateId
                                               }).FirstOrDefaultAsync();

                    if(inboxMessage!=null)
                    {

                        var communicationTemplate = _parentcheckContext.InstituteCommunicationTemplate.FirstOrDefault(i => i.Id == inboxMessage.TemplateId);

                        var communication = new CommunicationDTO
                        {
                            Id = inboxMessage.Id,
                            Subject = inboxMessage.Subject,
                            Message = inboxMessage.Message,
                            CommunicationType = inboxMessage.CommunicationType,
                            SendDate = inboxMessage.SendDate,
                            FromUser = $"{inboxMessage.FirstName} {inboxMessage.LastName}",
                            ToUser = $"{user.FirstName} {user.LastName}",
                            FromUserId = inboxMessage.FromUserid
                        };

                        if (communicationTemplate != null)
                        {
                            communication.CommunicationTemplateId = communicationTemplate.Id;
                            communication.CommunicationTemplateName = communicationTemplate.TemplateName;
                            communication.CommunicationTemplateContent = communicationTemplate.TemplateContent;
                        }

                        communications.Add(communication);

                        communications.AddRange(await GetAssosiateCommunicationInboxAsync(user.FirstName,user.LastName,id));
                    }
                }

                if (type == (int)EnumCommunicationBoxType.Outbox)
                {
                    var message = await (from c in _parentcheckContext.InstituteCommunication
                                          join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                          where c.Id == id && !c.AssociatedCommunicationId.HasValue
                                          && (c.CommunicationType == (int)EnumCommunication.Email || c.CommunicationType == (int)EnumCommunication.Template)
                                          //orderby c.SendDate descending
                                          select new
                                          {
                                              c.Message,
                                              c.SendDate,
                                              c.Subject,
                                              c.CommunicationType,
                                              c.Id
                                          }).FirstOrDefaultAsync();

                    if(message!=null)
                    {
                        string toList = string.Empty;
                        var toUsers = await (from cr in _parentcheckContext.InstituteCommunicationReceiver
                                             join iu in _parentcheckContext.InstituteUser on cr.ToUserId equals iu.Id
                                             where cr.CommunicationId == message.Id
                                             select new
                                             {
                                                 iu.FirstName,
                                                 iu.LastName
                                             }).ToListAsync();

                        if (toUsers.Any())
                        {
                            toList = string.Join(',', toUsers.Select(i => $"{i.FirstName} {i.LastName}"));
                        }

                        communications.Add(new CommunicationDTO
                        {
                            Id = message.Id,
                            Subject = message.Subject,
                            Message = message.Message,
                            CommunicationType = message.CommunicationType,
                            SendDate = message.SendDate,
                            FromUser = $"{user.FirstName} {user.LastName}",
                            ToUser = toList
                        });

                        communications.AddRange(await GetAssosiateCommunicationOutboxAsync(user.FirstName, user.LastName, id));
                    }
                }

                
            }

            return communications;
        }

        private async Task<List<CommunicationDTO>> GetAssosiateCommunicationInboxAsync(string userFirstName,string userLastName,long id)
        {
            List<CommunicationDTO> communications = new List<CommunicationDTO>();

            var inboxMessages = await (from cr in _parentcheckContext.InstituteCommunicationReceiver
                                       join c in _parentcheckContext.InstituteCommunication on cr.CommunicationId equals c.Id
                                       join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                       where c.AssociatedCommunicationId==id
                                       && (c.CommunicationType == (int)EnumCommunication.Email || c.CommunicationType == (int)EnumCommunication.Template)
                                       //orderby c.SendDate descending
                                       select new
                                       {
                                           c.Message,
                                           c.SendDate,
                                           c.Subject,
                                           c.CommunicationType,
                                           c.Id,
                                           iu.FirstName,
                                           iu.LastName,
                                           cr.TemplateId
                                       }).ToListAsync();

            foreach (var inboxMessage in inboxMessages)
            {
                var communicationTemplate = _parentcheckContext.InstituteCommunicationTemplate.FirstOrDefault(i => i.Id == inboxMessage.TemplateId);

                var communication = new CommunicationDTO
                {
                    Id = inboxMessage.Id,
                    Subject = inboxMessage.Subject,
                    Message = inboxMessage.Message,
                    CommunicationType = inboxMessage.CommunicationType,
                    SendDate = inboxMessage.SendDate,
                    FromUser = $"{inboxMessage.FirstName} {inboxMessage.LastName}",
                    ToUser = $"{userFirstName} {userLastName}"
                };

                if (communicationTemplate != null)
                {
                    communication.CommunicationTemplateId = communicationTemplate.Id;
                    communication.CommunicationTemplateName = communicationTemplate.TemplateName;
                    communication.CommunicationTemplateContent = communicationTemplate.TemplateContent;
                }

                communications.Add(communication);
            }

            return communications;
        }

        private async Task<List<CommunicationDTO>> GetAssosiateCommunicationOutboxAsync(string userFirstName, string userLastName, long id)
        {
            List<CommunicationDTO> communications = new List<CommunicationDTO>();

            var messages = await (from c in _parentcheckContext.InstituteCommunication
                                  join iu in _parentcheckContext.InstituteUser on c.FromUserid equals iu.Id
                                  where c.AssociatedCommunicationId==id
                                  && (c.CommunicationType == (int)EnumCommunication.Email || c.CommunicationType == (int)EnumCommunication.Template)
                                  //orderby c.SendDate descending
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
                                     where cr.CommunicationId == message.Id
                                     select new
                                     {
                                         iu.FirstName,
                                         iu.LastName
                                     }).ToListAsync();

                if (toUsers.Any())
                {
                    toList = string.Join(',', toUsers.Select(i => $"{i.FirstName} {i.LastName}"));
                }

                communications.Add(new CommunicationDTO
                {
                    Id = message.Id,
                    Subject = message.Subject,
                    Message = message.Message,
                    CommunicationType = message.CommunicationType,
                    SendDate = message.SendDate,
                    FromUser = $"{userFirstName} {userLastName}",
                    ToUser = toList
                }); ;
            }

            return communications;
        }

        public async Task<UserContactDTO> GetFromUserCommunicationAsync(long userId)
        {
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
                UserContactDTO userContact = new UserContactDTO();

                userContact.UserId = user.Id;
                userContact.UserFullName = $"{user.FirstName} {user.LastName}";

                var userContacts = _parentcheckContext.UserContact.Where(i => i.InstituteUserId == user.Id).ToList();
                foreach (var uContact in userContacts)
                {
                    if (uContact.ContactTypeId == (int)EnumContactType.Email)
                    {
                        userContact.Email = uContact.ContactValue;
                    }

                    if (uContact.ContactTypeId == (int)EnumContactType.Mobile)
                    {
                        userContact.Mobile = uContact.ContactValue;
                    }
                }

                return userContact;
            }

            return null;
        }

        public async Task<List<UserContactDTO>> GetToUserCommunicationAsync(List<ReferenceDTO> toGroup, long userId)
        {
            List<UserContactDTO> userContactDTOs = new List<UserContactDTO>();

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
                foreach (var item in toGroup)
                {
                    var toUsers = await (from u in _parentcheckContext.InstituteUser 
                                        where u.CommunicationGroup == item.Id && u.InstituteId==user.InstituteId
                                        select new
                                        {
                                            u.FirstName,
                                            u.LastName,
                                            u.Id,
                                            u.InstituteId,
                                            uId = u.Id
                                        }).ToListAsync();

                    foreach (var toUser in toUsers)
                    {
                        UserContactDTO userContact = new UserContactDTO();

                        userContact.UserId = toUser.Id;
                        userContact.UserFullName = $"{toUser.FirstName} {toUser.LastName}";

                        var userContacts = _parentcheckContext.UserContact.Where(i => i.InstituteUserId == toUser.Id).ToList();
                        foreach (var uContact in userContacts)
                        {
                            if (uContact.ContactTypeId == (int)EnumContactType.Email)
                            {
                                userContact.Email = uContact.ContactValue;
                            }

                            if (uContact.ContactTypeId == (int)EnumContactType.Mobile)
                            {
                                userContact.Mobile = uContact.ContactValue;
                            }
                        }

                        userContactDTOs.Add(userContact);
                    }
                }
            }

            return userContactDTOs;
        }

        public async Task<bool> SaveComposeCommunicationAsync(string subject, string messageText, List<UserContactDTO> toUsers, bool isGroup, DateTime? date, long? templateId, int communicationType, UserContactDTO fromUser, long userId)
        {
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
                try
                {
                    InstituteCommunication instituteCommunication = new InstituteCommunication();
                    instituteCommunication.Subject = subject;
                    instituteCommunication.CommunicationType = communicationType;
                    instituteCommunication.Message = messageText;
                    instituteCommunication.SendDate = DateTime.UtcNow;
                    instituteCommunication.FromUserid = user.Id;

                    if (communicationType == (int)EnumCommunication.SMS)
                    {
                        instituteCommunication.CommunicationSourceId = fromUser.Mobile;
                    }
                    else
                    {
                        instituteCommunication.CommunicationSourceId = fromUser.Email;
                    }

                    instituteCommunication.InstituteId = user.InstituteId;

                    if (templateId != null)
                    {
                        var template = await _parentcheckContext.InstituteCommunicationTemplate.FirstOrDefaultAsync(i => i.Id == templateId && i.IsSenderTemplate);
                        if (template != null){
                            instituteCommunication.TemplateId = templateId;
                        }
                    }
                    
                    instituteCommunication.CreatedOn = DateTime.UtcNow;
                    instituteCommunication.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteCommunication.UpdateOn = DateTime.UtcNow;
                    instituteCommunication.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteCommunication.Add(instituteCommunication);
                    await _parentcheckContext.SaveChangesAsync();
                    
                    foreach (var toUser in toUsers)
                    {
                        InstituteCommunicationReceiver instituteCommunicationReceiver = new InstituteCommunicationReceiver();
                        instituteCommunicationReceiver.CommunicationId = instituteCommunication.Id;
                        instituteCommunicationReceiver.ToUserId = toUser.UserId;

                        if (communicationType == (int)EnumCommunication.SMS)
                        {
                            instituteCommunicationReceiver.CommunicationSourceId = toUser.Mobile;
                        }
                        else
                        {
                            instituteCommunicationReceiver.CommunicationSourceId = toUser.Email;
                        }                        

                        if (templateId != null)
                        {
                            var template = await _parentcheckContext.InstituteCommunicationTemplate.FirstOrDefaultAsync(i => i.Id == templateId && !i.IsSenderTemplate);
                            if (template != null)
                            {
                                instituteCommunicationReceiver.TemplateId = templateId;
                            }
                        }
                        
                        instituteCommunicationReceiver.CreatedOn = DateTime.UtcNow;
                        instituteCommunicationReceiver.CreatedBy = $"{user.FirstName} {user.LastName}";
                        instituteCommunicationReceiver.UpdateOn = DateTime.UtcNow;
                        instituteCommunicationReceiver.UpdatedBy = $"{user.FirstName} {user.LastName}";

                        _parentcheckContext.InstituteCommunicationReceiver.Add(instituteCommunicationReceiver);
                        await _parentcheckContext.SaveChangesAsync();
                    }

                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
                
            }

            return false;
        }

        public async Task<bool> SaveReplyCommunicationAsync(long id, string subject, string messageText, long toUserId, UserContactDTO toUser, UserContactDTO fromUser, long userId)
        {
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
                try
                {
                    InstituteCommunication instituteCommunication = new InstituteCommunication();
                    instituteCommunication.Subject = subject;
                    instituteCommunication.CommunicationType = 1;
                    instituteCommunication.Message = messageText;
                    instituteCommunication.SendDate = DateTime.UtcNow;
                    instituteCommunication.FromUserid = user.Id;
                    instituteCommunication.InstituteId = user.InstituteId;
                    instituteCommunication.AssociatedCommunicationId = id;
                    instituteCommunication.CommunicationSourceId = fromUser.Email;


                    instituteCommunication.CreatedOn = DateTime.UtcNow;
                    instituteCommunication.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteCommunication.UpdateOn = DateTime.UtcNow;
                    instituteCommunication.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteCommunication.Add(instituteCommunication);
                    await _parentcheckContext.SaveChangesAsync();


                    InstituteCommunicationReceiver instituteCommunicationReceiver = new InstituteCommunicationReceiver();
                    instituteCommunicationReceiver.CommunicationId = instituteCommunication.Id;
                    instituteCommunicationReceiver.ToUserId = toUserId;
                    instituteCommunicationReceiver.CommunicationSourceId = toUser.Email;
                    instituteCommunicationReceiver.CreatedOn = DateTime.UtcNow;
                    instituteCommunicationReceiver.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteCommunicationReceiver.UpdateOn = DateTime.UtcNow;
                    instituteCommunicationReceiver.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteCommunicationReceiver.Add(instituteCommunicationReceiver);
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

        public async Task<List<CommunicationTemplateDTO>> GetCommunicationTemplateAsync(bool isActiveOnly,long userId)
        {
            List<CommunicationTemplateDTO> communicationTemplates = new List<CommunicationTemplateDTO>();

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
                var templates = await (from ct in _parentcheckContext.InstituteCommunicationTemplate
                                           where ct.InstituteId==user.InstituteId && 
                                           (isActiveOnly==false || ct.IsActive== isActiveOnly)
                                       select new
                                           {
                                               ct.IsSenderTemplate,
                                               ct.TemplateName,
                                               ct.TemplateContent,
                                               ct.Id,
                                               ct.IsActive
                                           }).ToListAsync();

                foreach (var template in templates)
                {
                    communicationTemplates.Add(new CommunicationTemplateDTO
                    {
                        Id = template.Id,
                        TemplateName= template.TemplateName,
                        TemplateContent= template.TemplateContent,
                        IsSenderTemplate= template.IsSenderTemplate,
                        IsActive=template.IsActive
                    });
                }
            }

            return communicationTemplates;
        }

        public async Task<bool> SaveCommunicationTemplate(long id, string name, string content, bool isSenderTemplate, bool isActive, long userId)
        {
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
                try
                {
                    InstituteCommunicationTemplate instituteCommunicationTemplate;
                    if (id > 0)
                    {
                        var communicationTemplate = _parentcheckContext.InstituteCommunicationTemplate.FirstOrDefault(i => i.Id == id);

                        if (communicationTemplate != null)
                        {
                            instituteCommunicationTemplate = communicationTemplate;
                            instituteCommunicationTemplate.TemplateName = name;
                            instituteCommunicationTemplate.TemplateContent = content;
                            instituteCommunicationTemplate.IsSenderTemplate = isSenderTemplate;
                            instituteCommunicationTemplate.IsActive = isActive;
                            instituteCommunicationTemplate.UpdateOn = DateTime.UtcNow;
                            instituteCommunicationTemplate.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteCommunicationTemplate).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteCommunicationTemplate = new InstituteCommunicationTemplate();
                    
                    instituteCommunicationTemplate.TemplateName = name;
                    instituteCommunicationTemplate.InstituteId = user.InstituteId;
                    instituteCommunicationTemplate.TemplateContent = content;
                    instituteCommunicationTemplate.IsSenderTemplate = isSenderTemplate;
                    instituteCommunicationTemplate.IsActive = isActive;
                    instituteCommunicationTemplate.CreatedOn = DateTime.UtcNow;
                    instituteCommunicationTemplate.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteCommunicationTemplate.UpdateOn = DateTime.UtcNow;
                    instituteCommunicationTemplate.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteCommunicationTemplate.Add(instituteCommunicationTemplate);
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
