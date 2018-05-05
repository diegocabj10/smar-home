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


        //        select *
        //from INFORMATION_SCHEMA.COLUMNS
        //where TABLE_NAME='T_EVENTOS'
        //SELECT OBJECT_DEFINITION(OBJECT_ID(N'PR_NOTIFICACIONES_SF'))

        //        CREATE TABLE T_EVENTOS(
        //ID_EVENTO int not null identity(1,1) unique,
        //ID_ARDUINO int,
        //ID_SENAL int,
        //N_VALOR int,
        //FECHA_EVENTO datetime );
        public static List<DtoEventos> ObtenerEventos()
        {
            List<DtoEventos> listaEventos = new List<DtoEventos>();
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_eventos_sf");
                // acceso.agregarParametros("p_apellido", parametro);
                SqlDataReader leerBD = acceso.leerDatos();
                while (leerBD.Read())
                {
                    //Creo una entidad para guardar lo que viene de la 
                    DtoEventos evento = new DtoEventos();
                    evento.Id_Evento = (int)leerBD["id_evento"];
                    evento.Id_Arduino = (int)leerBD["id_arduino"];
                    evento.Id_Senal = (int)leerBD["id_senal"];
                    evento.Valor = (int)leerBD["valor"];
                    if (leerBD["fecha_evento"] != DBNull.Value) { evento.Fecha_Evento = (DateTime)leerBD["fecha_evento"]; }
                    //    cliente.pais = leerBD["descripcion"].ToString();
                    //  cliente.telefono = (int)leerBD["telefono"];
                    // if (leerBD["email"] != DBNull.Value) { cliente.email = leerBD["email"].ToString(); }
                    //  cliente.fecha_nacimiento = (DateTime)leerBD["fecha_nac"];
                    //cliente.promo_mail = (Boolean)leerBD["promo_mail"];
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
//        CREATE PROCEDURE PR_EVENTOS_G @ID_ARDUINO int, @ID_SENAL int,@N_VALOR int AS
//BEGIN
//SET NOCOUNT ON
//INSERT INTO T_EVENTOS(ID_ARDUINO, ID_SENAL, N_VALOR, FECHA_EVENTO)
//VALUES(@ID_ARDUINO, @ID_SENAL, @N_VALOR, GETDATE())
//END
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
