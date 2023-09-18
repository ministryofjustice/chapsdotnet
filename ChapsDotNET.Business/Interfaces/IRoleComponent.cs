using System;
using ChapsDotNET.Business.Models;
using ChapsDotNET.Business.Models.Common;

namespace ChapsDotNET.Business.Interfaces
{
	public interface IRoleComponent
	{
        Task<PagedResult<List<RoleModel>>> GetRolesAsync(RoleRequestModel request);

    }
}

