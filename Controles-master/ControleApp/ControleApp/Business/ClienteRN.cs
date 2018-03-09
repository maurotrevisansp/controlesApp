using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Model;
using ControleApp.Webservice;

namespace ControleApp.Business
{
    public class ClienteRN
    {
        public async static Task<List<Cliente>> GetClientes()
        {
            try
            {
                return await ClienteWS.GetClientes();
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
    }
}
