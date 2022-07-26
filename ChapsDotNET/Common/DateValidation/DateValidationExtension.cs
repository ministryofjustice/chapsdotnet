using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ChapsDotNET.Common.DateValidation
{
    public class CustomDateAttribute : RangeAttribute
    {
      public CustomDateAttribute()
        : base(typeof(DateTime),
               DateTime.Now.AddDays(1).ToShortDateString(),
               DateTime.Now.AddYears(5).ToShortDateString()
        )
      { } 
    }
}
