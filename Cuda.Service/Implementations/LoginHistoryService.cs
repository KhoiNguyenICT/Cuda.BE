using System;
using System.Linq;
using System.Threading.Tasks;
using Cuda.Model;
using Cuda.Model.Entities;
using Cuda.Service.Dtos.LoginHistory;
using Cuda.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cuda.Service.Implementations
{
    public class LoginHistoryService: BaseService<LoginHistory>, ILoginHistoryService
    {
        public LoginHistoryService(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> CheckTokenLoginNeareast(CheckTokenLoginNeareastDto checkTokenLoginNeareastDto)
        {
            var now = DateTime.Now;
            var query = await this.Queryable().OrderByDescending(x => x.CreatedDate)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Username == checkTokenLoginNeareastDto.Username);
            if (query != null && query.Token == checkTokenLoginNeareastDto.Token)
            {
                var timeDifference = (now - query.CreatedDate).Seconds;
                if (timeDifference <= 3600)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}