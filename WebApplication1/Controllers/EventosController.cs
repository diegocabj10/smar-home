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
    public class EventosController : Controller
    {
        // GET: Eventos   -- TEST CLAUDIO
        public ActionResult Index()
        {
            return View();
        }
        [JsonFechasStringFilter]
        public JsonResult Buscar()
        {
            List<DtoEventos> a = new List<DtoEventos>();
            string _mensaje = "";
            try
            {
                a = RepositorioEventos.ObtenerEventos();
               
            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
            }


            return Json(new { Lista = a, Salida = _mensaje });
            //var listaEventos = new List<DtoEventos>();

            //DtoEventos evento = new DtoEventos();
            //evento.Id_Evento = 2;
            //evento.Id_Arduino = 2;
            //evento.Id_Senal = 2;
            //evento.N_Valor = 2;
            //evento.Fecha_Evento = DateTime.Today;
            //evento.TotalRegistrosListado = 10;
            //listaEventos.Add(evento);

            //return Json(new
            //{
            //    Lista = listaEventos
            //});

        }
        /* 
         * 
         * AGREGADO POR FRANCO - DESDE
         *   
        */
        [HttpPost]
        public ActionResult RecibirEvento(int? id)
        {

            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();


            string _mensaje = "";
            try
            {

                DtoEventos evento = Newtonsoft.Json.JsonConvert.DeserializeObject<DtoEventos>(json);
                RepositorioEventos.Guardar(evento);
                return new HttpStatusCodeResult(HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
        /* 
           * 
           * AGREGADO POR FRANCO - HASTA
           *   
          */
        //// GET: Eventos/Details/5
        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        //// GET: Eventos/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Eventos/Create
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

        //// GET: Eventos/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Eventos/Edit/5
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

        //// GET: Eventos/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Eventos/Delete/5
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


        public class JsonFechasStringFilterAttribute : ActionFilterAttribute
        {
            public override void OnActionExecuted(ActionExecutedContext filterContext)
            {
                if (filterContext.Result is JsonResult == false)
                    return;

                filterContext.Result = new JsonNetResult((JsonResult)filterContext.Result);
            }

            private class JsonNetResult : JsonResult
            {
                public JsonNetResult(JsonResult jsonResult)
                {
                    this.ContentEncoding = jsonResult.ContentEncoding;
                    this.ContentType = jsonResult.ContentType;
                    this.Data = jsonResult.Data;
                    this.JsonRequestBehavior = jsonResult.JsonRequestBehavior;
                    //this.MaxJsonLength = jsonResult.MaxJsonLength;
                    //this.RecursionLimit = jsonResult.RecursionLimit;
                }

                public override void ExecuteResult(ControllerContext context)
                {
                    if (context == null)
                        throw new ArgumentNullException("context");

                    if (this.JsonRequestBehavior == JsonRequestBehavior.DenyGet
                        && String.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("GET not allowed! Change JsonRequestBehavior to AllowGet.");

                    var response = context.HttpContext.Response;

                    response.ContentType = String.IsNullOrEmpty(this.ContentType) ? "application/json" : this.ContentType;

                    if (this.ContentEncoding != null)
                        response.ContentEncoding = this.ContentEncoding;

                    if (Data != null)
                    {
                        // Using Json.NET serializer
                        var isoConvert = new Newtonsoft.Json.Converters.IsoDateTimeConverter();
                        const string _dateFormat = "dd/MM/yyyy HH:mm:ss";
                        isoConvert.DateTimeFormat = _dateFormat;
                        response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(Data, isoConvert));
                    }
                }
            }


        }



    }

}
