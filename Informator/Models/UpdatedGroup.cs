namespace Informator.Models
{
    public class UpdatedGroup
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public List<DataUser>? UsersOutGroup{ get; set; }
        public List<DataUser>? UsersIntoGroup { get; set; }
    }
}
