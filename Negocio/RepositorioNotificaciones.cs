using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using DTO;
using AccesoDatos;
namespace Negocio
{
    public class RepositorioNotificaciones
    {


       
        public static List<DtoNotificaciones> ObtenerNotificaciones()
        {
            //            CREATE PROCEDURE PR_NOTIFICACIONES_SF
            //AS
            //BEGIN
            //    SET NOCOUNT ON;
            //            SELECT t.ID_ARDUINO, t.ID_EVENTO,t.ID_SENAL, s.V_DESCRIPCION V_Descripcion_Senal, t.VALOR, t.FECHA_EVENTO fecha_notificacion
            //            FROM T_EVENTO t Join T_SENAL s on t.ID_SENAL = s.ID_SENAL

            //    END

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
                    //    cliente.pais = leerBD["descripcion"].ToString();
                    //  cliente.telefono = (int)leerBD["telefono"];
                    // if (leerBD["email"] != DBNull.Value) { cliente.email = leerBD["email"].ToString(); }
                    //  cliente.fecha_nacimiento = (DateTime)leerBD["fecha_nac"];
                    //cliente.promo_mail = (Boolean)leerBD["promo_mail"];
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

        public static void Guardar(DtoEventos dtoNuevo)
        {
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_notificaciones_g");
                acceso.agregarParametros("id_arduino", dtoNuevo.Id_Arduino);
                acceso.agregarParametros("id_senal", dtoNuevo.Id_Senal);
                acceso.agregarParametros("valor", dtoNuevo.Valor);
                acceso.agregarParametros("fecha_notificacion", dtoNuevo.Fecha_Evento);
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

    }
}
