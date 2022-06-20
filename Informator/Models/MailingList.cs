namespace Informator.Models
{
    public class MailingList
    {
        public string? MailingListName { get; set; }
        public ICollection<Contact>? Contacts { get; set; }
    }
}
