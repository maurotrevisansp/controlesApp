using ControleApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Web;

namespace ControleApp.Webservice
{
    public class UsuarioWS
    {
        //public static async Task<List<Usuario>> GetUsuario(string Nome, string Senha)
        //{
        //    try
        //    {
        //        string url = "http://vm01.bulgart.com:5000/Api/Usuarios?usuario=" + Nome + "&senha=" + Senha;
        //        var response = await client.GetStringAsync(url);
        //        var UsuarioLogado = JsonConvert.DeserializeObject<List<Usuario>>(response);
        //        return UsuarioLogado;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        public async static Task<List<Usuario>> GetUsuario(string Nome, string Senha)
        {
            try
            {
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Usuarios?usuario=" + Nome + "&senha=" + Senha;
                var req = new Request(url);
                return await req.Get<List<Usuario>>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<List<Usuario>> GetUsuarios(string usuario, string senha, string perfil)
        {
            try
            {
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Usuarios?usuario=" + usuario + "&senha=123&perfil=" + perfil;
                var req = new Request(url);
                return await req.Get<List<Usuario>>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



    }
}
