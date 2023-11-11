
using Inmobiliaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json;

namespace Inmobiliaria.Api;

[ApiController]
[Route("api/[controller]")]
public class InmuebleController : ControllerBase
{
    private readonly DataContext _context;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment environment;

    public InmuebleController(DataContext context, IConfiguration configuration, IWebHostEnvironment environment)
    {
        _context = context;
        _configuration = configuration;
        this.environment = environment;

    }


    [HttpGet("getInmuebles")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerInmuebles()
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        return propietarioActual == null
                                    ? Unauthorized()
                                    : Ok(_context.Inmueble
                                               .Where(inmueble => inmueble.IdPropietario == propietarioActual.IdPropietario)
                                               .ToList());
    }

    [HttpGet("inmueblesConContratoVigente/{idPropietario}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerInmueblesConContratoVigente(int idPropietario)
    {
        var propietario = _context.Propietario.FirstOrDefault(p => p.IdPropietario == idPropietario);

        if (propietario == null)
        {
            return NotFound("Propietario no encontrado");
        }
        else
        {
            var inmueblesConContratoVigente = _context.Inmueble
                .Where(inmueble => inmueble.IdPropietario == idPropietario)
                .Where(inmueble => _context.Contrato.Any(contrato => contrato.IdInmueble == inmueble.IdInmueble && contrato.Activo))
                .ToList();

            return Ok(inmueblesConContratoVigente);
        }
    }


    [HttpGet("inmueblesAlquilados")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerInmueblesAlquilados()
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        if (propietarioActual == null)
            return Unauthorized();

        var propiedadesAlquiladas = _context.Contrato
            .Where(contrato => contrato.Inmueble.Propietario.IdPropietario == propietarioActual.IdPropietario && contrato.Activo == true)
            .Select(contrato => contrato.Inmueble)
            .ToList();

        return Ok(propiedadesAlquiladas);
    }



    // Dado un inmueble retorna el contrato activo de dicho inmueble
    [HttpPost("getContratoActivo")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerContratoActivo([FromBody] Inmueble inmueble)
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        if (propietarioActual == null)
        {
            return Unauthorized("No se encontró un propietario autenticado.");
        }

        var inmuebleEncontrado = _context.Inmueble.FirstOrDefault(i => i.IdInmueble == inmueble.IdInmueble && i.IdPropietario == propietarioActual.IdPropietario);
        if (inmuebleEncontrado == null)
        {
            return NotFound("No se encontró el inmueble especificado para el propietario actual.");
        }

        var contratoVigente = _context.Contrato
            .Include(c => c.Inquilino)
            .FirstOrDefault(contrato => contrato.IdInmueble == inmuebleEncontrado.IdInmueble && contrato.Activo);

        if (contratoVigente == null)
        {
            return NotFound("No se encontró un contrato vigente para el inmueble especificado.");
        }

        return Ok(contratoVigente);
    }


    //Dado un inmueble, retorna el inquilino del ultimo contrato activo de ese inmueble.
    [HttpPost("getUltimoInquilino")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerUltimoContratoInquilino([FromBody] Inmueble inmueble)
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        if (propietarioActual == null)
            return Unauthorized();

        var inmuebleEncontrado = _context.Inmueble
            .FirstOrDefault(i => i.IdInmueble == inmueble.IdInmueble && i.IdPropietario == propietarioActual.IdPropietario);

        if (inmuebleEncontrado == null)
            return NotFound();


        var contratoVigente = _context.Contrato
            .Include(c => c.Inquilino)
            .Where(contrato => contrato.IdInmueble == inmuebleEncontrado.IdInmueble && contrato.Activo)
            .OrderByDescending(contrato => contrato.FechaInicio)
            .FirstOrDefault();


        if (contratoVigente == null)
            return NotFound();


        return Ok(contratoVigente.Inquilino);
    }


    // Dado un Contrato, retorna los pagos de dicho contrato
    [HttpGet("getPagoContrato/{contratoId:int:min(1)}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ObtenerPagosContrato(int contratoId)
    {
        var propietarioActual = ObtenerPropietarioLogueado();
        if (propietarioActual == null)
            return Unauthorized();

        var contratoVer = _context.Contrato
            .Include(c => c.Inquilino)
            .Include(c => c.Inmueble)
                .ThenInclude(i => i.Propietario)
            .FirstOrDefault(c => c.IdContrato == contratoId);

        if (contratoVer == null)
            return BadRequest("No se proporcionó un contrato válido.");

        var pagosContrato = _context.Pago
            .Include(p => p.Contrato)
                .ThenInclude(c => c.Inquilino)
            .Include(p => p.Contrato)
                .ThenInclude(c => c.Inmueble)
                    .ThenInclude(i => i.Propietario)
            .Where(pago => pago.IdContrato == contratoVer.IdContrato)
            .Select(pago => new
            {
                pago.IdPago,
                pago.IdContrato,
                pago.Contrato,
                pago.NumeroPago,
                pago.Fecha,
                pago.Importe
            })
            .ToList();


        return Ok(pagosContrato);
    }



    // Actualizar Perfil
    [HttpPost("updateUsuario")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<Propietario> ActualizarUsuario([FromBody] Propietario propietario)
    {
        Console.WriteLine(propietario.ToString());
        if (propietario == null)
        {
            return BadRequest("No se proporcionó un propietario válido.");
        }

        var propietarioExistente = _context.Propietario.FirstOrDefault(p => p.IdPropietario == propietario.IdPropietario);
        var emailExistente = _context.Propietario.Any(p => p.IdPropietario != propietario.IdPropietario && p.Email == propietario.Email);

        if (emailExistente)
        {
            return BadRequest("El email ya está en uso por otro propietario.");
        }

        if (propietarioExistente != null)
        {
            // Actualizar los campos del propietario existente
            propietarioExistente.Dni = propietario.Dni;
            propietarioExistente.Nombre = propietario.Nombre;
            propietarioExistente.Apellido = propietario.Apellido;
            propietarioExistente.Telefono = propietario.Telefono;
            propietarioExistente.Email = propietario.Email;

            if (!string.IsNullOrEmpty(propietario.Clave))
            {
                // Hashear la contraseña
                string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                    password: propietario.Clave,
                    salt: System.Text.Encoding.ASCII.GetBytes(_configuration["Salt"]),
                    prf: KeyDerivationPrf.HMACSHA1,
                    iterationCount: 30000,
                    numBytesRequested: 256 / 8));

                propietarioExistente.Clave = hashedPassword;
            }
            if(propietario.Avatar != null){
                propietarioExistente.Avatar = propietario.Avatar;
            }
            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return Ok(propietarioExistente);
        }

        return NotFound("No se encontró el propietario especificado.");
    }



    //ActualizarInmueble
    [HttpPost("updateInmueble")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public IActionResult ActualizarInmueble([FromBody] Inmueble inmueble)
    {
        if (inmueble == null)
        {
            return BadRequest("No se proporcionó un inmueble válido.");
        }

        var inmuebleExistente = _context.Inmueble.Include(i => i.Propietario).FirstOrDefault(i => i.IdInmueble == inmueble.IdInmueble);
        if (inmuebleExistente != null)
        {
            // Actualizar los campos del inmueble existente con los valores proporcionados
            inmuebleExistente.IdPropietario = inmueble.IdPropietario;
            inmuebleExistente.Direccion = inmueble.Direccion;
            inmuebleExistente.Uso = inmueble.Uso;
            inmuebleExistente.Tipo = inmueble.Tipo;
            inmuebleExistente.Ambientes = inmueble.Ambientes;
            inmuebleExistente.Coordenadas = inmueble.Coordenadas;
            inmuebleExistente.Precio = inmueble.Precio;
            inmuebleExistente.Activo = inmueble.Activo;

            // Guardar los cambios en la base de datos
            _context.SaveChanges();

            return Ok(inmuebleExistente);
        }

        return NotFound("No se encontró el inmueble especificado.");
    }

    private Propietario ObtenerPropietarioLogueado()
    {
        var email = User.Identity.Name;
        var propietario = _context.Propietario.FirstOrDefault(p => p.Email == email);
        return propietario;
    }

    [HttpGet("imagen/{id:int:min(1)}")]
    public async Task<IActionResult> ObtenerImagenInmueble(int id)
    {
        var inmueble = await _context.Inmueble.FirstOrDefaultAsync(x => x.IdInmueble == id);
        if (inmueble == null)
        {
            return NotFound();
        }

        var wwwPath = environment.WebRootPath;
        var ruta = Path.Combine(wwwPath, inmueble.Foto.TrimStart('/').Replace('/', '\\'));

        if (ruta.StartsWith(Path.Combine(wwwPath, "Uploads")))
        {
            if (System.IO.File.Exists(ruta))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(ruta);
                return File(fileBytes, "image/png");
            }
        }
        else
        {
            // Ruta por defecto para la imagen
            var defaultImagePath = Path.Combine(wwwPath, "imagenes", "avatar_por_defecto_propiedad.jpg");
            if (System.IO.File.Exists(defaultImagePath))
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(defaultImagePath);
                return File(fileBytes, "image/jpg");
            }
        }

        return NotFound();
    }

    [HttpPost("nuevoInmuebleConImagen")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> NuevoInmuebleConImagen([FromForm] Inmueble inmueble, [FromForm(Name = "file")] IFormFile file)
    {
        try
        {
            Console.WriteLine(inmueble.ToString());

            // Guardar el inmueble en la base de datos
            _context.Inmueble.Add(inmueble);
            await _context.SaveChangesAsync();

            // Guardar la imagen en el servidor
            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(environment.WebRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = "foto_" + inmueble.IdInmueble + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);
                inmueble.Foto = "/Uploads/" + fileName;

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Actualizar la propiedad 'Foto' en la base de datos con la nueva ruta
                inmueble = await ActualizarFotoInmuebleEnBaseDeDatos(inmueble);
            }
            else
            {
                // Si no se recibe ninguna imagen, cargar una imagen por defecto
                string wwwPath = environment.WebRootPath;
                var defaultImagePath = Path.Combine(wwwPath, "imagenes", "avatar_por_defecto_propiedad.jpg");
                if (System.IO.File.Exists(defaultImagePath))
                {
                    string uploadsFolder = Path.Combine(environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    string fileName = "foto_defecto_" + inmueble.IdInmueble + ".jpg";
                    string filePath = Path.Combine(uploadsFolder, fileName);
                    inmueble.Foto = "/Uploads/" + fileName;

                    // Copiar la imagen por defecto al directorio de uploads
                    System.IO.File.Copy(defaultImagePath, filePath);

                    // Actualizar la propiedad 'Foto' en la base de datos con la nueva ruta
                    inmueble = await ActualizarFotoInmuebleEnBaseDeDatos(inmueble);
                }
            }

            return Ok(inmueble);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error en NuevoInmuebleConImagen: {ex.Message}");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    private async Task<Inmueble> ActualizarFotoInmuebleEnBaseDeDatos(Inmueble inmueble)
    {
        // Actualizar la propiedad 'Foto' en la base de datos
        _context.Entry(inmueble).Property(x => x.Foto).IsModified = true;
        await _context.SaveChangesAsync();
        return inmueble;
    }

}