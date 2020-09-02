namespace WebService.Controllers
{
    #region << Using >>

    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using MailKit.Net.Smtp;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    using WebService.Model;

    #endregion

    [Route("api/[controller]")]
    [ApiController]
    public class MailsController : ControllerBase
    {
        #region Properties

        private IConfiguration Configuration { get; }

        private readonly ApplicationContext db;

        #endregion

        #region Constructors

        public MailsController(ApplicationContext context, IConfiguration configuration)
        {
            this.db = context;
            Configuration = configuration;
        }

        #endregion

        private async Task<SendEmail> SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();
            var SMTP = Configuration.GetSection("SMTP");

            emailMessage.From.Add(new MailboxAddress(SMTP.GetValue<string>("Name"), SMTP.GetValue<string>("UserName")));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                                {
                                        Text = message
                                };

            var sendEmail = new SendEmail();

            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(SMTP.GetValue<string>("Host"), SMTP.GetValue<int>("Port"), SMTP.GetValue<bool>("UseSsl"));
                    await client.AuthenticateAsync(SMTP.GetValue<string>("UserName"), SMTP.GetValue<string>("Password"));
                    await client.SendAsync(emailMessage);

                    await client.DisconnectAsync(true);
                    sendEmail.Result = Result.Ok;
                }
                catch (Exception e)
                {
                    sendEmail.FailedMessage = e.Message;
                    sendEmail.Result = Result.Failed;
                }
            }

            return sendEmail;
        }

        /// <summary>
        /// Sends the message to all recipients and saves result to the database.
        /// </summary>
        /// <param name="emailPost">Subject: Subject of email, Body: Body of email, Recipients: Email addresses.</param>
        /// <returns>Result of operation</returns>
        [HttpPost]
        public async Task<IActionResult> Post( EmailPostModel emailPost)
        {
            foreach (var send in emailPost.Recipients)
            {
                var sendEmail = await SendEmailAsync(send, emailPost.Subject, emailPost.Body);
                var email = new Email()
                            {
                                    Result = sendEmail.Result,
                                    FailedMessage = sendEmail.FailedMessage,
                                    Body = emailPost.Body,
                                    Subject = emailPost.Subject,
                                    Recipient = send
                            };

                await this.db.Emails.AddAsync(email);
            }

            await this.db.SaveChangesAsync();

            return Ok();
        }

        /// <summary>
        /// Returns all saved emails.
        /// </summary>
        /// <returns>List of emails</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = await this.db.Emails.ToListAsync();

            var emailVM = email.Select(a => new EmailVM()
                                            {
                                                    Result = a.Result.ToString(),
                                                    Subject = a.Subject,
                                                    Body = a.Body,
                                                    FailedMessage = a.FailedMessage,
                                                    Recipient = a.Recipient,
                                                    Id = a.Id,
                                                    CreateDate = a.CreateDate
                                            }).ToList();

            return Ok(emailVM);
        }
    }
}