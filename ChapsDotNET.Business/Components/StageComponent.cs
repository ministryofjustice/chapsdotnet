using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class StageComponent(DataContext context) : IStageComponent
    {


        /// <summary>
        /// Returns a single StageModel
        /// </summary>
        /// <param name="id">Int</param>
        /// <returns>A StageModel</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<StageModel> GetStageAsync(int id)
        {
            var Stage = await context.Stages
                .FirstOrDefaultAsync(x => x.StageID == id);

            if (Stage == null) throw new NotFoundException("Stage", id.ToString());

            return new StageModel
            {
                StageID = Stage.StageID, // Corrected property name
                Name = Stage.Name
            };
        }

        public async Task<List<StageModel>> GetStagesByCorrespondentType(int id)
        {
            // TODO: Implement logic to get stages by CorrespondentTypeID  (Relies upon wfpaths dependency)
            await Task.CompletedTask;
            return new List<StageModel>();
        }
    }
}
