﻿using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class MPComponent : IMPComponent
    {
        private readonly DataContext _context;

        public MPComponent(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Returns a list of only active MPs
        /// </summary>
        /// <param name="request">MPRequestModel</param>
        /// <returns>A paged list of MPModels</returns>
        public async Task<PagedResult<List<MPModel>>> GetMPsAsync(MPRequestModel request)
        {
            var query = _context.MPs.AsQueryable();

            query = query
                .Where(x => x.active == true)
                .OrderBy(x => x.Surname).ThenBy(x => x.FirstNames);

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);


            // Get Mps.
            var mpsList = await query
                .Select(x => new MPModel
                {
                    MPId = x.MPID,
                    RtHon = x.RtHon,
                    SalutationId = x.SalutationID,
                    FirstNames = x.FirstNames,
                    Surname = x.Surname,
                    Email = x.Email,
                    Suffix = x.Suffix,
                    AddressLine1 = x.AddressLine1,
                    AddressLine2 = x.AddressLine2,
                    AddressLine3 = x.AddressLine3,
                    Town = x.Town,
                    County = x.County,
                    Postcode = x.Postcode,
                    Active = x.active,
                }).ToListAsync();

            return new PagedResult<List<MPModel>>
            {
                Results = mpsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
                RowCount = count
            };
        }

        /// <summary>
        /// Returns a list of MPs filtered by terms entered
        /// </summary>
        /// <param name="model">MPRequestModel</param>
        /// <returns>A paged list of MP Models</returns>
        public async Task<PagedResult<List<MPModel>>> GetFilteredMPsAsync(MPRequestModel model)
        {
            var query = _context.MPs.AsQueryable();

            if (!string.IsNullOrEmpty(model.nameFilterTerm))
            {
                query = query.Where(x =>
                x.FirstNames != null && x.FirstNames.Contains(model.nameFilterTerm) ||
                x.Surname.Contains(model.nameFilterTerm));
            }

            if (!string.IsNullOrEmpty(model.addressFilterTerm))
            {
                query = query.Where(x =>
                x.AddressLine1!.Contains(model.addressFilterTerm) ||
                x.AddressLine2!.Contains(model.addressFilterTerm) ||
                x.AddressLine3!.Contains(model.addressFilterTerm) ||
                x.Town != null && x.Town.Contains(model.addressFilterTerm) ||
                x.County != null && x.County.Contains(model.addressFilterTerm) ||
                x.Postcode != null && x.Postcode.Contains(model.addressFilterTerm));
            }

            if (!string.IsNullOrEmpty(model.emailFilterTerm))
            {
                query = query.Where(x => x.Email != null && x.Email.Contains(model.emailFilterTerm));
            }

            if (!model.ShowActiveAndInactive)
            {
                query = query.Where(x => x.active == true);
            }

            query = (model.sortOrder) switch
            {
                ("name") => query.OrderBy(x => x.Surname).ThenBy(x => x.FirstNames),
                ("name_desc") => query.OrderByDescending(x => x.Surname).ThenByDescending(x => x.FirstNames),
                ("address") => query.OrderBy(x => x.AddressLine1),
                ("address_desc") => query.OrderByDescending(x => x.AddressLine1),
                ("email") => query.OrderBy(x => x.Email),
                ("email_desc") => query.OrderByDescending(x => x.Email),
                ("deactiv") => query.OrderBy(x => x.deactivatedOn),
                ("deactiv_desc") => query.OrderByDescending(x => x.deactivatedOn),
                _ => query.OrderBy(x => x.Surname).ThenBy(x => x.FirstNames)
            };

            int totalCount = await query.CountAsync();

            query = query
                .Skip((model.PageNumber - 1) * model.PageSize)
                .Take(model.PageSize);

            // get Mps with any users that deactivated them
            var mpsWithUsers = await
                (from mp in query
                    join user in _context.Users
                    on mp.deactivatedByID equals user.UserID
                    into groupJoin
                        from subUser in groupJoin.DefaultIfEmpty()
                            select new
                            {
                                MP = mp,
                                User = subUser
                            }).ToListAsync();

            var mpsList = mpsWithUsers
                .Select(x => new MPModel
                {
                    MPId = x.MP.MPID,
                    RtHon = x.MP.RtHon,
                    SalutationId = x.MP.SalutationID,
                    FirstNames = x.MP.FirstNames,
                    Surname = x.MP.Surname,
                    Email = x.MP.Email,
                    Suffix = x.MP.Suffix,
                    AddressLine1 = x.MP.AddressLine1,
                    AddressLine2 = x.MP.AddressLine2,
                    AddressLine3 = x.MP.AddressLine3,
                    Town = x.MP.Town,
                    County = x.MP.County,
                    Postcode = x.MP.Postcode,
                    Active = x.MP.active,
                    DeactivatedByID = x.MP.deactivatedByID,
                    DeactivatedOn = x.MP.deactivatedOn,
                    DeactivatedDisplay = x.User != null ? $"{x.User.DisplayName} on {x.MP.deactivatedOn:dd/MM/yyyy HH:mm:ss}" : null
                }).ToList();

            return new PagedResult<List<MPModel>>
            {
                Results = mpsList,
                CurrentPage = model.PageNumber,
                PageSize = model.PageSize,
                RowCount = totalCount
            };
        }

        /// <summary>
        /// Returns an MPmodel by id 
        /// </summary>
        /// <param name="id">Int MPID</param>
        /// <returns>An MP Model</returns>
        public async Task<MPModel> GetMPAsync(int id)
        {
            var query = _context.MPs.AsQueryable();
            query = query.Where(x => x.MPID == id);

            var mp = await query.Select(x => new MPModel
            {
                MPId = x.MPID,
                RtHon = x.RtHon,
                SalutationId = x.SalutationID,
                FirstNames = x.FirstNames,
                Surname = x.Surname,
                Suffix = x.Suffix,
                Email = x.Email,
                Active = x.active,
                AddressLine1 = x.AddressLine1,
                AddressLine2 = x.AddressLine2,
                AddressLine3 = x.AddressLine3,
                Town = x.Town,
                County = x.County,
                Postcode = x.Postcode,
                DeactivatedByID = x.deactivatedByID,
                DeactivatedOn = x.deactivatedOn
            }).SingleOrDefaultAsync();

            if (mp == null)
            {
                return new MPModel
                {
                    Surname = string.Empty
                };
            }
            return mp;
        }

        /// <summary>
        /// Adds an MP 
        /// </summary>
        /// <param name="model">MPModel</param>
        /// <returns>Int</returns>
        public async Task<int> AddMPAsync(MPModel model)
        {
            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new ArgumentNullException("Parameter Surname cannot be empty");
            }

            var mp = new MP
            {
                RtHon = model.RtHon,
                SalutationID = model.SalutationId,
                FirstNames = model.FirstNames,
                Surname = model.Surname,
                Suffix = model.Suffix,
                Email = model.Email,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AddressLine3 = model.AddressLine3,
                County = model.County,
                Town = model.Town,
                Postcode = model.Postcode,
                active = true,
            };

            await _context.MPs.AddAsync(mp);
            await _context.SaveChangesAsync();
            return mp.MPID;
        }

        /// <summary>
        /// Updates an MP
        /// </summary>
        /// <param name="model">MPModel</param>
        public async Task UpdateMPAsync(MPModel model)
        {
            var mp = await _context.MPs.FirstOrDefaultAsync(x => x.MPID == model.MPId);

            if (mp == null)
            {
                throw new NotFoundException("mp", model.MPId.ToString());
            }

            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
            }

            mp.RtHon = model.RtHon;
            mp.SalutationID = model.SalutationId;
            mp.FirstNames = model.FirstNames;
            mp.Surname = model.Surname;
            mp.Suffix = model.Suffix;
            mp.Email = model.Email;
            mp.AddressLine1 = model.AddressLine1;
            mp.AddressLine2 = model.AddressLine2;
            mp.AddressLine3 = model.AddressLine3;
            mp.County = model.County;
            mp.Town = model.Town;
            mp.Postcode = model.Postcode;
            mp.active = model.Active;
            mp.deactivatedByID = model.DeactivatedByID;
            mp.deactivatedOn = model.DeactivatedOn;

            _context.MPs.Update(mp);
            await _context.SaveChangesAsync();
        }
    }
}
