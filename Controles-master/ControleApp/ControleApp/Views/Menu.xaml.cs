using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ControleApp.Model;
using ControleApp.Business;
using ControleApp.DAO;
using ControleApp.Util;
using ItemTappedEventArgs = Syncfusion.ListView.XForms.ItemTappedEventArgs;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        private List<Item> lista = new List<Item>();
        private List<Item> lista2 = new List<Item>();
        public Menu()
        {
            InitializeComponent();
            Session.RetornaMenu = false;
            Session.MinhaDelegada = "Minha";
            NavigationPage.SetHasBackButton(this, false);

            //GenerateItems();
            //ItemsListView.ItemsSource = lista;

        }

        protected override async void OnAppearing()
        {
            try
            {
                var tarefas = await TarefasRN.GetTarefas("Eresumo", Session.Usuario.Usw_cod.ToString());
                GenerateItems(tarefas[0]);
            }
            catch (Exception e)
            {
                
                throw;
            }
            
            //ItemsListView.ItemsSource = lista;
            //ItemsListView2.ItemsSource = lista2;

        }

        private void GenerateItems(Tarefas t)
        {
            GrdNaoLidas.BindingContext = new Item()
            {
                Nome = $"Não Lidas ({t.NLTotal})",
                Image = "icone_naolidas.png"
                };
            GrdAtraso.BindingContext = new Item()
            {
                Nome = $"Atrasos ({t.AtrTotal})",
                Image = "icone_atrasos_vermelho.png"
            };
            GrdFazerHoje.BindingContext = new Item()
            {
                Nome = $"Fazer Hoje ({t.FhjTotal})",
                Image = "icone_fazerhoje_laranja.png"
            };
            GrdValidar.BindingContext = new Item() {Nome = $"Validar ({t.VldTotal})", Image = "icone_validar_azul.png"};
            GrdAnotacoes.BindingContext = new Item()
            {
                Nome = $"Anotações ({t.AnotTotal})",
                Image = "icone_anotacoes_verde.png"
            };
            GrdFuturo.BindingContext = new Item() {Nome = $"Futuro ()", Image = "icone_futuro.png"};

            lista.Add(new Item() { Nome = $"Não Lidas ({t.NLTotal})", Image = "icone_naolidas.png" });
            lista.Add(new Item() { Nome = $"Atrasos ({t.AtrTotal})", Image = "icone_atrasos_vermelho.png" });
            lista.Add(new Item() { Nome = $"Fazer Hoje ({t.FhjTotal})", Image = "icone_fazerhoje_laranja.png" });
            lista.Add(new Item() { Nome = $"Validar ({t.VldTotal})", Image = "icone_validar_azul.png" });
            lista.Add(new Item() { Nome = $"Anotações ({t.AnotTotal})", Image = "icone_anotacoes_verde.png" });
            lista.Add(new Item() { Nome = $"Futuro ()", Image = "icone_futuro.png" });

            lista2.Add(new Item() { Nome = $"Nova Tarefa", Image = "Icone_NovaTarefa.png" });
            lista2.Add(new Item() { Nome = $"Adicionar Arquivos", Image = "icone_arquivos_marron.png" });
        }

        private void ItemsListView_SelectionChanged(object sender, Syncfusion.ListView.XForms.ItemSelectionChangedEventArgs e)
        {
            //if (ItemsListView2.SelectedItem != null)
            //{
            //    if (((Item) ItemsListView2.SelectedItem).Nome.Contains("Nova Tarefa"))
            //    {
            //        Session.Navigation.Navigation.PushAsync(new NovaTarefa());
            //    }
            //}
        }

        private void ItemsListView2_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            //var item = (Item) e.ItemData;
            //if (item.Nome.Contains("Nova Tarefa"))
            //{
            //    Session.Navigation.Navigation.PushAsync(new NovaTarefa());
            //}
        }

        private void TapNaoLidas(object sender, EventArgs e)
        {
            Session.UltOpMenu = "Não Lidas";
            Session.UltOpMenu1 = "NaoLidas";

            Session.Navigation.Navigation.PushAsync(new Listar("Não Lidas", "NaoLidas"));
        }

        private void TapAtrasos(object sender, EventArgs e)
        {
            Session.UltOpMenu = "Atrasos";
            Session.UltOpMenu1 = "Atraso";
            Session.Navigation.Navigation.PushAsync(new Listar("Atrasos", "Atraso"));
        }

        private void TapNovaTarefa(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new NovaTarefa());
        }

        private void Sair_clicked(object sender, EventArgs e)
        {
            UsuarioDAO.Remover();
            Session.Navigation.Navigation.PushAsync(new Login());
            Session.Navigation.Navigation.RemovePage(this);
        }

        private void FazerHoje_Tapped(object sender, EventArgs e)
        {
            Session.UltOpMenu = "Fazer Hoje";
            Session.UltOpMenu1 = "FazerHoje";

            Session.Navigation.Navigation.PushAsync(new Listar("Fazer Hoje", "FazerHoje"));
        }

        private void TapValidar(object sender, EventArgs e)
        {
            Session.UltOpMenu = "Validar";
            Session.UltOpMenu1 = "Validar";

            Session.Navigation.Navigation.PushAsync(new Listar("Validar", "Validar"));
        }

        private void AnotacaoTap(object sender, EventArgs e)
        {
            Session.UltOpMenu = "Anotações";
            Session.UltOpMenu1 = "Anotar";

            Session.Navigation.Navigation.PushAsync(new Listar("Anotações", "Anotar"));
        }

        private void FuturoTap(object sender, EventArgs e)
        {
            
        }
    }
}