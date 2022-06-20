namespace Informator.Models
{
    public class GroupCrud
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<DataUser>? Members{ get; set; }
    }
}
