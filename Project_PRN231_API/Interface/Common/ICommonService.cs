using Microsoft.AspNetCore.Mvc;
using Project_PRN231_API.Models;

namespace Project_PRN231_API.Interface.Common
{
    public interface ICommonService
    {
        Task<string?> Login(string email, string password);
        Task<List<Taskk>> HomePage(int UserId);
        Task<bool> Register(string email, string password, string usename);
    }
}
