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

        public static MoJMinisterViewModel ToViewModel(this MoJMinisterModel model)
        {
            return new MoJMinisterViewModel
            {
                Name = model.Name,
                Prefix = model.Prefix,
                Suffix = model.Suffix,
                Rank = model.Rank,
                MoJMinisterId = model.MoJMinisterId,
            };
        }

        public static MoJMinisterModel ToModel(this MoJMinisterViewModel viewModel)
        {
            return new MoJMinisterModel
            {
                Name = viewModel.Name,
                Prefix = viewModel.Prefix,
                Suffix = viewModel.Suffix,
                Rank = viewModel.Rank,
                MoJMinisterId = viewModel.MoJMinisterId,
            };
        }
    }
}
