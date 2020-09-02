namespace WebService.Model
{
    using System;

    public class EmailVM
    {
        #region Properties

        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Recipient { get; set; }

        public string Result { get; set; }

        public string FailedMessage { get; set; }

        #endregion
    }
}