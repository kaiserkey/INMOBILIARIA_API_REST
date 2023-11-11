
namespace Inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//TODO: agregarle telefono y dni al usuario
public enum UsuarioRol
{
    Administrador = 1,
    Empleado = 2,
}
public class Usuario
{
    [Key]
    [Display(Name = "Código")]
    public int IdUsuario { get; set; }
    [Required]
    public string Nombre { get; set; }
    [Required]
    public string Apellido { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, DataType(DataType.Password)]
    public string Clave { get; set; }

    public string ClaveAntigua { get; set; }
    public string NuevaClave { get; set; }
    public string ConfirmarClave { get; set; }

    public string Dni { get; set; }
    public string Telefono { get; set; }

    public string Avatar { get; set; }
    [NotMapped]//Para EF
    public IFormFile AvatarFile { get; set; }

    public int Rol { get; set; }
    [NotMapped]//Para EF
    public string RolNombre => Rol > 0 ? ((UsuarioRol)Rol).ToString() : "";

    public static IDictionary<int, string> ObtenerRoles()
    {
        SortedDictionary<int, string> roles = new SortedDictionary<int, string>();
        Type tipoEnumRol = typeof(UsuarioRol);
        foreach (var valor in Enum.GetValues(tipoEnumRol))
        {
            roles.Add((int)valor, Enum.GetName(tipoEnumRol, valor));
        }
        return roles;
    }
}