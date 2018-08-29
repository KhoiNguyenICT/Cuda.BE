using System;
using System.Net.Http;
using System.Threading.Tasks;
using Cuda.Api.Models;
using Cuda.Model.Entities;
using Cuda.Service.Dtos.LoginHistory;
using Cuda.Service.Dtos.Token;
using Cuda.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Cuda.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly ILoginHistoryService _historyService;

        public AccountController(IConfiguration configuration, ILoginHistoryService historyService)
        {
            _configuration = configuration;
            _historyService = historyService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                var client = new RestClient(_configuration["IdentityServerUrl"] + "/connect/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter(
                    "application/x-www-form-urlencoded", 
                    "grant_type=password&" +
                    "client_id=" + _configuration["IdentityServerConfiguration:ClientId"] + "&" +
                    "client_secret=" + _configuration["IdentityServerConfiguration:ClientSecret"] + "&" +
                    "username=" + loginViewModel.Username + "&" +
                    "password=" + loginViewModel.Password + "&" +
                    "scope=" + _configuration["IdentityServerConfiguration:Scope"], ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var token = JsonConvert.DeserializeObject<TokenDto>(response.Content);
                if (token.access_token != null)
                {
                    var loginHistory = new LoginHistory
                    {
                        Username = loginViewModel.Username,
                        Token = token.access_token
                    };
                    await _historyService.Add(loginHistory);
                    return Redirect(_configuration["WebManagerUrl"] + "/" + token.access_token);
                }

                return RedirectToAction("Login");
            }
            catch(Exception e)
            {
                return RedirectToAction("Login");
            }
        }

        [AllowAnonymous]
        public async Task<bool> CheckToken(CheckTokenLoginNeareastDto checkTokenLoginNeareastDto)
        {
            var result = await _historyService.CheckTokenLoginNeareast(checkTokenLoginNeareastDto);
            return result;
        }
    }
}