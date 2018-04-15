using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControleApp.Webservice;
using System.Threading.Tasks;
using ControleApp.Model;

namespace ControleApp.Business
{
    public class TarefasRN
    {
        public async static Task<List<Tarefas>> GetTarefas(string tipo, string codUsuario)
        {
            try
            {
                return await TarefasWS.GetTarefas(tipo, codUsuario);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<string> Cadastrar(Tarefas t)
        {
            try
            {
                return await TarefasWS.Cadastrar(t);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<string> IncluirAnotacao(Tarefas t)
        {
            try
            {
                return await TarefasWS.IncluirAnotacao(t);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<List<Tipo>> GetTipo()
        {
            try
            {
                return await TarefasWS.GetTipo();
            }
            catch (Exception e)
            {
                
                throw;
            }
        }
    }

    
}
