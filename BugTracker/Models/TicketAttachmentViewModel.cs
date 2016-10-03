namespace BugTracker.Models
{
    public class TicketAttachmentViewModel
    {
        private const string TITLE_PREFIX = "File URL: ";

        public int TicketId { get; set; }

        public string Title
        {
            get
            {
                return TITLE_PREFIX + MediaSource;
            }
        }

        public string MediaSource { get; set; }
    }
}