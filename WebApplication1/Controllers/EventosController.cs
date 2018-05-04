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
            System.Web.Caching.CacheItemRemovedCallback callback = new System.Web.Caching.CacheItemRemovedCallback(OnRemove);
            Stream req = Request.InputStream;
            req.Seek(0, System.IO.SeekOrigin.Begin);
            string json = new StreamReader(req).ReadToEnd();

            string _mensaje = "";
            int tiempoEnCache = 5; //Si no lo encuentra por defecto son 5 (expresado en minutos)..
           
            try
            {

                DtoEventos evento = Newtonsoft.Json.JsonConvert.DeserializeObject<DtoEventos>(json);
                int tiempo = RepositorioConfiguraciones.ObtenerTiempoDelay(evento.Id_Arduino, evento.Id_Senal);
                guardarEvento(evento); //Todos los eventos se guardan
                if (tiempo != -1) tiempoEnCache = tiempo;
                if (evento.Id_Senal == 1 ) //Si la señal es de luz lo manejo, sino lo guardo de una..
                {
                    if (estaEnCache("NotifacionLuz" + evento.Id_Arduino))
                    {
                        System.Web.HttpContext.Current.Cache.Remove("NotifacionLuz" + evento.Id_Arduino);
                        if (evento.Valor == 1) 
                            System.Web.HttpContext.Current.Cache.Insert("NotifacionLuz" + evento.Id_Arduino, evento, null, DateTime.Now.AddMinutes(tiempoEnCache), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, callback);
                    }
                    else
                    {
                        if (evento.Valor == 1)
                        System.Web.HttpContext.Current.Cache.Insert("NotifacionLuz" + evento.Id_Arduino, evento, null, DateTime.Now.AddMinutes(tiempoEnCache), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Default, callback);
                        else
                        {
                            guardarNotificacion(evento);
                           
                        }
                    }
                   
                    
                }
                else
                {
                    guardarNotificacion(evento);
                }
             
                return new HttpStatusCodeResult(HttpStatusCode.Created);

            }
            catch (Exception ex)
            {
                _mensaje = ex.Message;
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }

        public void guardarEvento(DtoEventos evento)
        {
            evento.Fecha_Evento = DateTime.Now;
            RepositorioEventos.Guardar(evento);
        }

        public static void guardarNotificacion(DtoEventos evento)
        {
            evento.Fecha_Evento = DateTime.Now;
            RepositorioNotificaciones.Guardar(evento);
            //Aca va la llamada a la funcion que envia el correo...
        }

        private Boolean estaEnCache(String keyCache)
        {
            DtoEventos eventoEnCache=(DtoEventos) System.Web.HttpContext.Current.Cache.Get(keyCache);
            if (eventoEnCache == null) return false;
            else return true;
          
        }

        private static void OnRemove(string key,
                                       object cacheItem,
                                       System.Web.Caching.CacheItemRemovedReason reason)
        {
            String test = reason.ToString();
            if (reason.ToString() == "Expired")
            {
                DtoEventos evento = (DtoEventos)cacheItem;
                
                guardarNotificacion(evento);
               

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
