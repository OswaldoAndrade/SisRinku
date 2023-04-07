using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebRinku.Controllers
{
    public class MovimientosController : Controller
    {
        public HttpClient _client = new HttpClient();
        public string url = ConfigurationManager.AppSettings["ApiUrl"];
        //public Connection_Query connection_Query;


        public class Respuesta
        {
            public bool Error { get; set; }
            public string Data { get; set; }
        }

        //Obtener lista de empleados
        public async Task<ActionResult> GetMovimientos()
        {

            Uri uri = new Uri(string.Format(url + "Movimiento/", string.Empty));
            ActionResult res = null;

            try
            {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    res = Json(new Respuesta { Error = false, Data = content }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                res = Json(new Respuesta { Error = true, Data = ex.Message });
            }

            return res;
        }



        // GET: Movimientos
        public ActionResult Index()
        {
            return View();
        }

        //// GET: Movimientos/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Movimientos/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Movimientos/Create
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

        //// GET: Movimientos/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Movimientos/Edit/5
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

        //// GET: Movimientos/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Movimientos/Delete/5
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
