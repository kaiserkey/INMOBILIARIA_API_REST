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
    public class UsuarioController : Controller
    {
        private MySqlDatabase con { get; set; }
        private readonly RepositorioUsuario RepoUsuario;
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;

        public UsuarioController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            con = new MySqlDatabase();
            RepoUsuario = new RepositorioUsuario();
            this.configuration = configuration;
            this.environment = environment;
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // GET: Usuario/Salir
        [Route("Salir", Name = "Logout")]
        public ActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // POST: Usuario/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var Usuario = RepoUsuario.ObtenerPorEmail(con, login.Usuario);
                    Console.WriteLine("Usuario: " + Usuario.Email + " - " + Usuario.Clave);
                    Console.WriteLine("Hash: " + hashed);
                    if (Usuario == null || Usuario.Clave != hashed)
                    {
                        
                        ViewBag.Error = "Usuario o contraseña incorrecto.";
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, Usuario.Email),
                        new Claim("FullName", Usuario.Nombre + " " + Usuario.Apellido),
                        new Claim(ClaimTypes.Role, Usuario.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity));


                    return Redirect("/Home");
                }

                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Usuario
        [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = RepoUsuario.GetUsuarios(con);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje")){
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(usuarios);
        }

        // GET: Usuario/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            Usuario usuario = RepoUsuario.GetUsuario(con, id);
            return View(usuario);
        }

        // GET: Usuario/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario usuario)
        {
            try
            {

                var existUser = RepoUsuario.ObtenerPorEmail(con, usuario.Email);
                if(existUser != null){
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    ViewBag.Error = "El email ya se encuentra registrado.";
                    return View();
                }
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usuario.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8
                    ));
                usuario.Clave = hashed;

                var res = RepoUsuario.CreateUsuario(con, usuario);

                if (usuario.IdUsuario > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (usuario.AvatarFile != null && usuario.AvatarFile.Length > 0)
                    {
                        string fileName = "avatar_" + usuario.IdUsuario + Path.GetExtension(usuario.AvatarFile.FileName);
                        string pathCompleto = Path.Combine(path, fileName);
                        usuario.Avatar = Path.Combine("/Uploads", fileName);
                        using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                        {
                            usuario.AvatarFile.CopyTo(stream);
                        }
                    }
                    else
                    {
                        usuario.Avatar = Path.Combine("/img", "default.webp");
                    }

                    RepoUsuario.UpdateUsuario(con, usuario);
                }
                ViewBag.Roles = Usuario.ObtenerRoles();
                TempData["Id"] = usuario.IdUsuario;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
        }

        [Authorize]
        public ActionResult Perfil()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            var usuario = RepoUsuario.ObtenerPorEmail(con, User.Identity.Name);
            if (TempData.ContainsKey("Mensaje")){
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(usuario);
        }

        [Authorize]
        public ActionResult EditarPerfil()
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            var usuario = RepoUsuario.ObtenerPorEmail(con, User.Identity.Name);
            return View(usuario);
        }

        // GET: Usuario/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            ViewBag.Roles = Usuario.ObtenerRoles();
            var usuario = RepoUsuario.GetUsuario(con, id);
            return View(usuario);
        }

        // POST: Usuario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id, Usuario usuarioEdit)
        {
            var usuario = RepoUsuario.GetUsuario(con, id);
            try
            {
                if (usuarioEdit.Clave == null || usuarioEdit.Clave == "")
                {
                    usuarioEdit.Clave = usuario.Clave;
                }
                else
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: usuarioEdit.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8
                        ));
                    usuarioEdit.Clave = hashed;
                }

                if (usuarioEdit.AvatarFile != null)
                {
                    //borramos la imagen anterior
                    if (usuario.Avatar != null || usuario.Avatar != "")
                    {
                        string wwwPath_delete = environment.WebRootPath;
                        string filePath_delete = wwwPath_delete + usuario.Avatar;

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
                    string fileName = "avatar_" + usuarioEdit.IdUsuario + Path.GetExtension(usuarioEdit.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuarioEdit.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuarioEdit.AvatarFile.CopyTo(stream);
                    }
                }
                else
                {
                    usuarioEdit.Avatar = usuario.Avatar;
                }

                var res = RepoUsuario.UpdateUsuario(con, usuarioEdit);
                ViewBag.Roles = Usuario.ObtenerRoles();
                TempData["Mensaje"] = "El usuario se actualizo correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: Usuario/EditarPerfil/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult EditarPerfil(int id, Usuario usuarioEdit)
        {
            var usuario = RepoUsuario.ObtenerPorEmail(con, User.Identity.Name);
            id = usuario.IdUsuario;
            usuarioEdit.Rol = usuario.Rol;
            try
            {
                var usuarioActual = RepoUsuario.ObtenerPorEmail(con, User.Identity.Name);
                if (usuarioActual.IdUsuario != id)
                {
                    return RedirectToAction(nameof(Index), "Home");
                }

                if (usuarioEdit.Clave == null || usuarioEdit.Clave == "")
                {
                    usuarioEdit.Clave = usuario.Clave;
                }
                else
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                            password: usuarioEdit.Clave,
                            salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                            prf: KeyDerivationPrf.HMACSHA1,
                            iterationCount: 1000,
                            numBytesRequested: 256 / 8
                        ));
                    usuarioEdit.Clave = hashed;
                }

                if (usuarioEdit.AvatarFile != null)
                {
                    //borramos la imagen anterior
                    if (usuarioActual.Avatar != null || usuarioActual.Avatar != "")
                    {
                        string wwwPath_delete = environment.WebRootPath;
                        string filePath_delete = wwwPath_delete + usuarioActual.Avatar;

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
                    string fileName = "avatar_" + usuarioEdit.IdUsuario + Path.GetExtension(usuarioEdit.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usuarioEdit.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usuarioEdit.AvatarFile.CopyTo(stream);
                    }
                }
                else
                {
                    usuarioEdit.Avatar = usuario.Avatar;
                }

                usuarioEdit.IdUsuario = id;
                var res = RepoUsuario.UpdateUsuario(con, usuarioEdit);
                ViewBag.Roles = Usuario.ObtenerRoles();
                TempData["Mensaje"] = "Su perfil ha sido actualizado correctamente.";
                return RedirectToAction(nameof(Perfil));
            }
            catch
            {
                return View();
            }
        }

        // GET: Usuario/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Usuario usuario = RepoUsuario.GetUsuario(con, id);
            return View(usuario);
        }

        // POST: Usuario/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Usuario usuario)
        {
            try
            {
                var usuario_avatar = RepoUsuario.GetUsuario(con, id);
                var res = RepoUsuario.DeleteUsuario(con, id);
                if (res > 0)
                {
                    // Delete avatar image
                    if (usuario_avatar.Avatar != null || usuario_avatar.Avatar != "")
                    {
                        string wwwPath = environment.WebRootPath;
                        string filePath = wwwPath + usuario_avatar.Avatar;

                        if (System.IO.File.Exists(filePath) && Path.GetFileName(filePath) != "default.webp")
                        {
                            System.IO.File.Delete(filePath);
                            Console.WriteLine("Archivo eliminado exitosamente");
                        }
                    }
                }
                TempData["Mensaje"] = "El usuario ha sido eliminado.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}