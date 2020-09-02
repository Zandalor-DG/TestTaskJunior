namespace WebService.Controllers
{
    #region << Using >>

    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public Task<IActionResult> Post(EmailPostModel email)
        {
            return Ok();
        }
    }
}