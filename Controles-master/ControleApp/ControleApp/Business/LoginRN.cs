using ControleApp.Model;
using ControleApp.Webservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleApp.Business
{
    public class UsuarioRN
    {
        public static async Task<List<Usuario>> GetUsuario(string Nome, string Senha)
        {
            try
            {
                return await UsuarioWS.GetUsuario(Nome, Senha);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static async Task<List<Usuario>> GetUsuarios(string usuario, string senha, string perfil)
        {
            try
            {
                return await UsuarioWS.GetUsuarios(usuario, senha, perfil);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
