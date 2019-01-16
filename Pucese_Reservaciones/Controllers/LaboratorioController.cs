using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using CrystalDecisions.CrystalReports.Engine;
using Model;

namespace Pucese_Reservaciones.Controllers
{
    public class LaboratorioController : Controller
    {
        ReservacionesEntities db = new ReservacionesEntities();
        // GET: Escuela
        public ActionResult Index()
        {
            try
            {
                string sql = @"SELECT *FROM Laboratorio";
                using (var db = new ReservacionesEntities())
                {
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Laboratorio", "VISUALIZO LISTA  DE LABORATORIOS");
                    }
                    return View(db.Database.SqlQuery<Laboratorio>(sql).ToList());

                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Details(Laboratorio al)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    Laboratorio alu = db.Laboratorios.Find(al.Id);
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Laboratorio", "DETALLE DE LABORATORIOS");
                    }
                    return View(alu);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Encontrar al Usuario", ex);
                return View();
            }
        }
        public ActionResult ReporteUsr()
        {
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "ReportReserva.rpt"));

            rd.SetDataSource(db.Escuelas.ToList());
            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Lista_de_Escuelas.pdf");
            }
            catch
            {
                throw;
            }
        }
        public ActionResult Create(Laboratorio a)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    //Auditoria(Cedula, "Usuario", "AGREGO UN USUARIO");
                    db.Laboratorios.Add(a);
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Laboratorio", "CREO UN LABORATORIO");
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error al agregar una Escuela", ex);
                return View();
            }
        }




        public ActionResult Edit(int id)
        {
            try
            {
                using (var db = new ReservacionesEntities())
                {
                    //Alumno al = db.Alumno.Where(a => a.id == id).FirstOrDefault();
                    Laboratorio al2 = db.Laboratorios.Find(id);
                    return View(al2);
                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Editar al Alumno", ex);
                return View();
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Laboratorio a)
        {
            if (!ModelState.IsValid)
                return View();
            try
            {
                using (var db = new ReservacionesEntities())
                {

                    Laboratorio al = db.Laboratorios.Find(a.Id);
                    al.Nombre = a.Nombre;
                    var K = db.Usuarios.Single(u => u.Estado == "1");
                    if (K != null)
                    {
                        Auditoria(K.Cedula, "Laboratorio", "EDITO UN LABORATORIO");
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {

                ModelState.AddModelError("Error al Editar el motivo", ex);
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult Delete(int id, Laboratorio usu)
        {
            try
            {

                usu = db.Laboratorios.Find(usu.Id);
                db.Laboratorios.Remove(usu);
                var K = db.Usuarios.Single(u => u.Estado == "1");
                if (K != null)
                {
                    Auditoria(K.Cedula, "Laboratorio", "ELIMINO UN LABORATORIO");
                }
                db.SaveChanges();


                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public void Auditoria(string cedula, string tabla, string descripcion)
        {

            IDbConnection _conn = DBComon.Conexion();
            _conn.Open();
            SqlCommand _Comand = new SqlCommand("AGREGAR_AUDITORIA", _conn as SqlConnection);
            _Comand.CommandType = CommandType.StoredProcedure;
            _Comand.Parameters.Add(new SqlParameter("@IDUSUARIO", cedula));
            _Comand.Parameters.Add(new SqlParameter("@TBAUDITORIA", tabla));
            _Comand.Parameters.Add(new SqlParameter("@DESCRIPCION", descripcion));
            _Comand.Parameters.Add(new SqlParameter("@FECHA", DateTime.Now));
            _Comand.Parameters.Add(new SqlParameter("@HORA", DateTime.Now));
            int Resultado = _Comand.ExecuteNonQuery();
            _conn.Close();



        }
    }
}