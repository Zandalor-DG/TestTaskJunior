namespace WebService.Controllers
{
    #region << Using >>

    using System.Threading.Tasks;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Mvc;
    using MimeKit;
    using WebService.Model;

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        #region Properties

        private ApplicationContext db;

        #endregion

        #region Constructors

        public MailsController(ApplicationContext context)
        {
            this.db = context;
        }

        #endregion

        public async Task<IActionResult> SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "Chamar007@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                                {
                                        Text = message
                                };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.com", 465, true);
                await client.AuthenticateAsync("Chamar007@yandex.ru", "@123654789@");
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }

            return Ok();
        }
    }
}