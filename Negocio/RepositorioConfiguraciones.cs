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
    public class RepositorioConfiguraciones
    {
        public static int ObtenerTiempoDelay(int idArduino, int idSenal)
        {
            int tiempo = 0;
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_configuracion_l");
                 acceso.agregarParametros("ID_ARDUINO", idArduino);
                acceso.agregarParametros("ID_SENAL", idSenal);
                tiempo = acceso.devolver_scalar();
                

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

            return tiempo;
        }
           /*
            *CREATE PROCEDURE PR_CONFIGURACION_L @ID_ARDUINO int, @ID_SENAL int
              AS
                BEGIN
                SET NOCOUNT ON;
                    SELECT MAX(N_DELAY_ALARMA)
                        FROM T_CONF_ARDUINO t WHERE t.ID_SENAL=@ID_SENAL AND t.ID_ARDUINO=@ID_ARDUINO
               END
               */
    }
}
