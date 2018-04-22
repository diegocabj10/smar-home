using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DtoAbmBase
    {
        public virtual string Id { get; set; }
        public virtual DateTime? FechaBaja { get; set; }
        public virtual string Activo { get; set; }
    }
}
