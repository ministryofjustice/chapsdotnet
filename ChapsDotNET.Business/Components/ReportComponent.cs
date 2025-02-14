using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class ReportComponent : IReportComponent
    {
        private readonly DataContext _context;

        public ReportComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of ReportModels
        /// </summary>
        /// <returns>List of ReportModels</returns>
        public async Task<List<ReportModel>> GetReportsAsync()
        {
            var query = _context.Reports.AsQueryable();
            var reportsList = await query.Select(x => new ReportModel
                {
                    ReportId = x.ReportId,
                    Name = x.Name,
                    Description = x.Description,
                    LongDescription = x.LongDescription
                }).ToListAsync();
            return reportsList;
        }

        /// <summary>
        /// Returns a Report
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>A single Report</returns>
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
                    LongDescription = x.LongDescription
                }).SingleOrDefaultAsync();

            if (Report == null)
            {
                return new ReportModel
                {
                    Description = null,
                    LongDescription = null
                };
            }
            return Report;
        }

        /// <summary>
        /// Updates a Report
        /// </summary>
        /// <param name="model">ReportModel</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateReportAsync(ReportModel model)
        {
            var Report = await _context.Reports.FirstOrDefaultAsync(x => x.ReportId == model.ReportId);

            if (Report == null)
            {
                throw new NotFoundException("Report", model.ReportId.ToString());
            }
            
            if (string.IsNullOrEmpty(model.Description))
            {
                throw new ArgumentNullException(model.Description, "description field cannot be empty");
            }

            Report.Description = model.Description;
            Report.LongDescription = model.LongDescription;

            await _context.SaveChangesAsync();
        }
    }
}
