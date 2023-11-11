
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class PagoController : Controller
    {
        private MySqlDatabase con { get; set; }
        private readonly RepositorioPago RepoPago;
        private readonly RepositorioContrato RepoContrato;
        public PagoController()
        {
            con = new MySqlDatabase();
            RepoPago = new RepositorioPago();
            RepoContrato = new RepositorioContrato();
        }
        // GET: Pago
        [Authorize]
        public ActionResult Index()
        {
            var listaPagos = RepoPago.GetPagos(con);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje"))
            {
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(listaPagos);
        }

        // GET: Pago/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            Pago pago = RepoPago.GetPago(con, id);
            return View(pago);
        }

        // GET: Pago/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult CreateModal(int id)
        {
            ViewBag.IdContrato = id;
            return View();
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Pago pago)
        {
            try
            {
                var res = RepoPago.CreatePago(con, pago);
                TempData["Id"] = pago.IdPago;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /* buscar contratos por jquery */
        public IActionResult BuscarContratos(string busqueda, string opcion)
        {
            var contrato = new List<Contrato>();
            
            contrato = RepoContrato.BuscarContrato(con, busqueda, opcion);
            
            var resultados = contrato.Select(c => new
            {
                idContrato = c.IdContrato,
                idInmueble = c.IdInmueble,
                idInquilino = c.IdInquilino,
                nombre = c.Inquilino.Nombre,
                apellido = c.Inquilino.Apellido,
                dni = c.Inquilino.Dni,
                fechaFin = c.FechaFin,
                fechaInicio = c.FechaInicio,
            });

            return Json(resultados);
        }

        /* buscar pagos por jquery */
        public IActionResult BuscarPagos(int codigo)
        {
            var pago = new List<Pago>();
            
            pago = RepoPago.BuscarPagos(con, codigo);
            
            var resultados = pago.Select(p => new
            {
                idPago = p.IdPago,
                fecha = p.Fecha,
                numeroPago = p.NumeroPago,
                importe = p.Importe,
                idContrato = p.IdContrato,
            });
            
            return Json(resultados);
        }

        // GET: Pago/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Pago pago = RepoPago.GetPago(con, id);
            return View(pago);
        }

        // POST: Pago/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {
                var res = RepoPago.UpdatePago(con, pago);
                TempData["Mensaje"] = "La entidad se actualizo correctamente ID:" + id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pago/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            Pago pago = RepoPago.GetPago(con, id);
            return View(pago);
        }

        // POST: Pago/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                var res = RepoPago.DeletePago(con, id);
                TempData["Mensaje"] = "La entidad se ha elimino correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}