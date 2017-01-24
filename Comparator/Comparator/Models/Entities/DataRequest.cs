using System.ComponentModel.DataAnnotations;

namespace Comparator.Models.Entities
{
    /// <summary>
    /// This Request object is used as parameter to store binary data to certain ID.
    /// </summary>
    /// <seealso cref="Comparator.Models.Entities.SingleData" />
    public class DataRequest : SingleData
    {
        [Required]
        public int BinaryDataId { get; set; }

    }
}