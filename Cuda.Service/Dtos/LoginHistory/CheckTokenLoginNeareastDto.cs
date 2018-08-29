using System;

namespace Cuda.Service.Dtos.LoginHistory
{
    public class CheckTokenLoginNeareastDto: BaseDto
    {
        public string Username { get; set; }
        public string Token { get; set; }
    }
}