
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class PropietarioController : Controller
    {
        private MySqlDatabase con { get; set; }
        private readonly RepositorioPropietario RepoPropietario;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        public PropietarioController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            con = new MySqlDatabase();
            RepoPropietario = new RepositorioPropietario();
            this.configuration = configuration;
            this.environment = environment;
        }

        // GET: Propietario
        [Authorize]
        public ActionResult Index()
        {
            var listaPropietarios = RepoPropietario.GetPropietarios(con);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje")){
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(listaPropietarios);
        }

        // GET: Propietario/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var propietario = RepoPropietario.GetPropietario(con, id);
            return View(propietario);
        }

        // GET: Propietario/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Propietario propietario)
        {
            try
            {
                var existEmail = RepoPropietario.GetPropietarioPorEmail(con, propietario.Email);

                if(existEmail != null){
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    ViewBag.Error = "El email ya se encuentra registrado.";
                    return View();
                }

                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: propietario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8
                    ));

                propietario.Clave = hashed;

                RepoPropietario.CreatePropietario(con, propietario);

                if (propietario.IdPropietario > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (propietario.AvatarFile != null && propietario.AvatarFile.Length > 0)
                    {
                        string fileName = "avatar_" + propietario.IdPropietario + Path.GetExtension(propietario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        propietario.Avatar = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            propietario.AvatarFile.CopyTo(stream);
                        }
                    }
                    else
                    {
                        propietario.Avatar = Path.Combine("/img", "default.webp");
                    }

                    RepoPropietario.UpdatePropietario(con, propietario);
                }

                TempData["Id"] = propietario.IdPropietario;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Propietario propietario = RepoPropietario.GetPropietario(con, id);
            return View(propietario);
        }

        // POST: Propietario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Propietario UpdatePropietario)
        {
            var propietario = RepoPropietario.GetPropietario(con, id);
            try
            {
                if (UpdatePropietario.Clave == null || UpdatePropietario.Clave == "")
                {
                    UpdatePropietario.Clave = propietario.Clave;
                }
                else
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: UpdatePropietario.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8
                        ));
                    UpdatePropietario.Clave = hashed;
                }

                if (UpdatePropietario.AvatarFile != null)
                {
                    //borramos la imagen anterior
                    if (propietario.Avatar != null || propietario.Avatar != "")
                    {
                        string wwwPath_delete = environment.WebRootPath;
                        string filePath_delete = wwwPath_delete + propietario.Avatar;

                        if (System.IO.File.Exists(filePath_delete) && Path.GetFileName(filePath_delete) != "default.webp")
                        {
                            System.IO.File.Delete(filePath_delete);
                            Console.WriteLine("Archivo eliminado exitosamente");
                        }
                    }

                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string fileName = "avatar_" + UpdatePropietario.IdPropietario + Path.GetExtension(UpdatePropietario.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    UpdatePropietario.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        UpdatePropietario.AvatarFile.CopyTo(stream);
                    }
                }
                else
                {
                    UpdatePropietario.Avatar = propietario.Avatar;
                }

                var res = RepoPropietario.UpdatePropietario(con, UpdatePropietario);
                TempData["Mensaje"] = "La entidad se actualizo correctamente ID:" + id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Propietario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Propietario propietario = RepoPropietario.GetPropietario(con, id);
            
            return View(propietario);
        }

        // POST: Propietario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                var res = RepoPropietario.DeletePropietario(con, id);
                TempData["Mensaje"] = "La entidad se ha eliminado corectamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}