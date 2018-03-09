using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Model;
using ControleApp.Web;

namespace ControleApp.Webservice
{
    public class ClienteWS
    {
        public async static Task<List<Cliente>> GetClientes()
        {
            try
            {
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Clientes";
                var req = new Request(url);
                return await req.Get<List<Cliente>>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
