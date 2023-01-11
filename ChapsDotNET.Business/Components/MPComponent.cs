using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using ChapsDotNET.Business.Exceptions;
using ChapsDotNET.Business.Interfaces;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;
using ChapsDotNET.Data.Contexts;
using ChapsDotNET.Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ChapsDotNET.Business.Components
{
    public class MPComponent : IMPComponent
    {
        private readonly DataContext _context;
        private readonly ISalutationComponent _salutationComponent;

        public MPComponent(DataContext context, ISalutationComponent salutationComponent)
        {
            _context = context;
            _salutationComponent = salutationComponent;
        }

        /// <summary>
        /// This method by default returns a list of only active MPs
        /// </summary>
        /// <param name="request"></param>
        /// <returns>A list of MP Models</returns>

        public async Task<PagedResult<List<MPModel>>> GetMPsAsync(MPRequestModel request)
        {
            var query = _context.MPs.Include(x => x.Salutation).AsQueryable();

            // Filtering

            if (!string.IsNullOrWhiteSpace(request.NameFilterTerm)) {
                bool includeRtHonourables = false;
                string name = request.NameFilterTerm.Replace(" ", "").ToLower();
                string characterPattern = "";

                switch (name.Length)
                {
                    case 1:
                        characterPattern = @"[rthon]";
                        break;
                    case 2:
                        characterPattern = @"(rt|th|ho|on)";
                        break;
                    case 3:
                        characterPattern = @"(rth|tho|hon)";
                        break;
                    case 4:
                        characterPattern = @"(rtho|thon)";
                        break;
                    default:
                        characterPattern = @"(rthon)";
                        break;
                }

                Regex isRtHonourable = new Regex(characterPattern);
                includeRtHonourables = isRtHonourable.IsMatch(name);

                if (includeRtHonourables)
                {
                    query = query
                        .Where(x => (x.Salutation!.Detail + x.FirstNames! + x.Surname + x.Suffix!).Replace(" ", "").ToLower()
                        .Contains(name) || x.RtHon.Equals(true));
                }
                else
                {
                    query = query
                        .Where(x => (x.Salutation!.Detail + x.FirstNames! + x.Surname + x.Suffix!).Replace(" ", "").ToLower()
                        .Contains(name));
                }
            }

            if (!string.IsNullOrWhiteSpace(request.AddressFilterTerm))
            {
                query = query
                    .Where(x => (x.AddressLine1! + x.AddressLine2! + x.AddressLine3! + x.Town! + x.County! + x.Postcode!).Replace(" ", "").ToLower()
                    .Contains(request.AddressFilterTerm.Replace(" ", "").ToLower())
                );
            }

            if (!string.IsNullOrWhiteSpace(request.EmailFilterTerm))
            {
                query = query
                    .Where(x => x.Email!.ToLower()
                    .Contains(request.EmailFilterTerm.ToLower()));
            }

            if (request.ActiveFilter == true)
            {
                query = query
                    .Where(x => x.active.Equals(true));
            }

            query = query
                .OrderBy(x => x.Surname)
                .ThenBy(x => x.FirstNames);

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
                ActiveFilter = request.ActiveFilter,
                AddressFilterTerm = request.AddressFilterTerm,
                CurrentPage = request.PageNumber,
                EmailFilterTerm = request.EmailFilterTerm,
                NameFilterTerm = request.NameFilterTerm,
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

        public async Task<string> DisplayFullName(int id)
        {
            var mpmodel = await GetMPAsync(id);
            string? salutation = null;
            if (mpmodel.SalutationId != null)
            {
                salutation = _salutationComponent.GetSalutationAsync((int)mpmodel.SalutationId).Result.Detail;
            }
            else
            {
                salutation = String.Empty;
            }
           
            List<string> nameParts = new List<string>();

            if (mpmodel.RtHon == true)
            {
                nameParts.Add("Rt Hon");
            }

            nameParts.Add(salutation!);
            nameParts.Add(mpmodel.FirstNames != null ? mpmodel.FirstNames : null!);
            nameParts.Add(mpmodel.Surname);
            nameParts.Add(mpmodel.Suffix != null ? mpmodel.Suffix : null!);
            var tep = string.Join(" ", nameParts.Where(s => !string.IsNullOrEmpty(s)));
            return string.Join(" ", nameParts.Where(s => !string.IsNullOrEmpty(s)));
        }
    }
}
