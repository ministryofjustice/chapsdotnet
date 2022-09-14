using ChapsDotNET.Business.Models;
using ChapsDotNET.Models;

namespace ChapsDotNET.Common.Mappers
{
    public static class MappingExtension
    {
        public static CampaignModel ToModel(this CampaignViewModel viewModel)
        {
            return new CampaignModel
            {
                Detail = viewModel.Detail,
                Active = viewModel.Active,
                CampaignId = viewModel.CampaignId
            };
        }

        public static CampaignViewModel ToViewModel(this CampaignModel model)
        {
            return new CampaignViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                CampaignId = model.CampaignId
            };
        }

        public static LeadSubjectModel ToModel(this LeadSubjectViewModel viewModel)
        {
            return new LeadSubjectModel
            {
                Detail = viewModel.Detail,
                Active = viewModel.Active,
                LeadSubjectId = viewModel.LeadSubjectId,
            };
        }

        public static LeadSubjectViewModel ToViewModel(this LeadSubjectModel model)
        {
            return new LeadSubjectViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                LeadSubjectId = model.LeadSubjectId,
            };
        }

        public static MoJMinisterModel ToModel(this MoJMinisterViewModel viewModel)
        {
            return new MoJMinisterModel
            {
                Active = viewModel.Active,
                MoJMinisterId = viewModel.MoJMinisterId,
                Name = viewModel.Name,
                Prefix = viewModel.Prefix,
                Rank = viewModel.Rank,
                Suffix = viewModel.Suffix
            };
        }

        public static MoJMinisterViewModel ToViewModel(this MoJMinisterModel model)
        {
            return new MoJMinisterViewModel
            {
                Active = model.Active,
                MoJMinisterId = model.MoJMinisterId,
                Name = model.Name,
                Prefix = model.Prefix,
                Rank = model.Rank,
                Suffix = model.Suffix
            };
        }

        public static MPModel ToModel(this MPViewModel viewModel)
        {
            return new MPModel
            {
                MPId = viewModel.MPId,
                SalutationID = viewModel.SalutationID,
                Surname = viewModel.Surname,
                FirstNames = viewModel.FirstNames,
                AddressLine1 = viewModel.AddressLine1,
                AddressLine2 = viewModel.AddressLine2,
                AddressLine3 = viewModel.AddressLine3,
                Town = viewModel.Town,
                County = viewModel.County,
                Postcode = viewModel.Postcode,
                Email = viewModel.Email,
                RtHon = viewModel.RtHon,
                Suffix = viewModel.Suffix,
                Active = viewModel.Active
            };
        }

        public static MPViewModel ToViewModel(this MPModel model)
        {
            return new MPViewModel
            {
                MPId = model.MPId,
                SalutationID = model.SalutationID,
                Surname = model.Surname,
                FirstNames = model.FirstNames,
                AddressLine1 = model.AddressLine1,
                AddressLine2 = model.AddressLine2,
                AddressLine3 = model.AddressLine3,
                Town = model.Town,
                County = model.County,
                Postcode = model.Postcode,
                Email = model.Email,
                RtHon = model.RtHon,
                Suffix = model.Suffix,
                Active = model.Active
            };
        }

        public static PublicHolidayModel ToModel(this PublicHolidayViewModel viewModel)
        {
            return new PublicHolidayModel
            {
                Description = viewModel.Description,
                Date = viewModel.Date,
                PublicHolidayId = viewModel.PublicHolidayId,
            };
        }

        public static PublicHolidayViewModel ToViewModel(this PublicHolidayModel model)
        {
            return new PublicHolidayViewModel
            {
                Description = model.Description,
                Date = model.Date,
                PublicHolidayId = model.PublicHolidayId,
            };
        }

        public static SalutationModel ToModel(this SalutationViewModel viewModel)
        {
            return new SalutationModel
            {
                Detail = viewModel.Detail,
                Active = viewModel.Active,
                SalutationId = viewModel.SalutationId,
            };
        }

        public static SalutationViewModel ToViewModel(this SalutationModel model)
        {
            return new SalutationViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                SalutationId = model.SalutationId,
            };
        }

        public static TeamModel ToModel(this TeamViewModel viewModel)
        {
            return new TeamModel
            {
                Acronym = viewModel.Acronym,
                Active = viewModel.Active,
                TeamId = viewModel.TeamId,
                Email = viewModel.Email,
                IsOgd = viewModel.IsOgd,
                IsPod = viewModel.IsPod,
                Name = viewModel.Name
            };
        }

        public static TeamViewModel ToViewModel(this TeamModel model)
        {
            return new TeamViewModel
            {
                Acronym= model.Acronym,
                Active = model.Active,
                TeamId= model.TeamId,
                Email = model.Email,
                IsOgd = model.IsOgd,
                IsPod = model.IsPod,
                Name = model.Name
            };
        }        
    }
}
