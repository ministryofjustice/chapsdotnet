using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Tracing;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChapsDotNET.Models
{
	public class AlertViewModel
	{
		[Key]
		public int AlertID { get; set; }
		[Required]
		public bool Live { get; set; }
		[Required]
		public DateTime EventStart { get; set; }

		public int RaisedHours { get; set; }
		[Required]
		public DateTime WarnStart { get; set; }
		[Required, MaxLength(200)]
		public string? Message { get; set; }

		[Required]
		public string? WarnStartString { get; set; }
		[Required]
        public string? EventStartString { get; set; }
    }
}

