namespace WebService.Model
{
    public class EmailPostModel
    {
        #region Properties

        public string Subject { get; set; }

        public string Body { get; set; }

        public string[] Recipients { get; set; }

        #endregion
    }
}