namespace WebService.Model
{
    #region << Using >>

    using Microsoft.EntityFrameworkCore;

    #endregion

    public class ApplicationContext : DbContext
    {
        #region Properties

        public DbSet<EmailPostModel> Emails { get; set; }

        #endregion

        #region Constructors

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        #endregion
    }
}