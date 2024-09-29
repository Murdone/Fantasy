using Fantasy.shared.Entities;
using Fantasy.shared.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fantasy.shared.DTOs;

public class UserDTO
{
    [DataType(DataType.Password)]
    [Display(Name = "Password", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = "LengthField", ErrorMessageResourceType = typeof(Literals))]
    public string Password { get; set; } = null!;

    [Compare("Password", ErrorMessageResourceName = "PasswordAndConfirmationDifferent", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "PasswordConfirm", ResourceType = typeof(Literals))]
    [DataType(DataType.Password)]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [StringLength(20, MinimumLength = 6, ErrorMessageResourceName = "LengthField", ErrorMessageResourceType = typeof(Literals))]
    public string PasswordConfirm { get; set; } = null!;

    // Propiedades adicionales para el DTO (necesarias para el mapeo)
    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int CountryId { get; set; }

    // Método para convertir UserDTO a User
    public User ToUser()
    {
        return new User
        {
            Email = this.Email,
            FirstName = this.FirstName,
            LastName = this.LastName,
            CountryId = this.CountryId
            // Mapea cualquier otra propiedad que necesites
        };
    }

    public string Language { get; set; } = null!;
}