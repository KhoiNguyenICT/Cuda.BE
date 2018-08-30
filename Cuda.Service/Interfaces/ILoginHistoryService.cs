using System.Threading.Tasks;
using Cuda.Model.Entities;
using Cuda.Service.Dtos.LoginHistory;

namespace Cuda.Service.Interfaces
{
    public interface ILoginHistoryService: IService<LoginHistory>
    {
        bool CheckTokenLoginNeareast(CheckTokenLoginNeareastDto checkTokenLoginNeareastDto);
    }
}