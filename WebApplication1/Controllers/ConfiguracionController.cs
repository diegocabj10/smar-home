using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Negocio;
using DTO;
using System.Web.Helpers;
using System.IO;
using System.Net;

namespace WebApplication1.Controllers
{
    public class ConfiguracionController : Controller
    {
        // GET: Eventos
        public ActionResult Index()
        {
            return View();
        }
        [JsonFechasStringFilter]
        public JsonResult Buscar()
        {
            List<DtoConfiguracion> a = new List<DtoConfiguracion>();
            string _mensaje = "";
            try
            {                
                a = RepositorioConfiguraciones.ObtenerIdArduino();

            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
            }

            return Json(new { Lista = a, Salida = _mensaje });            
        }     
    }
}
