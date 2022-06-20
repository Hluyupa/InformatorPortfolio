using Microsoft.AspNetCore.Identity;

namespace Informator.Models;

public class UserIdentity : IdentityUser<Guid> {
    public virtual DataUser? DataUser { get; set; }
}
