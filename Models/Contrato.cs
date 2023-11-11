namespace Inmobiliaria.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Contrato
{
    [Display( Name = "Código" )]
    public int IdContrato { get; set; }

    [Display( Name = "Código De Inmueble" ) ]
    public int IdInmueble { get; set; }

    [ForeignKey( nameof( IdInmueble ) ) ]
    public Inmueble Inmueble { get; set; }

    [Display( Name = "Inquilino" ) ]
    public int IdInquilino{ get; set; }

    [ForeignKey( nameof( IdInquilino ) ) ]
    public Inquilino Inquilino{ get; set; }

    [Display( Name = "Fecha de Inicio" )]
    public DateTime FechaInicio { get; set; }

    [Display(Name = "Fecha de Fin")]
    public DateTime FechaFin { get; set; }

    [Display(Name = "Monto Mensual del Alquiler")]
    public decimal MontoAlquilerMensual { get; set; }

    public Boolean Activo { get; set; }

    public override string ToString()
    {
        return $"inicio: {FechaInicio} | fin: {FechaFin}| Activo: {(Activo ? "si" : "no")}";
    }

}