using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;


namespace ChapsDotNET.Business.Interfaces
{
    public interface IEmailTemplateComponent
    {
        Task<int> AddEmailTemplateAsync(EmailTemplateModel model);

    }
}
