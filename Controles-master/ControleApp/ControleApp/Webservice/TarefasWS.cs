using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Model;
using ControleApp.Web;

namespace ControleApp.Webservice
{
    public class TarefasWS
    {

        public async static Task<string> Cadastrar(Tarefas t)
        {
            try
            {
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Tarefas";
                var req = new Request(url);
                
                return await req.Post<string>(t);
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
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Tarefas";
                var req = new Request(url);

                return await req.Post<string>(t);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public async static Task<string> Update(Tarefas t)
        {
            try
            {
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Tarefas";
                var req = new Request(url);

                return await req.Post<string>(t);
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
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/TarefasTipos";
                var req = new Request(url);
                return await req.Get<List<Tipo>>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async static Task<List<Tarefas>> GetTarefas(string tipo, string codUsuario)
        {
            try
            {
                //string cnpj, string cpf, DateTime? mes, DateTime? inicioPeriodo, DateTime? fimPeriodo, int convenioId, int //hospitalId, string paciente, int? situacaoId

                string url = "http://vm01.bulgart.com:5000/Api/Tarefas?tipo=" + tipo + "&usuario=" + codUsuario + "&senha=123";
                var req = new Request(url);
                return await req.Get<List<Tarefas>>();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
