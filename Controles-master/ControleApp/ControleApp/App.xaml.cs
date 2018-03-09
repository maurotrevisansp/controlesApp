using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ControleApp.DAO;
using ControleApp.Util;
using Xamarin.Forms;

namespace ControleApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (UsuarioDAO.Obter() != null)
            {
                Session.Usuario = UsuarioDAO.Obter();
                Session.Navigation = new NavigationPage(new ControleApp.Views.Menu());
            }
            else
            {
                Session.Navigation = new NavigationPage(new ControleApp.Views.Login());
            }
            
            MainPage = Session.Navigation;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
