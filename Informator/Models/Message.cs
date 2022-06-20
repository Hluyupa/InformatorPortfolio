namespace Informator.Models;

public class Message {
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string? Text { get; set; }
    public byte[]? Attachments { get; set; }
    public virtual DataUser? Sender { get; set; }
}