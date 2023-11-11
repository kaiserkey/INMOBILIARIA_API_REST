
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Inmobiliaria.Models;

namespace Inmobiliaria.Controllers
{
    [Authorize]
    public class InquilinoController : Controller
    {

        private MySqlDatabase con { get; set; }
        private readonly RepositorioInquilino RepoInquilino;

        public InquilinoController()
        {
            con = new MySqlDatabase();
            RepoInquilino = new RepositorioInquilino();
        }

        // GET: Inquilino
        [Authorize]
        public ActionResult Index()
        {
            var listaInquilinos = RepoInquilino.GetInquilinos(con);
            ViewBag.Id = TempData["Id"];
            if (TempData.ContainsKey("Mensaje")){
                ViewBag.Mensaje = TempData["Mensaje"];
            }
            return View(listaInquilinos);
        }

        // GET: Inquilino/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            var inquilino = RepoInquilino.GetInquilino(con, id);
            return View(inquilino);
        }

        // GET: Inquilino/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilino/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {   
                var existEmail = RepoInquilino.GetInquilinoPorEmail(con, inquilino.Email);
                if(existEmail != null){
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    ViewBag.Error = "El email ya se encuentra registrado.";
                    return View();
                }
                RepoInquilino.CreateInquilino(con, inquilino);
                TempData["Id"] = inquilino.IdInquilino;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilino/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            var inquilino = RepoInquilino.GetInquilino(con, id);
            return View(inquilino);
        }

        // POST: Inquilino/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                int res = RepoInquilino.UpdateInquilino(con, inquilino);
                TempData["Mensaje"] = "La entidad se actualizo correctamente ID:" + id;
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Inquilino/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            var inquilino = RepoInquilino.GetInquilino(con, id);
            return View(inquilino);
        }

        // POST: Inquilino/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                RepoInquilino.DeleteInquilino(con, id);
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