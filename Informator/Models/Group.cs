namespace Informator.Models;

public class Group {

    

    public Guid Id { get; set; }
    public string? Name { get; set; }
    public virtual ICollection<DataUser>? Members { get; set; }
}
