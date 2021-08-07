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
    public class PaymentRepository : IPaymentRepository
    {
        private ParentCheckContext _parentcheckContext;

        public PaymentRepository(ParentCheckContext parentcheckContext)
        {
            _parentcheckContext = parentcheckContext;
        }

        public async Task<List<InvoiceDTO>> GetInvoiceAsync(bool isGenerated, long userId)
        {
            List<InvoiceDTO> invoiceDTOs = new List<InvoiceDTO>();

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
                if (isGenerated)
                {
                    var invoices = await (from i in _parentcheckContext.InstituteInvoice
                                          join s in _parentcheckContext.Status on i.StatusId equals s.Id
                                          join it in _parentcheckContext.InstituteInvoiceType on i.InvoiceTypeId equals it.Id
                                          where i.InstituteId == user.InstituteId
                                          && s.StatueTypeCode == "invoice"
                                          select new
                                          {
                                              i.Id,
                                              i.InvoiceAmount,
                                              i.InvoiceDate,
                                              i.InvoiceNo,
                                              i.InvoiceTitle,
                                              i.InvoiceDetails,
                                              i.DueDate,
                                              s.StatusText,
                                              it.InvoiceTypeText
                                          }).ToListAsync();

                    foreach (var invoice in invoices)
                    {
                        invoiceDTOs.Add(new InvoiceDTO
                        {
                            Id = invoice.Id,
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceDate = invoice.InvoiceDate,
                            DueDate = invoice.DueDate,
                            InvoiceTitle = invoice.InvoiceTitle,
                            InvoiceDetails = invoice.InvoiceDetails,
                            InvoiceAmount = invoice.InvoiceAmount,
                            StatusText = invoice.StatusText,
                            InvoiceType = invoice.InvoiceTypeText,
                            DueAmount = 0,
                            PaidAmount = 0
                        });
                    }
                }
                else
                {
                    var invoices = await (from id in _parentcheckContext.InstituteInvoiceDetail
                                          join i in _parentcheckContext.InstituteInvoice on id.InvoiceId equals i.Id
                                          join s in _parentcheckContext.Status on i.StatusId equals s.Id
                                          join it in _parentcheckContext.InstituteInvoiceType on i.InvoiceTypeId equals it.Id
                                          where id.InstituteUserId == user.Id
                                          && s.StatueTypeCode == "invoice"
                                          select new
                                          {
                                              i.Id,
                                              i.InvoiceAmount,
                                              i.InvoiceDate,
                                              i.InvoiceNo,
                                              i.InvoiceTitle,
                                              i.InvoiceDetails,
                                              i.DueDate,
                                              s.StatusText,
                                              it.InvoiceTypeText,
                                              id.PaidAmount,
                                              id.DueAmount
                                          }).ToListAsync();

                    foreach (var invoice in invoices)
                    {
                        invoiceDTOs.Add(new InvoiceDTO
                        {
                            Id = invoice.Id,
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceDate = invoice.InvoiceDate,
                            DueDate = invoice.DueDate,
                            InvoiceTitle = invoice.InvoiceTitle,
                            InvoiceDetails = invoice.InvoiceDetails,
                            InvoiceAmount = invoice.InvoiceAmount,
                            StatusText = invoice.StatusText,
                            InvoiceType = invoice.InvoiceTypeText,
                            DueAmount = invoice.DueAmount,
                            PaidAmount = invoice.PaidAmount,
                            InvoiceUserName = $"{user.FirstName} {user.LastName}"
                        });
                    }
                }
            }

            return invoiceDTOs;
        }

        public async Task<InvoiceDTO> GetInvoiceDetailAsync(bool isGenerated, long invoiceId, long userId)
        {
            InvoiceDTO invoiceDTO = new InvoiceDTO();

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
                if (isGenerated)
                {
                    var invoice = await (from i in _parentcheckContext.InstituteInvoice
                                         join s in _parentcheckContext.Status on i.StatusId equals s.Id
                                         join it in _parentcheckContext.InstituteInvoiceType on i.InvoiceTypeId equals it.Id
                                         where i.InstituteId == user.InstituteId
                                         && s.StatueTypeCode == "invoice"
                                         && i.Id == invoiceId
                                         select new
                                         {
                                             i.Id,
                                             i.InvoiceAmount,
                                             i.InvoiceDate,
                                             i.InvoiceNo,
                                             i.InvoiceTitle,
                                             i.InvoiceDetails,
                                             i.DueDate,
                                             s.StatusText,
                                             it.InvoiceTypeText
                                         }).FirstOrDefaultAsync();

                    if (invoice != null)
                    {
                        invoiceDTO = new InvoiceDTO
                        {
                            Id = invoice.Id,
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceDate = invoice.InvoiceDate,
                            DueDate = invoice.DueDate,
                            InvoiceTitle = invoice.InvoiceTitle,
                            InvoiceDetails = invoice.InvoiceDetails,
                            InvoiceAmount = invoice.InvoiceAmount,
                            StatusText = invoice.StatusText,
                            InvoiceType = invoice.InvoiceTypeText,
                            DueAmount = 0,
                            PaidAmount = 0
                        };
                    }
                }
                else
                {
                    var invoice = await (from id in _parentcheckContext.InstituteInvoiceDetail
                                         join i in _parentcheckContext.InstituteInvoice on id.InvoiceId equals i.Id
                                         join s in _parentcheckContext.Status on i.StatusId equals s.Id
                                         join it in _parentcheckContext.InstituteInvoiceType on i.InvoiceTypeId equals it.Id
                                         where id.InstituteUserId == user.Id
                                         && s.StatueTypeCode == "invoice"
                                         && i.Id == invoiceId
                                         select new
                                         {
                                             i.Id,
                                             i.InvoiceAmount,
                                             i.InvoiceDate,
                                             i.InvoiceNo,
                                             i.InvoiceTitle,
                                             i.InvoiceDetails,
                                             i.DueDate,
                                             s.StatusText,
                                             it.InvoiceTypeText,
                                             id.PaidAmount,
                                             id.DueAmount
                                         }).FirstOrDefaultAsync();

                    if (invoice != null)
                    {
                        invoiceDTO = new InvoiceDTO
                        {
                            Id = invoice.Id,
                            InvoiceNo = invoice.InvoiceNo,
                            InvoiceDate = invoice.InvoiceDate,
                            DueDate = invoice.DueDate,
                            InvoiceTitle = invoice.InvoiceTitle,
                            InvoiceDetails = invoice.InvoiceDetails,
                            InvoiceAmount = invoice.InvoiceAmount,
                            StatusText = invoice.StatusText,
                            InvoiceType = invoice.InvoiceTypeText,
                            DueAmount = invoice.DueAmount,
                            PaidAmount = invoice.PaidAmount,
                            InvoiceUserName = $"{user.FirstName} {user.LastName}"
                        };
                    }
                }
            }

            return invoiceDTO;
        }

        public async Task<bool> GenerateInvoiceAsync(string invoiceTitle, string invoiceDetails, List<UserContactDTO> toUsers, DateTime dueDate, DateTime invoiceDate, decimal invoiceAmount, int invoiceTypeId, long userId)
        {
            InvoiceDTO invoiceDTO = new InvoiceDTO();

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
                using (var dbContextTransaction = _parentcheckContext.Database.BeginTransaction())
                {
                    try
                    {
                        var invoicePrefix = _parentcheckContext.InstitutePrefix.FirstOrDefault(i => i.InstituteId == user.InstituteId && i.PrefixType == "invoice");

                        if (invoicePrefix == null)
                        {
                            return false;
                        }

                        InstituteInvoice instituteInvoice = new InstituteInvoice();
                        instituteInvoice.InvoiceTitle = invoiceTitle;
                        instituteInvoice.InvoiceDetails = invoiceDetails;
                        instituteInvoice.InvoiceDate = invoiceDate;
                        instituteInvoice.InvoiceNo = $"{invoicePrefix.PrefixCode}{invoicePrefix.NextNumber}";
                        instituteInvoice.DueDate = dueDate;
                        instituteInvoice.StatusId = (int)EnumStatus.Sent;
                        instituteInvoice.InvoiceTitle = invoiceTitle;
                        instituteInvoice.InvoiceAmount = invoiceAmount;
                        instituteInvoice.InstituteId = user.InstituteId;
                        instituteInvoice.InvoiceTypeId = invoiceTypeId;
                        instituteInvoice.GeneratedBy = user.Id;
                        instituteInvoice.CreatedOn = DateTime.UtcNow;
                        instituteInvoice.CreatedBy = $"{user.FirstName} {user.LastName}";
                        instituteInvoice.UpdateOn = DateTime.UtcNow;
                        instituteInvoice.UpdatedBy = $"{user.FirstName} {user.LastName}";

                        _parentcheckContext.InstituteInvoice.Add(instituteInvoice);
                        await _parentcheckContext.SaveChangesAsync();

                        foreach (var toUser in toUsers)
                        {
                            InstituteInvoiceDetail instituteInvoiceDetail = new InstituteInvoiceDetail();
                            instituteInvoiceDetail.InvoiceId = instituteInvoice.Id;
                            instituteInvoiceDetail.InvoiceAmount = invoiceAmount;
                            instituteInvoiceDetail.PaidAmount = 0;
                            instituteInvoiceDetail.DueAmount = invoiceAmount;
                            instituteInvoiceDetail.InstituteUserId = toUser.UserId;
                            instituteInvoiceDetail.CreatedOn = DateTime.UtcNow;
                            instituteInvoiceDetail.CreatedBy = $"{user.FirstName} {user.LastName}";
                            instituteInvoiceDetail.UpdateOn = DateTime.UtcNow;
                            instituteInvoiceDetail.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.InstituteInvoiceDetail.Add(instituteInvoiceDetail);
                            await _parentcheckContext.SaveChangesAsync();
                        }

                        invoicePrefix.NextNumber = invoicePrefix.NextNumber + 1;
                        _parentcheckContext.Entry(invoicePrefix).State = EntityState.Modified;
                        await _parentcheckContext.SaveChangesAsync();

                        await dbContextTransaction.CommitAsync();
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public async Task<List<InvoiceTypeDTO>> GetInvoiceTypesAsync(long userId)
        {
            List<InvoiceTypeDTO> invoiceTypeDTO = new List<InvoiceTypeDTO>();

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
                var invoiceTypes = await (from i in _parentcheckContext.InstituteInvoiceType
                                          where i.InstituteId == user.InstituteId
                                          select new
                                          {
                                              i.Id,
                                              i.InvoiceTypeText,
                                              i.NumbersOfTerms,
                                              i.IsActive
                                          }).ToListAsync();

                foreach (var invoiceType in invoiceTypes)
                {
                    invoiceTypeDTO.Add(new InvoiceTypeDTO
                    {
                        Id = invoiceType.Id,
                        InvoiceTypeText = invoiceType.InvoiceTypeText,
                        NumbersOfTerms = invoiceType.NumbersOfTerms,
                        IsActive = invoiceType.IsActive
                    });
                }
            }

            return invoiceTypeDTO;
        }
        public async Task<bool> InvoiceTypeSaveAsync(long id, string typeText, int terms, bool isActive, long userId)
        {
            InvoiceDTO invoiceDTO = new InvoiceDTO();

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
                try
                {

                    InstituteInvoiceType instituteInvoiceType;

                    if (id > 0)
                    {
                        var invoiceType = _parentcheckContext.InstituteInvoiceType.FirstOrDefault(i => i.Id == id);

                        if (invoiceType != null)
                        {
                            instituteInvoiceType = invoiceType;
                            instituteInvoiceType.InvoiceTypeText = typeText;
                            instituteInvoiceType.NumbersOfTerms = terms;
                            instituteInvoiceType.IsActive = isActive;
                            instituteInvoiceType.UpdateOn = DateTime.UtcNow;
                            instituteInvoiceType.UpdatedBy = $"{user.FirstName} {user.LastName}";

                            _parentcheckContext.Entry(instituteInvoiceType).State = EntityState.Modified;
                            await _parentcheckContext.SaveChangesAsync();
                            return true;
                        }

                    }

                    instituteInvoiceType = new InstituteInvoiceType();
                    instituteInvoiceType.InvoiceTypeText = typeText;
                    instituteInvoiceType.NumbersOfTerms = terms;
                    instituteInvoiceType.IsActive = true;
                    instituteInvoiceType.InstituteId = user.InstituteId;
                    instituteInvoiceType.CreatedOn = DateTime.UtcNow;
                    instituteInvoiceType.CreatedBy = $"{user.FirstName} {user.LastName}";
                    instituteInvoiceType.UpdateOn = DateTime.UtcNow;
                    instituteInvoiceType.UpdatedBy = $"{user.FirstName} {user.LastName}";

                    _parentcheckContext.InstituteInvoiceType.Add(instituteInvoiceType);
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
