using ChapsDotNET.Business.Models;
using ChapsDotNET.Models;

namespace ChapsDotNET.Common.Mappers
{
    public static class MappingExtension
    {
        public static SalutationViewModel ToViewModel(this SalutationModel model)
        {
            return new SalutationViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                SalutationId = model.SalutationId,
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
        public static PublicHolidayViewModel ToViewModel(this PublicHolidayModel model)
        {
            return new PublicHolidayViewModel
            {
                Description = model.Description,
                Date = model.Date,
                PublicHolidayId = model.PublicHolidayId,
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
        public static LeadSubjectViewModel ToViewModel(this LeadSubjectModel model)
        {
            return new LeadSubjectViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                LeadSubjectId = model.LeadSubjectId,
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

        public static CampaignViewModel ToViewModel(this CampaignModel model)
        {
            return new CampaignViewModel
            {
                Detail = model.Detail,
                Active = model.Active,
                CampaignId = model.CampaignId
            };
        }

        public static CampaignModel ToModel(this CampaignViewModel viewModel)
        {
            return new CampaignModel
            {
                Detail = viewModel.Detail,
                Active = viewModel.Active,
                CampaignId = viewModel.CampaignId
            };
        }
    }
}
