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
using System.Web.Security;
using System.Configuration;

namespace WebApplication1.Controllers
{
    public class EventosController : Controller
    {
        //// GET: Eventos
        //public ActionResult Index()
        //{
        //    return View();
        //}
      

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
                if (evento.Id_Senal == 1 && evento.Id_Senal == 1) //Si la señal es de luz y es cuando vuelve
                {
                    System.Web.HttpContext.Current.Cache.Insert("NotifacionLuz" + evento.Id_Arduino, evento, null, DateTime.Now.AddMinutes(3), System.Web.Caching.Cache.NoSlidingExpiration);
                }
                evento.Fecha_Evento = DateTime.Now;
                RepositorioEventos.Guardar(evento);
                return new HttpStatusCodeResult(HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

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


    


    }

}
