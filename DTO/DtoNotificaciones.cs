using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DtoNotificaciones
    {        
        public int Id_Notificacion { get; set; }
        public int Id_Arduino { get; set; }
        public int Id_Senal { get; set; }
        public string V_Descripcion_Senal { get; set; }
        public int Valor { get; set; }
        public DateTime Fecha_Notificacion { get; set; }
    }
}
