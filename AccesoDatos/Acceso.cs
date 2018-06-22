using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
namespace AccesoDatos
{
    public class Acceso
    {
            private string ConexionString = "workstation id=smart-home-bd.mssql.somee.com;packet size=4096;user id=dc36257120_SQLLogin_1;pwd=x9jotz2km7;data source=smart-home-bd.mssql.somee.com;persist security info=False;initial catalog=smart-home-bd";
            private SqlConnection conexionBD = null;
            private SqlDataReader leerBD = null;
            private SqlCommand ejecutarComandos = null;
            private SqlParameter parametroUsuario = null;
            public SqlConnection conectarBD()
            {
                try
                {
                    conexionBD = new SqlConnection(ConexionString);
                    conexionBD.Open();
                    return conexionBD;
                }
                catch (Exception e)
                {
                    conexionBD.Close();
                    ConexionString = null;
                    conexionBD = null;
                    return conexionBD;
                }           
            }

            /// <summary>
            /// Setea un storedProcedure
            /// </summary>
            /// <param name="sp"></param>
            public void storedProcedure(String sp)
            {

                ejecutarComandos = new SqlCommand(sp, conexionBD);
                ejecutarComandos.CommandType = CommandType.StoredProcedure;

            }

            /// <summary>
            /// Agrega los parametros para ejecutar la consulta a un stored procedure. 
            /// parametroTabla= Hace referencia a los parametros que va recibir el sp
            /// parametroVista= Es el parametro que recibe desde el usuario.
            /// </summary>

            public void agregarParametros(string parametroSQL, object valor)
            {

                parametroUsuario = new SqlParameter(parametroSQL, valor);
                ejecutarComandos.Parameters.Add(parametroUsuario);


            }
            /// <summary>
            /// Ejecuta un sp que devuelve un escalar
            /// </summary>

            public int devolver_scalar()
            {
                return Convert.ToInt32(ejecutarComandos.ExecuteScalar());
            }
        
            /// <summary>
            /// Cierra la conexion de la BD.
            /// </summary>


            public void closeConexion()
            {
                conexionBD.Close();
            }

            /// <summary>
            /// Para leer datos de la BD.
            /// </summary>

            public SqlDataReader leerDatos()
            {

                leerBD = ejecutarComandos.ExecuteReader();
                return leerBD;

            }
        public void executeNonQuery()
        {
            ejecutarComandos.ExecuteNonQuery();
        }
            
        }

    }

