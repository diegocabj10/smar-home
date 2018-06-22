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
    public class RepositorioEventos
    {
        public static List<DtoEventos> ObtenerEventos()
        {
            List<DtoEventos> listaEventos = new List<DtoEventos>();
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_eventos_sf");
              
                SqlDataReader leerBD = acceso.leerDatos();
                while (leerBD.Read())
                {
                    DtoEventos evento = new DtoEventos();
                    evento.Id_Evento = (int)leerBD["id_evento"];
                    evento.Id_Arduino = (int)leerBD["id_arduino"];
                    evento.Id_Senal = (int)leerBD["id_senal"];
                    evento.Valor = (int)leerBD["valor"];
                    if (leerBD["fecha_evento"] != DBNull.Value) { evento.Fecha_Evento = (DateTime)leerBD["fecha_evento"]; }
                    listaEventos.Add(evento);
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

            return listaEventos;
        }

        public static void Guardar( DtoEventos dtoNuevo)
        {
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_eventos_g");
                acceso.agregarParametros("id_arduino", dtoNuevo.Id_Arduino);
                acceso.agregarParametros("id_senal", dtoNuevo.Id_Senal);
                acceso.agregarParametros("valor", dtoNuevo.Valor);
                acceso.agregarParametros("fecha_evento", dtoNuevo.Fecha_Evento);
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
