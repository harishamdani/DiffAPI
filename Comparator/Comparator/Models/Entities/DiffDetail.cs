namespace Comparator.Models.Entities
{
    /// <summary>
    /// This class represent the diff detail where it store the start index of the diff
    /// and the length of characters of the specific diff.
    /// </summary>
    public class DiffDetail
    {
        public DiffDetail()
        {
        }

        public DiffDetail(int offset)
        {
            Offset = offset;
            Length = 1;
        }

        public int Offset { get; set; }

        public int Length { get; set; }
    }
}