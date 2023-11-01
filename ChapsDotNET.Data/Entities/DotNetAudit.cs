using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Data.Entities
{
    public class DotNetAudit
	{
        [Key]
        public int AuditId { get; set; }  
        public DateTime Date { get; set; }  
        public int UserId { get; set; } 
        public string? Object { get; set; }
        public int? ObjectPrimaryKey { get; set; }
        public int? RootPrimaryKey { get; set; }
        public int ActionId { get; set; }  
    }
}