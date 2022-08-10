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
    }
}
