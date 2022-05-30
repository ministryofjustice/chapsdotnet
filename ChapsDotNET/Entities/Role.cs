using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChapsDotNET.Entities;

public class Role
{
    [Required, Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int strength { get; set; }
    [Required, MaxLength(20)]
    public string Detail { get; set; } = string.Empty;

}