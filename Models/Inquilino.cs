
namespace Inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;

public class Inquilino
{
    [Display (Name = "Código")]
    public int IdInquilino{ get; set; }
    [Display (Name = "Nombre")]
    public string Nombre{ get; set; }
    [Display (Name = "Apellido")]
    public string Apellido{ get; set; }
    [Display (Name = "Email")]
    public string Email{ get; set; }
    [Display (Name = "DNI")]
    public string Dni{ get; set; }
    [Display (Name = "Teléfono")]
    public string Telefono{ get; set; }
    [Display (Name = "Fecha de Nacimiento")]
    public DateTime FechaNacimiento{ get; set; }

    public override string ToString()
    {
        return $"Nombre: {Nombre}, Apellido: {Apellido}, Dni: {Dni}, Telefono: {Telefono}, Email: {Email}";
    }
}