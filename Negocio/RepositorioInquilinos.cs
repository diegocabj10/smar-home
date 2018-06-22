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
    public class RepositorioInquilinos
    {
        public static void InsertarInquilinos(DtoInquilinos dtoInquilino)
        {
            Acceso acceso = new Acceso();
            try
            {
                acceso.conectarBD();
                acceso.storedProcedure("pr_InsertatInquilinos_g");
                acceso.agregarParametros("ID_EDIFICIO", dtoInquilino.Id_Edificio);
                acceso.agregarParametros("V_NOMBRE", dtoInquilino.V_Nombre);
                acceso.agregarParametros("V_APELLIDO", dtoInquilino.V_Apellido);
                acceso.agregarParametros("V_MAIL", dtoInquilino.V_Mail);
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
