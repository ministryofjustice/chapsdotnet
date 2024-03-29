﻿using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class MoJMinisterComponent : IMoJMinisterComponent
    {
        private readonly DataContext _context;

        public MoJMinisterComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a paged list of MoJ Ministers
        /// </summary>
        /// <param name="request">MoJMinisterRequestModel</param>
        /// <returns>A paged list of MoJMinisterModels</returns>
        public async Task<PagedResult<List<MoJMinisterModel>>> GetMoJMinistersAsync(MoJMinisterRequestModel request)
        {
            var query = _context.MoJMinisters.AsQueryable();

            if (!request.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = query.OrderBy(x => x.Name);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            if(!request.NoPaging)
            {
                query = query.Skip(((request.PageNumber) - 1) * request.PageSize)
                    .Take(request.PageSize);
            }


            var MoJMinistersList = await query
                .Select(x => new MoJMinisterModel
                {
                    Active = x.active,
                    MoJMinisterId = x.MoJMinisterID,
                    Name = x.Name,
                    Prefix = x.prefix,
                    Rank = x.Rank,
                    Suffix = x.suffix,
                    Deactivated = x.deactivated,
                    DeactivatedBy = x.deactivatedBy
                }).ToListAsync();

            return new PagedResult<List<MoJMinisterModel>>
            {
                Results = MoJMinistersList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        /// <summary>
        /// Gets a single MoJMinister by ID
        /// </summary>
        /// <param name="id">Integer MoJMinisterID</param>
        /// <returns>A single MoJministerModel</returns>
        public async Task<MoJMinisterModel> GetMoJMinisterAsync(int id)
        {
            var query = _context.MoJMinisters.AsQueryable();
            query = query.Where(x => x.MoJMinisterID == id);

            var mojMinister = await query   
                .Select(x => new MoJMinisterModel
                {
                    Active = x.active,
                    MoJMinisterId = x.MoJMinisterID,
                    Name = x.Name,
                    Prefix = x.prefix,
                    Rank = x.Rank,
                    Suffix = x.suffix,
                    Deactivated = x.deactivated,
                    DeactivatedBy = x.deactivatedBy
                }).SingleOrDefaultAsync();

            if (mojMinister == null)
            {
                return new MoJMinisterModel
                {
                    Name = string.Empty,
                    Prefix = null,
                    Rank = null,
                    Suffix = null
                };
            }
            return mojMinister;
        }

        /// <summary>
        /// Adds a MoJMinister
        /// </summary>
        /// <param name="model">MoJMinisterModel</param>
        /// <returns>Int</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<int> AddMoJMinisterAsync(MoJMinisterModel model)
        {
            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            var mojMinister = new MoJMinister
            {
                active = true,
                Name = model.Name,
                prefix = model.Prefix,
                Rank = model.Rank,
                suffix = model.Suffix
            };

            await _context.MoJMinisters.AddAsync(mojMinister);
            await _context.SaveChangesAsync();

            return mojMinister.MoJMinisterID;
        }

        /// <summary>
        /// Updates a single MojMinister
        /// </summary>
        /// <param name="model">MoJMinisterModel</param>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task UpdateMoJMinisterAsync(MoJMinisterModel model)
        {
            var mojMinister = await _context.MoJMinisters.FirstOrDefaultAsync(x => x.MoJMinisterID == model.MoJMinisterId);

            if (mojMinister == null)
            {
                throw new NotFoundException("MoJMinister", model.MoJMinisterId.ToString());
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            mojMinister.active = model.Active;
            mojMinister.Name = model.Name;
            mojMinister.prefix = model.Prefix;
            mojMinister.Rank = model.Rank;
            mojMinister.suffix = model.Suffix;
            mojMinister.deactivated = model.Deactivated;
            mojMinister.deactivatedBy = model.DeactivatedBy;

            await _context.SaveChangesAsync();
        }
    }
}
