﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DtoEventos
    {
        public int Id_Evento { get; set; }
        public int Id_Arduino { get; set; }
        public int Id_Senal { get; set; }
        public int Valor { get; set; }
        public DateTime Fecha_Evento { get; set; }
    }
}
