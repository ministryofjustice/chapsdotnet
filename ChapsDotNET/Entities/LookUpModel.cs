using System.ComponentModel.DataAnnotations;

namespace ChapsDotNET.Entities;

public abstract class LookUpModel
{
    [Display(Name = "Active")]
    public bool active { get; set; }
    public DateTime? deactivated { get; set; }
    [MaxLength(50)]
    public string deactivatedBy { get; set; }
}