using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Model;


namespace Pucese_Reservaciones.Controllers
{
    public class AuditoriaController : Controller
    {
        ReservacionesEntities db = new ReservacionesEntities();
        public ActionResult Repor()
        {
            return View();
        }

        public ActionResult GetData()
        {
            int admin = db.Roles.Where(x => x.Nombre == "AMINISTRADOR").Count();
            int recepcion = db.Roles.Where(x => x.Nombre == "RECEPCION").Count();
            int doncente = db.Roles.Where(x => x.Nombre == "DOCENTES").Count();

            Clase obj = new Clase();
            obj.login = admin;
            obj.visual = recepcion;
            obj.doncente = doncente;
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        public class Clase
        {
            public int login;
            public int visual;
            public int doncente;

        }
        public ActionResult Index()
        {
            try
            {
                string sql = @"SELECT  Usuario.Cedula AS IDusuario, Auditoria.Fecha, Auditoria.Descripcion,
		                    Auditoria.TbAuditoria, Auditoria.Hora FROM Auditoria
                               INNER JOIN Usuario ON Auditoria.IdUsuario = Usuario.Cedula";
                using (var db = new ReservacionesEntities())
                {
                    
                    return View(db.Database.SqlQuery<CEAuditoria>(sql).ToList());

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: Auditoria/Details/5
        public ActionResult Administracion()
        {
            return View();
        }

        public ActionResult Details(Auditoria al)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    Auditoria alu = db.Auditorias.Find(al.Id);
                   
                    return View(alu);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Encontrar Historial", ex);
                return View();
            }
        }





    }
}
