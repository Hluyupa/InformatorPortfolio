namespace Informator.Models;

public class Contact {
    public Guid Id { get; set; }
    public string? Data { get; set; }
    public virtual SystemType? SystemType { get; set; }
    public virtual DataUser? User { get; set; }
}
