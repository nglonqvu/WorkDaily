using Microsoft.EntityFrameworkCore;
using Project_PRN231_API.Interface.Auth;
using Project_PRN231_API.Interface.Common;
using Project_PRN231_API.Models;
using System.Security.Cryptography;
using MimeKit;

namespace Project_PRN231_API.Services.Common
{
    public class CommonService : ICommonService
    {
        private readonly Project_PRN231_SE1710Context context = new Project_PRN231_SE1710Context();
        private readonly IAuthService service;
        public User? user = new User();
        public List<Taskk> tasks = new List<Taskk>();
        private static readonly char[] chars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_+-=[]{}|;:'\",.<>?/`~".ToCharArray();
        public CommonService(Project_PRN231_SE1710Context context, IAuthService service)
        {
            this.context = context;
            this.service = service;
        }

        public async Task<string?> Login(string email, string password)
        {
            user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return null;
            }
            else
            {
                var token = service.GetToken(user);
                return token;
            }
        }

        public async Task<List<Taskk>> HomePage(int UserId)
        {
            return await context.Taskks.Include(t => t.Category).Where(t => t.UserId == UserId).ToListAsync();
        }

        public async Task<bool> Register(string email, string password, string usename)
        {
            user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                string token = GenerateSecureRandomToken(10);
                var expiresAt = DateTime.Now.AddMinutes(2);
                UserTemp userTemp = new UserTemp()
                {
                    Username = usename,
                    Email = email,
                    Password = BCrypt.Net.BCrypt.HashPassword(password),
                    Token = token,
                    ExpiresAt = expiresAt
                };
                await context.UserTemps.AddAsync(userTemp);
                await context.SaveChangesAsync();

                await SendVerificationEmail(email, token);
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task SendVerificationEmail(string userEmail, string token)
        {
            try
            {
                using (var emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress("dailyworkfpt@gmail.com", "@Vu131102");
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress mailTo = new MailboxAddress("Receiver", userEmail);
                    emailMessage.To.Add(mailTo);
                    emailMessage.Subject = "Xác nhận tạo tài khoản";
                    BodyBuilder bodyBuilder = new BodyBuilder();
                    bodyBuilder.HtmlBody = $"Mã xác nhận email của bạn là: {token}";
                    emailMessage.Body = bodyBuilder.ToMessageBody();
                    using (var mailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        await mailClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                        await mailClient.AuthenticateAsync("dailyworkfpt@gmail.com", "@Vu131102");
                        await mailClient.SendAsync(emailMessage);
                        await mailClient.DisconnectAsync(true);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public static string GenerateSecureRandomToken(int length)
        {
            byte[] data = new byte[length];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }

            var token = new char[length];

            for (int i = 0; i < length; i++)
            {
                token[i] = chars[data[i] % chars.Length];
            }

            return new string(token);
        }
    }
}
