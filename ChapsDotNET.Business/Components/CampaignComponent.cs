﻿using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class CampaignComponent : ICampaignComponent
    {
        private readonly DataContext _context;

        public CampaignComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This method by default returns a list of Campaigns
        /// </summary>
        /// <param name="request">CampaignRequestModel</param>
        /// <returns>A paged list of CampaignModels</returns>
        public async Task<PagedResult<List<CampaignModel>>> GetCampaignsAsync(CampaignRequestModel request)
        {
            var query = _context.Campaigns.AsQueryable();

            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Detail);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            if (!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                    .Take(request.PageSize);
            }
            var CampaignsList = await query
                .Select(x => new CampaignModel
                {
                    CampaignId = x.CampaignID,
                    Detail = x.Detail,
                    Active = x.active,
                    Deactivated = x.deactivated,
                    DeactivatedBy = x.deactivatedBy
                }).ToListAsync();

            return new PagedResult<List<CampaignModel>>
            {
                Results = CampaignsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }


        /// <summary>
        /// This method by default returns a Campaign by id
        /// </summary>
        /// <param name="id">Integer CampaigninID</param>
        /// <returns>A single CampaignModel</returns>
        public async Task<CampaignModel> GetCampaignAsync(int id)
        {
            var query = _context.Campaigns.AsQueryable();
            query = query.Where(x => x.CampaignID == id);

            var campaign = await query
                .Select(x => new CampaignModel
                {
                    CampaignId = x.CampaignID,
                    Detail = x.Detail,
                    Active = x.active,
                    Deactivated = x.deactivated,
                    DeactivatedBy = x.deactivatedBy
                }).SingleOrDefaultAsync();

            if (campaign == null)
            {
                return new CampaignModel
                {
                    Detail = string.Empty
                };
            }
            return campaign;
        }

        /// <summary>
        /// Adds a Campaign
        /// </summary>
        /// <param name="model">CampaignModel</param>
        public async Task<int> AddCampaignAsync(CampaignModel model)
        {
            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var campaign = new Campaign
            {
                active = true,
                Detail = model.Detail
            };

            await _context.Campaigns.AddAsync(campaign);
            await _context.SaveChangesAsync();
            return campaign.CampaignID;
        }

        /// <summary>
        /// Updates a Campaign
        /// </summary>
        /// <param name="model">CampaignModel</param>
        public async Task UpdateCampaignAsync(CampaignModel model)
        {
            var campaign = await _context.Campaigns.FirstOrDefaultAsync(x => x.CampaignID == model.CampaignId);

            if (campaign == null)
            {
                throw new NotFoundException("Campaign", model.CampaignId.ToString());
            }

            if (string.IsNullOrEmpty(model.Detail))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            campaign.active = model.Active;
            campaign.Detail = model.Detail;
            campaign.deactivated = model.Deactivated;
            campaign.deactivatedBy = model.DeactivatedBy;
           
            await _context.SaveChangesAsync();
        }
    }
}
