using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cuda.Service.Dtos.LoginHistory;
using Cuda.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cuda.Api.Controllers
{
    public class AccountController : BaseController
    {
        private readonly ILoginHistoryService _historyService;

        public AccountController(ILoginHistoryService historyService)
        {
            _historyService = historyService;
        }

        [AllowAnonymous]
        [HttpPost("checkToken")]
        public async Task<bool> CheckToken(CheckTokenLoginNeareastDto checkTokenLoginNeareastDto)
        {
            var result = await _historyService.CheckTokenLoginNeareast(checkTokenLoginNeareastDto);
            return result;
        }
    }
}