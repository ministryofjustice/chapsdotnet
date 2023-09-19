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
        public string DisplayMessage
        {
            get
            {
                if (Message == null)
                {
                    throw new InvalidOperationException("Message cannot be null.");
                }
                else
                {
                    string result = Message!;
                    if (result != null)
                    {
                        result = result.Replace("@ESDT", this.EventStart.ToString("d MMM yy \\a\\t H:mm"));
                        result = result.Replace("@ESD", this.EventStart.ToString("d MMM yy"));
                        result = result.Replace("@EST", this.EventStart.ToString("H:mm"));
                    }
                    return result!;
                }
            }
        }

        public AlertStatus Status
        {
            get
            {
                if (Live)
                {
                    if (DateTime.Now >= EventStart) return AlertStatus.Overdue;
                    if (DateTime.Now < WarnStart) return AlertStatus.Off;
                    if (DateTime.Now > EventStart.AddHours(-RaisedHours)) return AlertStatus.High;
                    return AlertStatus.Warning;
                }
                else
                {
                    return AlertStatus.Off;
                }
            }
        }

        public string DisplayClass
        {
            get
            {
                return Status switch
                {
                    AlertStatus.Warning => "comment",
                    AlertStatus.High => "warning",
                    AlertStatus.Overdue => "error",
                    AlertStatus.Off => "Inactive",
                    _ => "Inactive"
                };

            }
        }


    }

    public enum AlertStatus
    {
        Off,
        Warning,
        High,
        Overdue
    }
}