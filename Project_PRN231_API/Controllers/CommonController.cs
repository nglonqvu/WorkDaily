using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_PRN231_API.Interface.Common;
using Project_PRN231_API.Models;

namespace Project_PRN231_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService service;

        public CommonController(ICommonService service)
        {
            this.service = service;
        }

        [HttpGet("Login")]
        public async Task<string?> Login(string email, string password)
        {
            return await service.Login(email, password);
        }

        [Authorize]
        [HttpGet("HomePage")]
        public async Task<List<Taskk>?> HomePage(int userId)
        {
            return await service.HomePage(userId);
        }

        [HttpGet("Register")]
        public async Task<bool> Register(string email, string password, string usename)
        {
            return await service.Register(email, password, usename);
        }
    }
}
