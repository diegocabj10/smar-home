using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using DTO;
using AccesoDatos;
namespace Negocio
{
    public class RepositorioNotificaciones
    {



        public static List<DtoNotificaciones> ObtenerNotificaciones()
        {
            List<DtoNotificaciones> listaNotificaciones = new List<DtoNotificaciones>();
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("PR_NOTIFICACIONES_SF");
                // acceso.agregarParametros("p_apellido", parametro);
                SqlDataReader leerBD = acceso.leerDatos();
                while (leerBD.Read())
                {
                    //Creo una entidad para guardar lo que viene de la 
                    DtoNotificaciones notificacion = new DtoNotificaciones();

                    notificacion.Id_Arduino = (int)leerBD["id_arduino"];
                    notificacion.V_Descripcion_Senal = leerBD["V_Descripcion_Senal"].ToString();
                    notificacion.Valor = (int)leerBD["valor"];
                    if (leerBD["fecha_notificacion"] != DBNull.Value) { notificacion.Fecha_Notificacion = (DateTime)leerBD["fecha_notificacion"]; }
                    listaNotificaciones.Add(notificacion);
                }

            }
            catch (Exception ex)
            {
                var a = "Error message: " + ex.Message;

                if (ex.InnerException != null)
                {
                    a = a + " Inner exception: " + ex.InnerException.Message;
                }

                a = a + " Stack trace: " + ex.StackTrace;
                System.ArgumentException bdEX = new System.ArgumentException("Mensaje: " + a, ex);
                throw bdEX;
            }

            finally
            {
                acceso.closeConexion();
            }

            return listaNotificaciones;
        }

        public static void Guardar(DtoNotificaciones dtoNuevo)
        {
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_notificaciones_g");
                acceso.agregarParametros("id_arduino", dtoNuevo.Id_Arduino);
                acceso.agregarParametros("id_senal", dtoNuevo.Id_Senal);
                acceso.agregarParametros("valor", dtoNuevo.Valor);
                acceso.agregarParametros("fecha_notificacion", dtoNuevo.Fecha_Notificacion);
                acceso.executeNonQuery();
            }
            catch (Exception ex)
            {
                var a = "Error message: " + ex.Message;

                if (ex.InnerException != null)
                {
                    a = a + " Inner exception: " + ex.InnerException.Message;
                }

                a = a + " Stack trace: " + ex.StackTrace;

                System.ArgumentException bdEX = new System.ArgumentException("Mensaje: " + a, ex);


                throw bdEX;

            }

            finally
            {
                acceso.closeConexion();
            }

        }

        public static String ObtenerDescripcionSenal(DtoNotificaciones dtoNuevo)
        {
            String salida = "";
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("PR_SENAL_L");

                acceso.agregarParametros("id_senal", dtoNuevo.Id_Senal);
                SqlDataReader leerBD = acceso.leerDatos();
                while (leerBD.Read())
                {
                    //Creo una entidad para guardar lo que viene de la 
                    DtoConfiguracion evento = new DtoConfiguracion();
                    salida = (String)leerBD["v_descripcion"];

                }
            }
            catch (Exception ex)
            {
                var a = "Error message: " + ex.Message;
                if (ex.InnerException != null)
                {
                    a = a + " Inner exception: " + ex.InnerException.Message;
                }
                a = a + " Stack trace: " + ex.StackTrace;
                System.ArgumentException bdEX = new System.ArgumentException("Mensaje: " + a, ex);
                throw bdEX;
            }
            finally
            {
                acceso.closeConexion();
            }
            return salida;

        }


    }
}
