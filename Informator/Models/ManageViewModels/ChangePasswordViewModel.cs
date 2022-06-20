using System.ComponentModel.DataAnnotations;

namespace Informator.Models.ManageViewModels;

public class ChangePasswordViewModel {
    [Required]
    [DataType(DataType.Password)]
    public string? OldPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare(nameof(NewPassword), ErrorMessage = "Password not equal")]
    public string? ConfirmPassword { get; set; }
}
