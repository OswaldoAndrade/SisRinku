using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Http;
using System.Data.SqlClient;

using RinkuAPI.Models;
using RinkuAPI.Classes;
using RinkuAPI.Classes.Enums;


namespace RinkuAPI.Controllers
{
    public class MovimientoController : ApiController
    {
        public DatabaseConnection dbconn = new DatabaseConnection();

        public class Respuesta
        {
            public int Exito { get; set; }
            public string Data { get; set; }
        }

        public List<Movimiento> Get()
        {
            dbconn = new DatabaseConnection();
            dbconn.OpenConection();

            SqlDataReader dataReader = dbconn.DataReader("GetMovimientos");
            List<Movimiento> listaMovimientos = new List<Movimiento>();

            while (dataReader.Read())
            {
                listaMovimientos.Add(new Movimiento
                {
                    Id = Convert.ToInt32(dataReader["Id"]),
                    IdEmpleado = Convert.ToInt32(dataReader["IdEmpleado"]),
                    Nombre = dataReader["Nombre"].ToString(),
                    Rol = dataReader["Rol"].ToString(),
                    Mes = Enum.GetName(typeof(Meses), Convert.ToInt32(dataReader["Mes"])),
                    Entregas = Convert.ToInt32(dataReader["Entregas"]),
                    SueldoBruto = dataReader["SueldoBruto"].ToString(),
                    Retenciones = dataReader["Retenciones"].ToString(),
                    BonoDespensa = dataReader["BonoDespensa"].ToString(),
                    SueldoNeto = dataReader["SueldoNeto"].ToString()
                });
            }

            return listaMovimientos;
        }

        public Respuesta Post([FromBody] Movimiento movimiento)
        {
            dbconn = new DatabaseConnection();
            dbconn.OpenConection();

            SqlDataReader dataReader = dbconn.DataReader($"AddOrUpdateMovimiento '{movimiento.IdEmpleado}', {movimiento.Mes}, {movimiento.Entregas}");
            Respuesta res = new Respuesta();

            if (dataReader.Read())
            {
                res.Exito = Convert.ToInt32(dataReader["Exito"]);
                res.Data = dataReader["Mensaje"].ToString();
            }

            return res;
        }

        //// GET: Movimiento
        //public ActionResult Index()
        //{
        //    return View();
        //}

        //// GET: Movimiento/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Movimiento/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Movimiento/Create
        //[HttpPost]
        //public ActionResult Create(FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Movimiento/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Movimiento/Edit/5
        //[HttpPost]
        //public ActionResult Edit(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Movimiento/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Movimiento/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
