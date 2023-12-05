using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class ChecklistModel
    {
        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
    }
}
