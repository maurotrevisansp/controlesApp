using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleApp.Model
{
    public class Tipo
    {
        public int Id { get; set; }
        public string TarefaTipo { get; set; }

        public override string ToString()
        {
            return TarefaTipo;
            
        }
    }
}
