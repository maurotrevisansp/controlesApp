using ControleApp.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.DAO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            if (Session.Usuario != null)
            {
                usernameEntry.Text = Session.Usuario.Usw_Usuario;
            }
        }

        private async void OnLoginButtonClickedAsync(object sender, EventArgs e)
        {
            try
            {
                Act.IsVisible = true;
                var usuario = await Business.UsuarioRN.GetUsuario(usernameEntry.Text, passwordEntry.Text);
                if(usuario.Count == 1)
                {

                    Session.Navigation.Navigation.PushAsync(new Menu());
                    Session.Usuario = usuario[0];
                    UsuarioDAO.InserirUsuario(usuario[0]);
                    Session.Navigation.Navigation.RemovePage(this);
                }
                else
                {
                    DisplayAlert("Erro", "Usuário ou senha incorretos","Ok");
                }
            }
            catch (Exception ex)
            {

                Util.Excecao.TratarExcecao(ex,this);
            }
            finally
            {
                Act.IsVisible = false;
            }
            
        }
    }
}