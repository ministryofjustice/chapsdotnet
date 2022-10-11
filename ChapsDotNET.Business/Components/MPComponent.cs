using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Net;
using System.Xml.Linq;
using ChapsDotNET.Business.Exceptions;
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
        /// This method by default returns a list of only active MPs
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of MP Models</returns>

        public async Task<PagedResult<List<MPModel>>> GetMPsAsync(MPRequestModel request)
        {
            var query = _context.MPs.AsQueryable();

            // Filtering ------------------------------------------------------

            //if (request.nameFilterTerm != null)
            //{
            //    query = query.Where(x =>
            //                            //TODO: RtHon -> boolean
            //                            //TODO: Salutation -> foreign key≠
            //                            x.FirstNames!.Contains(request.nameFilterTerm)
            //                            || x.Surname.Contains(request.nameFilterTerm)
            //                            || x.Suffix!.Contains(request.nameFilterTerm)
            //                        ).OrderBy(x => x.Surname);
            //}
            //if (request.addressFilterTerm != null)
            //{
            //    query = query.Where(x => x.AddressLine1!.Contains(request.addressFilterTerm)
            //                            || x.AddressLine2!.Contains(request.addressFilterTerm)
            //                            || x.AddressLine3!.Contains(request.addressFilterTerm)
            //                            || x.Town!.Contains(request.addressFilterTerm)
            //                            || x.County!.Contains(request.addressFilterTerm)
            //                            || x.Postcode!.Contains(request.addressFilterTerm)
            //                        ).OrderBy(x => x.AddressLine1);
            //}
            //if (request.emailFilterTerm != null)
            //{
            //    query = query.Where(x => x.Email!.Contains(request.emailFilterTerm)).OrderBy(x => x.Email);
            //}
            //if (request.activeFilter == true)
            //{
            //    query = query.Where(x => x.active == true).OrderBy(x => x.Surname);
            //}
            //if (request.activeFilter == false)
            //{
            //    query = query.Where(x => x.active == false).OrderBy(x => x.Surname);
            //}
            //else
            //    query = query.Where( x => x.active == true ).OrderBy(x => x.Surname);

            // ----------------------------------------------------------------

            // Sorting--------------------------------------------------------

            //System.Diagnostics.Debug.WriteLine("Active filter:  " + request.activeFilter);
            //System.Diagnostics.Debug.WriteLine("Address filter: " + request.addressFilterTerm);
            //System.Diagnostics.Debug.WriteLine("email filter:   " + request.emailFilterTerm);
            //System.Diagnostics.Debug.WriteLine("Name filter:    " + request.nameFilterTerm);

            System.Diagnostics.Debug.WriteLine("Sort order:     " + request.sortOrder);

            switch (request.sortOrder)
            {
                case "address_asc":
                    query = query.OrderBy(x => x.AddressLine1);
                    break;
                case "address_desc":
                    query = query.OrderByDescending(x => x.AddressLine1);
                    break;
                case "email_asc":
                    query = query.OrderBy(x => x.Email);
                    break;
                case "email_desc":
                    query = query.OrderByDescending(x => x.Email);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(x => x.Surname);
                    break;
                default:
                    query = query.OrderBy(x => x.Surname);
                    break;
            }

            // ----------------------------------------------------------------

            //Row Count
            var count = await query.CountAsync();

            //Paging query
            query = query.Skip(((request.PageNumber) - 1) * request.PageSize).Take(request.PageSize);

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
                    Active = x.active
                }).ToListAsync();
        
            return new PagedResult<List<MPModel>>
            {
                Results = mpsList,
                PageSize = request.PageSize,
                CurrentPage = request.PageNumber,
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
