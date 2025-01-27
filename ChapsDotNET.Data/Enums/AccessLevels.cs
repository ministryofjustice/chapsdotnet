using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChapsDotNET.Data.Enums
{
    
    public enum AccessLevel
    {
        Denied = -1,
        Deactivated = 0,
        ReadOnly = 26,
        User = 50,
        Manager = 75,
        Admin = 100
    }
}
