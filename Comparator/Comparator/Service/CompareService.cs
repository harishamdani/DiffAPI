using System.Collections.Generic;
using Comparator.Models.Entities;

namespace Comparator.Service
{
    /// <summary>
    /// This class provide function to compare strings 
    /// and present the differences between the strings.
    /// </summary>
    /// <seealso cref="Comparator.Service.ICompareService" />
    public class CompareService : ICompareService
    {
        /// <summary>
        /// Compares the specified left.
        /// </summary>
        /// <param name="left">The left string.</param>
        /// <param name="right">The right string.</param>
        /// <returns>
        /// If both strings have the same length and are actually different,
        /// It will return a list of differences contains the offset where
        /// the diff happened and for how many characters.
        /// </returns>
        /// <remarks>
        /// If the pre-condition to be compared is not fulfufilled,
        /// it will return an empty list as the default return.
        /// </remarks>
        public IEnumerable<DiffDetail> Compare(string left, string right)
        {
            if (string.IsNullOrWhiteSpace(left)
                || string.IsNullOrWhiteSpace(right)
                || left.Length != right.Length
                || string.Equals(left, right))
            {
                return new List<DiffDetail>();
            }

            var prevCharState = CharState.SIMILAR;
            var diffs = new List<DiffDetail>();
            var currentDiff = new DiffDetail();

            for (var i = 0; i < left.Length; i++)
            {
                // If char is different and the previous char is similar,
                // start new diffDetail. Mark the index and start the diff count.
                if(left[i] != right[i] && prevCharState == CharState.SIMILAR)
                {
                    currentDiff = new DiffDetail(i);
                }

                // If char is different and the previous char is different too,
                // increment the diffCount
                if (left[i] != right[i] && prevCharState == CharState.NOT_SIMILAR)
                {
                    currentDiff.Length = currentDiff.Length + 1;
                }

                if (left[i] == right[i] && prevCharState == CharState.NOT_SIMILAR)
                {
                    diffs.Add(currentDiff);
                }

                prevCharState = left[i] == right[i] ? CharState.SIMILAR : CharState.NOT_SIMILAR;
            }

            return diffs;
        }
    }
}