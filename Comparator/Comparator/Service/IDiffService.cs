using System.Threading.Tasks;
using Comparator.Models.Entities;
using Comparator.Models.ViewModels;

namespace Comparator.Service
{
    public interface IDiffService
    {
        Task<BaseResponse> Put(DataRequest request);

        Task<CompareResponse> GetComparison(int id);
    }
}