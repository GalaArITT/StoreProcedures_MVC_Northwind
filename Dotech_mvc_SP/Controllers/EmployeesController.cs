using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Dotech_mvc_SP.Models;

namespace Dotech_mvc_SP.Controllers
{
    public class EmployeesController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();

        // GET: Employees
        public ActionResult Index()
        {
            //conexion
            IEnumerable<sp_ventas_empleado_Result> llamada_sp;
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //llamar filtrar todos
                llamada_sp = db.Database.SqlQuery<sp_ventas_empleado_Result>
                    ("exec sp_ventas_empleado").ToList();
            }
            return View(llamada_sp);
        }

        // GET: Employees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employees employees = db.Employees.Find(id);
            if (employees == null)
            {
                return HttpNotFound();
            }
            return View(employees);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Shippers shippers)
        {
            if (ModelState.IsValid)
            {
                using (NorthwindEntities db = new NorthwindEntities())
                {
                    //definir los parametros (eso se hace primero)
                    SqlParameter companyName = new SqlParameter("@companyName", shippers.CompanyName);
                    SqlParameter phone = new SqlParameter("@phone", shippers.Phone);

                    //llamar al sp de insercion
                    var sp_insertar = db.Database.
                        ExecuteSqlCommand("EXEC SP_INSERTAR_SHIPP  @companyName,@phone",
                        companyName, phone);
                }
                    return RedirectToAction("Index");
            }

            return View(shippers);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shippers llamar_sp_de_id = null;
            //conexiom
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //
                SqlParameter parametro_id = new SqlParameter("@id", id);
                //llamar al sp que trae un solo elemento
                llamar_sp_de_id = db.Database.SqlQuery<Shippers>("exec sp_getShipp @id", parametro_id).SingleOrDefault();
            }

            return View(llamar_sp_de_id);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Shippers shippers, int id)
        {
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //sp_updateUsuariosLogin @id, @companyName, @phone  
                //llamar parametros
                SqlParameter _id = new SqlParameter("@id", id);
                SqlParameter companyName = new SqlParameter("@companyName", shippers.CompanyName);
                SqlParameter phone = new SqlParameter("@phone", shippers.Phone);
                //llamar sp 
                var sp_update = db.Database.ExecuteSqlCommand("SP_UPDATE @id, @companyName, @phone",
                    _id, companyName, phone);
            }
            return RedirectToAction("Index");
        }

        // GET: Employees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shippers llamar_sp_de_id = null;
            //conexiom
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //
                SqlParameter parametro_id = new SqlParameter("@id", id);
                //llamar al sp que trae un solo elemento
                llamar_sp_de_id = db.Database.SqlQuery<Shippers>("exec sp_getShipp @id", parametro_id).SingleOrDefault();
            }

            return View(llamar_sp_de_id);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //conectar fuera
            using (NorthwindEntities db = new NorthwindEntities())
            {
                //parametros
                SqlParameter _id = new SqlParameter("@id", id);
                //llamar al sp
                var eliminar = db.Database.ExecuteSqlCommand("exec sp_borrar_ship @id", _id);
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
