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
using System.Threading;
using System.Globalization;
using System.Net.Mail;
namespace WebApplication1.Controllers
{
    public class EventosController : Controller
    {
        //// GET: Eventos
        public ActionResult Index()
        {
            ViewBag.Title = "Eventos";
            return View();
        }


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
                if (evento.Id_Senal == 1) //Si la señal es de luz lo manejo, sino lo guardo de una..
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
            evento.Fecha_Evento = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"));
            RepositorioEventos.Guardar(evento);
        }

        public static void guardarNotificacion(DtoEventos evento)
        {
            DtoNotificaciones notificacion = new DtoNotificaciones();
            notificacion.Id_Arduino = evento.Id_Arduino;
            notificacion.Id_Senal = evento.Id_Senal;
            notificacion.Valor = evento.Valor;
            notificacion.Fecha_Notificacion = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("Argentina Standard Time"));
            RepositorioNotificaciones.Guardar(notificacion);
           
            enviarCorreoNotificacion(notificacion);
        }

        private Boolean estaEnCache(String keyCache)
        {
            DtoEventos eventoEnCache = (DtoEventos)System.Web.HttpContext.Current.Cache.Get(keyCache);
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



        public static void enviarCorreoNotificacion(DtoNotificaciones dto)
        {

            String descripcionSenal = RepositorioNotificaciones.ObtenerDescripcionSenal(dto);
            String mensaje = "";
            if (descripcionSenal == "LUZ")
            {
                if (dto.Valor == 1)
                {
                    mensaje = "Se ha normalizado el suministro de energía eléctrica.";
                }
                else
                {
                    mensaje = "Se ha interrumpido el suministro de energía eléctrica.";
                }
            }
            if (descripcionSenal == "GAS")
            {
                if (dto.Valor < 500)
                {
                    mensaje = "Se ha detectado una pérdida de gas.";
                }
                else
                {
                    mensaje = "El nivel de gas en el ambiente, esta dentro de las condiciones normales.";
                }
            }

            MailMessage mail = new MailMessage();
            mail.From = new System.Net.Mail.MailAddress("someesmarthome@gmail.com");
            mail.Subject = "Notificación SMART-HOME";
            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;   //  465 
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("someesmarthome@gmail.com", "viaje1234");
            smtp.Host = "smtp.gmail.com";
            //Hay que parametrizar las cuentas a enviar, y los mensajes segun señal y valor
             mail.To.Add(new MailAddress("diegocampos0909@gmail.com"));
            //mail.To.Add(new MailAddress("francoluna@gmail.com"));
            mail.IsBodyHtml = true;

            string st = @"<html>
                            <head>
                            <title>Querido vecino</title>
                            </head>
                            <body> 
                            <h1>";
                st += mensaje;
                 st+=@"</h1>
                            <p>Smart-Home</p>
                            </body>
                            </html>";
            mail.Body = st;
            smtp.Send(mail);

        }





    }

}
