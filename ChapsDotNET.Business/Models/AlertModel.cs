using System;
using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Business.Models
{
	public class AlertModel
	{
		public int AlertID { get; set; }
		public bool Live { get; set; }
		public DateTime EventStart { get; set; }
		public int RaisedHours { get; set; }
		public DateTime WarnStart { get; set; }
		public string? Message { get; set; }
	}
}

