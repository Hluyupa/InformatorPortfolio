using System.ComponentModel.DataAnnotations;

namespace Informator.Models.AccountViewModels;

public sealed class RegisterViewModel {
    [Required(ErrorMessage = "First name is required")]
    public string? FirstName { get; set; }

    [Required(ErrorMessage = "Second name is required")]
    public string? SecondName { get; set; }

    [Required(ErrorMessage = "Username is required")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Compare(nameof(Password), ErrorMessage = "Password not equal")]
    [DataType(DataType.Password)]
    public string? ConfirmPassword { get; set; }
}