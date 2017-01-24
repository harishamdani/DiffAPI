using Comparator.Service;

namespace Comparator.Models.Entities
{
    /// <summary>
    /// An object which has an ID, and both Left and Right data to be compared.
    /// </summary>
    public class BinaryData
    {
        public int Id { get; set; }

        public SingleData Left { get; set; }

        public SingleData Right { get; set; }
    }
}