
namespace Inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Inmueble
{
    [Display( Name = "Código" )]
    public int IdInmueble{ get; set; }
    [Display( Name = "Tipo" )]
    public string Tipo{ get; set; }
    [Display(Name = "Direccion")]
    public string Direccion { get; set; }

    [Display( Name = "Coordenadas" )]
    public string Coordenadas{ get; set; }
    [Display( Name = "Precio" )]
    public decimal Precio{ get; set; }
    [Display( Name = "Ambientes" )]
    public int Ambientes{ get; set; }
    [Display( Name = "Uso" )]
    public string Uso{ get; set; }
    [Display( Name = "Activo" )]
    public Boolean Activo{ get; set; }

    [Display( Name = "Propietario" ) ]
    public int IdPropietario{ get; set; }

    [ForeignKey( nameof( IdPropietario ) ) ]

    public Propietario Propietario{ get; set; }

    public string Foto { get; set; }
    [NotMapped]
    public IFormFile? FotoFile { get; set; }

    public override string ToString()
    {
        return
               $"{IdPropietario} |  " +
               $" {Coordenadas} | " +
               $" {Uso} | " +
               $" {Tipo} | " +
               $" ambientes: {Ambientes} | " +
               $" {Coordenadas} | " +
               $" $ {Precio} | " +
               $" Activo: {Activo}"+
               $" Foto: {Foto}"
               ;
    }

}