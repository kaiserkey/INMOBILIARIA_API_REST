
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class InmuebleController : Controller
    {
        private MySqlDatabase con { get; set; }
        private readonly RepositorioInmueble RepoInmueble;
        private readonly RepositorioPropietario RepoPropietario;
        public InmuebleController() { 
            con = new MySqlDatabase(); 
            RepoInmueble = new RepositorioInmueble();
            RepoPropietario = new RepositorioPropietario();
        }

        // GET: Inmueble
        [Authorize]
        public ActionResult Index()
        {
            var listaInmuebles = RepoInmueble.GetInmuebles(con);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje")){
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(listaInmuebles);
        }

        //obtener Inmuebles por JQuery
        public IActionResult BuscarInmuebles(string busqueda, string opcion)
        {
            var inmuebles = new List<Inmueble>();
            
            inmuebles = RepoInmueble.BuscarInmuebles(con, busqueda, opcion);

            var resultados = inmuebles.Select(i => new
            {
                idInmueble = i.IdInmueble,
                tipo = i.Tipo,
                direccion = i.Direccion,
                coordenadas = i.Coordenadas,
                precio = i.Precio,
                ambientes = i.Ambientes,
                uso = i.Uso,
                nombre = i.Propietario.Nombre,
                apellido = i.Propietario.Apellido,
            });

            return Json(resultados);
        }

        //buscar disponibles Inmuebles por JQuery
        public IActionResult BuscarInmueblesSinContrato(string fechaInicio, string fechaFin)
        {
            var inmuebles = new List<Inmueble>();
            
            inmuebles = RepoInmueble.BuscarInmueblesSinContrato(con, fechaInicio, fechaFin);

            var resultados = inmuebles.Select(i => new
            {
                idInmueble = i.IdInmueble,
                tipo = i.Tipo,
                direccion = i.Direccion,
                coordenadas = i.Coordenadas,
                precio = i.Precio,
                ambientes = i.Ambientes,
                uso = i.Uso,
                nombre = i.Propietario.Nombre,
                apellido = i.Propietario.Apellido,
            });

            return Json(resultados);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // GET: Inmueble/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var inmueble = RepoInmueble.GetInmueble(con, id);
            return View(inmueble);
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {
                RepoInmueble.CreateInmueble(con, inmueble);
                TempData["Id"] = inmueble.IdInmueble;
                return RedirectToAction(nameof(Index));
            }
            
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Inmueble inmueble = RepoInmueble.GetInmueble(con, id);
            
            return View(inmueble);
        }

        //obtener propietarios por JQuery
        public IActionResult BuscarPropietarios(string busqueda)
        {
            var propietarios = new List<Propietario>();
            propietarios = RepoPropietario.BuscarPropietario(con, busqueda);

            var resultados = propietarios.Select(p => new
            {
                idPropietario = p.IdPropietario,
                nombre = p.Nombre,
                apellido = p.Apellido,
                telefono = p.Telefono,
                email = p.Email,
            });

            return Json(resultados);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble UpdateInmueble)
        {
            try
            {
                int res = RepoInmueble.UpdateInmueble(con, UpdateInmueble);
                TempData["Mensaje"] = "La entidad se actualizo correctamente ID:" + id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inmueble/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Inmueble inmueble = RepoInmueble.GetInmueble(con, id);

            return View(inmueble);
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                int res = RepoInmueble.DeleteInmueble(con, id);
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