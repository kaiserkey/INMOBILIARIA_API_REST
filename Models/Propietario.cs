
namespace Inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Propietario
{
    [Display (Name = "Código")]
    public int IdPropietario{ get; set; }
    [Display (Name = "Nombre")]
    public string Nombre{ get; set; }
    [Display (Name = "Apellido")]
    public string Apellido{ get; set; }
    [Display (Name = "Dirección")]
    public string Direccion{ get; set; }
    [Display (Name = "Teléfono")]
    public string Telefono{ get; set; }
    [Display (Name = "DNI")]
    public string Dni{ get; set; }
    [Display (Name = "Email")]
    public string Email{ get; set; }

    [Required, DataType(DataType.Password)]
    public string Clave { get; set; }

    public string Avatar { get; set; }

    [NotMapped]
    public IFormFile AvatarFile { get; set; }

    [NotMapped]
    public string ClaveAntigua { get; set; }

    [NotMapped]
    public string NuevaClave { get; set; }

    [NotMapped]
    public string ConfirmarClave { get; set; }

    public override string ToString()
    {
        return $"Dni: {Dni}, Nombre: {Nombre}, Apellido: {Apellido}, Teléfono: {Telefono}, Email: {Email}";
    }
}