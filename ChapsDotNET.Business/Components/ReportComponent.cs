using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class ReportComponent : IReportComponent
    {
        private readonly DataContext _context;

        public ReportComponent(DataContext context)
        {
            this._context = context;
        }

        public async Task<ReportModel> GetReportAsync(int id)
        {
            var query = _context.Reports.AsQueryable();
            query = query.Where(x => x.ReportId == id);

            var Report = await query
                .Select(x => new ReportModel
                {
                    ReportId = x.ReportId,
                    Name = x.Name,
                    Description = x.Description,
                }).SingleOrDefaultAsync();

            if (Report == null)
            {
                return new ReportModel
                {
                    Description = null
                };
            }
            return Report;
        }


        public async Task UpdateReportAsync(ReportModel model)
        {
            var Report = await _context.Reports.FirstOrDefaultAsync(x => x.ReportId == model.ReportId);

            if (Report == null)
            {
                throw new NotFoundException("Report", model.ReportId.ToString());
            }

            
            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException(model.Description, "cannot be empty");
            }

            Report.Description = model.Description;

            await _context.SaveChangesAsync();
        }
    }
}
