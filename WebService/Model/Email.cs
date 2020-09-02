namespace WebService.Model
{
    #region << Using >>

    using System;

    #endregion

    public class Email
    {
        #region Properties

        public DateTime CreateDate { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Recipient { get; set; }

        public Result Result { get; set; }

        public string FailedMessage { get; set; }

        #endregion

        #region Constructors

        public Email()
        {
            CreateDate = DateTime.Now;
        }

        #endregion
    }
}