namespace Informator.Models;

public class DataUser {
    private string? _email;
    private string? _phone;

    public Guid Id { get; set; }
    public string? FirstName { get; set; }
    public string? SecondName { get; set; }
    public string? MiddleName { get; set; }
    public string? Email {
        get {
            if (UserIdentity is null) {
                return _email;
            }
            else {
                return UserIdentity.Email;
            }
        }
        set {
            if (UserIdentity is null) {
                _email = value;
            }
            else {
                UserIdentity.Email = value;
            }
        }
    }
    public string? PhoneNumber {
        get {
            if (UserIdentity is null) {
                return _phone;
            }
            else {
                return UserIdentity.PhoneNumber;
            }
        }
        set {
            if (UserIdentity is null) {
                _phone = value;
            }
            else {
                UserIdentity.PhoneNumber = value;
            }
        }
    }
    public string? PlaceOfWork { get; set; }
    public virtual UserIdentity? UserIdentity { get; set; }
    public virtual ICollection<Contact>? Contacts { get; set; }
    public virtual ICollection<Group>? Groups { get; set; }
}
