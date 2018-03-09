using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleApp.Model
{
    public class Usuario
    {

        public int Usw_cod { get; set; }
        public string Usw_Usuario { get; set; }
        public string Usw_Nome { get; set; }
        public int Usw_Status { get; set; }
        public string Usw_Senha { get; set; }
        public string Perfil { get; set; }

        public override string ToString()
        {
            return Usw_Nome;
        }
    }

    public class Item
    {
        public string Image { get; set; }
        public string Nome { get; set; }
    }

    
}
