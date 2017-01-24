using System.Collections.Generic;
using Comparator.Models.ViewModels;

namespace Comparator.Models.Entities
{
    /// <summary>
    /// This class is a representation of diff result between a left and right data from a certain BinaryData ID.
    /// </summary>
    /// <seealso cref="Comparator.Models.ViewModels.BaseResponse" />
    public class CompareResponse : BaseResponse
    {
        public string DiffResultType { get; set; }

        public IEnumerable<DiffDetail> Diffs { get; set; }

        public CompareResponse()
        {
            Diffs = new List<DiffDetail>();
        }
    }
}