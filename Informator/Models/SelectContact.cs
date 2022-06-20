namespace Informator.Models
{
    public class SelectContact
    {
        public string? TextMessage { get; set; }
        public List<DataUser>? Contacts { get; set; }
        public List<Group>? Groups { get; set; }
    }
}
