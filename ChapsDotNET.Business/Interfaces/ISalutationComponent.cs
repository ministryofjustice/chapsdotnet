﻿using ChapsDotNET.Business.Models;

namespace ChapsDotNET.Business.Interfaces
{
    public interface ISalutationComponent
    {
        Task<List<SalutationModel>> GetSalutationsAsync(bool showActiveAndInactive = false);
    }
}