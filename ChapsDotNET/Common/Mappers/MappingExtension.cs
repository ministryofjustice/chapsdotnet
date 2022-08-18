﻿using ChapsDotNET.Business.Models;
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
    }
}
