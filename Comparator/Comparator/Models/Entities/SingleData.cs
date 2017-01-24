using System.ComponentModel.DataAnnotations;

namespace Comparator.Models.Entities
{
    /// <summary>
    /// This is common object where it has binary data information and its type (LEFT or RIGHT).
    /// </summary>
    public class SingleData
    {
        [Required(AllowEmptyStrings = false)]
        public string Data { get; set; }

        [Required]
        public string DataType { get; set; }


    }
}