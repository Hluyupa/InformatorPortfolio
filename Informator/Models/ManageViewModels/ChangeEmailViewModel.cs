using System.ComponentModel.DataAnnotations;

namespace Informator.Models.ManageViewModels;

public class ChangeEmailViewModel {
    [Required]
    [DataType(DataType.EmailAddress)]
    public string? OldEmail { get; set; }

    [Required]
    [DataType(DataType.EmailAddress)]
    public string? NewEmail { get; set; }

    [Required]
    public string? ConfirmCode { get; set; }
}
