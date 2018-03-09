using ControleApp.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ControleApp.Util
{
    public class Excecao
    {
        public static void TratarExcecao(Exception erro, Page page)
        {
            Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
            {
                if (erro.GetType() == typeof(WebRequestException))
                {
                    page.DisplayAlert("Erro",
                        "Não foi possível realizar requisição ao servidor. Verifique conexão com a rede.", "Ok");
                }
                else if (erro.GetType() == typeof(WebRequestResponseError))
                {
                    page.DisplayAlert("Erro",
                        "Resposta inesperada do servidor. Entre em contato com o suporte.", "Ok");
                }
                else
                {
                    page.DisplayAlert("Erro",
                        "Erro inesperado. Entre em contato com o suporte.", "Ok");
                }
            });

        }
    }
}
