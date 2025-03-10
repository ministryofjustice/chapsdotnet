using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class EmailTemplateComponent : IEmailTemplateComponent
    {
        private readonly DataContext _context;

        public EmailTemplateComponent(DataContext context)
        {
            _context = context;
        }
        public async Task<int> AddEmailTemplateAsync(EmailTemplateModel model)
        {
            var emailTemplate = new EmailTemplate
            {
                CorrespondenceTypeID = model.CorrespondenceTypeID,
                StageID = model.StageID,
                Chaser = model.Chaser,
                Subject = model.Subject,
                BodyText = model.BodyText
            };

            await _context.EmailTemplates.AddAsync(emailTemplate);
            await _context.SaveChangesAsync();
            return emailTemplate.EmailTemplateID;
        }
        //public async Task<EmailTemplateModel> GetEmailTemplateAsync(int id)
        //{
        //    var query = _context.emailTemplates.AsQueryable();
        //    query = query.Where(x => x.emailTemplateID == id);

        //    var emailTemplate = await query.Select(x => new EmailTemplateModel
        //    {
        //        EmailTemplateId = x.emailTemplateID,
        //        CorrespondenceTypeID = x.CorrespondenceTypeID,
        //        StageID = x.StageID,
        //        Chaser = x.Chaser,
        //        Subject = x.Subject,
        //        BodyText = x.BodyText
        //    }).SingleOrDefaultAsync();

        //    if (emailTemplate == null)
        //    {
        //        return new emailTemplateModel
        //        {
        //            Subject = string.Empty
        //        };
        //    }
        //    return emailTemplate;
        //}
    }
}