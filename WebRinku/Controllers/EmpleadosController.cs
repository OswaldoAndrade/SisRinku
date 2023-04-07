using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using System.Net.Http;
using System.Data.SqlClient;
using WebRinku.Classes;
using System.Data;
using System.Web.Services;
using System.Configuration;

using WebRinku.Models;
using Newtonsoft.Json;
using System.Text;

namespace WebRinku.Controllers
{
    public class EmpleadosController : Controller
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
        public async Task<ActionResult> GetEmpleados()
        {

            Uri uri = new Uri(string.Format(url + "Empleado/", string.Empty));
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


        //Obtener lista de roles
        public async Task<ActionResult> GetRoles()
        {

            Uri uri = new Uri(string.Format(url + "Rol/", string.Empty));
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

        //Agregar o actualizar un empleado 
        public async Task<ActionResult> PostEmpleado(int Id, string Nombre, int Rol, bool Estatus)
        {
            Uri uri = new Uri(string.Format(url + "Empleado/", string.Empty));
            ActionResult res = null;



            Empleado obj = new Empleado
            {
                Id = Id,
                Nombre = Nombre,
                Rol = Rol,
                Estatus = Estatus
            };

            try
            {
                string json = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                if (Id == 0)
                    response = await _client.PostAsync(uri, content);
                else
                    response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    res = Json(new Respuesta { Error = false, Data = json });
                }

            }
            catch (Exception ex)
            {
                res = Json(new Respuesta { Error = true, Data = ex.Message });
            }
            return res;
        }

        public async Task<ActionResult> PostMovimiento(int Id, int Mes, int Entregas )
        {
            Uri uri = new Uri(string.Format(url + "Movimiento/", string.Empty));
            ActionResult res = null;



            Movimiento obj = new Movimiento
            {
                IdEmpleado = Id,
                Mes = Mes,
                Entregas = Entregas
            };

            try
            {
                string json = JsonConvert.SerializeObject(obj);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null; 
                response = await _client.PostAsync(uri, content); 

                if (response.IsSuccessStatusCode)
                {
                    string respuesta = await response.Content.ReadAsStringAsync();
                    res = Json(new Respuesta { Error = false, Data = respuesta });
                }

            }
            catch (Exception ex)
            {
                res = Json(new Respuesta { Error = true, Data = ex.Message });
            }
            return res;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Control de nómina para Rinku";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "LSC. Abraham Andrade García";

            return View();
        } 
    }
}