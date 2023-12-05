using System.ComponentModel.DataAnnotations;

namespace Web_API.Models
{
    public class ChecklistItemModel
    {

        [Key] public int Id { get; set; }
        [Required] public int ChecklistId { get; set; }
        [Required] public string Name { get; set; }
        [Required] public int status { get; set; }
    }
}
