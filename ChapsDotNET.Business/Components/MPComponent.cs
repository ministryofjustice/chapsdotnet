using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection.Emit;
using System.Text;  
using System.Xml.Linq;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

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
        /// This method by default returns a list of only active MPs
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of MP Models</returns>

        public async Task<PagedResult<List<MPModel>>> GetMPsAsync(MPRequestModel request)
        {
            var query = _context.MPs.Include(x => x.Salutation).AsQueryable();


            // Filtering ------------------------------------------------------

            if (
                string.IsNullOrWhiteSpace(request.nameFilterTerm) &&
                string.IsNullOrWhiteSpace(request.addressFilterTerm) &&
                string.IsNullOrWhiteSpace(request.emailFilterTerm) &&
                request.activeFilter.Equals(null)
                )
            {
                    query = query.Where(x => x.active.Equals(true));
            }
            else
            {
                if ( !string.IsNullOrWhiteSpace(request.nameFilterTerm) ) {
                    query = query.Where(
                        x => (
                            x.Salutation!.Detail??"".Concat(x.FirstNames!).Concat(x.Surname).Concat(x.Suffix!)
                        )
                        .ToString()
                        .ToLower()
                        .Contains(request.nameFilterTerm.ToLower())
                    );
                }

                if ( !string.IsNullOrWhiteSpace(request.addressFilterTerm )) {
                    query = query.Where(
                        x => x.AddressLine1!.ToLower().Contains(request.addressFilterTerm.ToLower()) ||
                        x.AddressLine2!.ToLower().Contains(request.addressFilterTerm.ToLower()) ||
                        x.AddressLine3!.ToLower().Contains(request.addressFilterTerm.ToLower()) ||
                        x.Town!.ToLower().Contains(request.addressFilterTerm.ToLower()) ||
                        x.County!.ToLower().Contains(request.addressFilterTerm.ToLower()) ||
                        x.Postcode!.ToLower().Contains(request.addressFilterTerm.ToLower())
                    );
                }

                if ( !string.IsNullOrWhiteSpace(request.emailFilterTerm) ) {
                    query = query.Where( x => x.Email!.ToLower().Contains(request.emailFilterTerm.ToLower()) );
                }

                if ( request.activeFilter == true ) {
                    query = query.Where( x => x.active.Equals(true) );
                }

                if ( request.activeFilter == false ) {
                    query = query.Where( x => x.active.Equals(false) );
                }
            }

            query = query.OrderBy(x => x.Surname).ThenBy(x => x.FirstNames);

            // ----------------------------------------------------------------

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query.Skip(((request.PageNumber) - 1) * request.PageSize).Take(request.PageSize);

            var mpsList = await query.Select(x => new MPModel
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
                    Active = x.active
                }
            ).ToListAsync();

            return new PagedResult<List<MPModel>>
            {
                activeFilter = request.activeFilter,
                addressFilterTerm = request.addressFilterTerm,
                CurrentPage = request.PageNumber,
                emailFilterTerm = request.emailFilterTerm,
                nameFilterTerm = request.nameFilterTerm,
                PageSize = request.PageSize,
                Results = mpsList,
                RowCount = count
            };
        }

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
                    Postcode = x.Postcode
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

        public async Task<int> AddMPAsync(MPModel model)
        {
            if (string.IsNullOrEmpty(model.Surname))
            {
                throw new ArgumentNullException("Parameter Detail cannot be empty");
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
                active = true
            };

            await _context.MPs.AddAsync(mp);
            await _context.SaveChangesAsync();
            return mp.MPID;
        }

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

            _context.MPs.Update(mp);
            await _context.SaveChangesAsync();
        }
    }
}
