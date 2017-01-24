using System.Collections.Generic;
using Comparator.Models.Entities;

namespace Comparator.Service
{
    public interface ICompareService
    {
        IEnumerable<DiffDetail> Compare(string left, string right);
    }
}