using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Business;
using ControleApp.Model;
using ControleApp.Util;
using Xamarin.Forms;
using Newtonsoft.Json;
using Xamarin.Forms.Xaml;
using Syncfusion.SfDataGrid.XForms;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovaTarefa : ContentPage
    {
        private Tarefas tarefa;
        private List<TarefasAnot> minhasAnot = new List<TarefasAnot>();

        public NovaTarefa(Tarefas t = null)
        {
            InitializeComponent();
            if (t != null)
            {
                tarefa = t;
            }
            TxtHoraFim.Text = DateTime.Now.ToString("HH:mm");
            NavigationPage.SetHasBackButton(this, false);
            
        }

        private List<Usuario> usuarios;
        protected  override async void OnAppearing()
        {

            usuarios = await UsuarioRN.GetUsuarios(Session.Usuario.Usw_cod.ToString(), "123", Session.Usuario.Perfil);
            PckCliente.ItemsSource = await ClienteRN.GetClientes();
            PckPara.ItemsSource = usuarios;
            var tipos = await TarefasRN.GetTipo();
            PckTipo.ItemsSource = tipos;
            PckTipo.SelectedItem = tipos.Where(t => t.TarefaTipo.ToUpper().Contains("AGENDA")).FirstOrDefault();
            List<String> acoes = new List<string>();
            acoes.Add("      Incluir ");
            acoes.Add("Ler / Aceitar ");
            acoes.Add("       Baixar ");
            acoes.Add("      Validar ");
            PckAcao.ItemsSource = acoes;
            PckAcao.SelectedIndex = 0;
            if (tarefa != null)
            {
                PckCliente.SelectedItem = ((List<Cliente>)PckCliente.ItemsSource).FirstOrDefault(c => c.Id == tarefa.CLIENTE);
                TxtDataFim.Date = tarefa.DATA_PROGR;
                PckPara.SelectedItem = usuarios.FirstOrDefault(u => u.Usw_cod == tarefa.RESPOSAVEL);

                PckTipo.SelectedItem = tipos.FirstOrDefault(t => t.TarefaTipo.Contains("Tarefa"));
                TxtTexto.Text = tarefa.HISTORICO;
                PckAcao.SelectedIndex = tarefa.Pgr_Fase;
                //TxtTexto.IsVisible = false;
                //ScrollEditor.ScrollToAsync(0, 0, false);
                if (tarefa.tarefasAnot != null)
                {
                    var listavm = new List<ListaVM>();
                    minhasAnot = tarefa.tarefasAnot;
                    if (minhasAnot.Count > 0)
                    {
                        TxtTexto.Text += "\r\n\r\n Possui Anotações ";
                        TxtAnot.Text = "Anotações";
                        TxtAnot.IsVisible = true;
                    }
                    else
                    {
                        TxtAnot.IsVisible = false;
                    }
                    foreach (var l in minhasAnot)
                    {
                        var itemm = new ListaVM() { Anot_DataAnot = l.Anot_DataAnot.ToString("dd/MM/yyyy"), Anot_histor = l.Anot_histor };
                        listavm.Add(itemm);
                        //TxtTexto.Text += "\r\n\r\n " + l.Anot_DataAnot.ToString("dd/MM/yyyy") + " - " + l.Anot_histor;
                    }

                    ListaAnot.ItemsSource = listavm;

                }

            }
        }

        private void Menu_clicked(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new Menu());
        }

        private void Lista_clicked(object sender, EventArgs e)
        {
            if (Session.UltOpMenu == string.Empty || Session.UltOpMenu == null)
            {
                Session.UltOpMenu = "Fazer Hoje";
                Session.UltOpMenu1 = "FazerHoje";
            }
            Session.Navigation.Navigation.PushAsync(new Listar(Session.UltOpMenu, Session.UltOpMenu1));
        }

        private void Enviar_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Erro", "Por favor preencha os campos necessários.", "OK");
        }

        private async void Enviar(object sender, EventArgs e)
        {
            try
            {
                StckSucesso.IsVisible = false;
                if (PckPara.SelectedIndex == -1 ||
                    PckTipo.SelectedIndex == -1)
                {
                    DisplayAlert("Erro", "Preencha todos os campos", "Ok");
                    return;
                }
                var t = new Tarefas();
                if (PckCliente.SelectedIndex == -1)
                {
                    t.CLIENTE = 0;
                }
                else
                {
                    t.CLIENTE = ((Cliente)PckCliente.SelectedItem).Id;
                }
                //t.CLIENTE = (PckCliente.SelectedIndex == -1) ? Convert.ToInt32(TxtCliente.Text) : ((Cliente)PckCliente.SelectedItem).Id;
                t.DATA_PROGR = TxtDataFim.Date;
                t.SOLICITANTE = Session.Usuario.Usw_cod;
                t.RESPOSAVEL = ((Usuario)PckPara.SelectedItem).Usw_cod;
                t.HISTORICO = TxtTexto.Text;
                t.Prg_NatProg = ((Tipo)PckTipo.SelectedItem).Id;
                t.Pgr_Fase = 1;

                if (tarefa != null)
                {
                    t.CodPro = tarefa.CodPro;
                    
                    if (PckAcao.SelectedIndex == 1)
                    {
                        t.Pgr_Fase = 2;
                        t.Pgr_LidaPor = Session.Usuario.Usw_cod;
                    }
                    if (PckAcao.SelectedIndex == 2)
                    {
                        t.Pgr_Fase = 3;
                        t.Pgr_LidaPor = Session.Usuario.Usw_cod;
                    }
                    if (PckAcao.SelectedIndex == 3)
                    {
                        t.Pgr_Fase = 4;
                        t.Pgr_LidaPor = Session.Usuario.Usw_cod;
                    }
                }
                else
                {
                    t.CodPro = 0;
                }
                var retorno = await TarefasRN.Cadastrar(t);
                if (String.IsNullOrEmpty(retorno))
                {
                    StckSucesso.IsVisible = true;
                    if (t.Pgr_Fase == 1)
                    {
                        TxtSucesso.Text = "Tarefa Incluida.";
                    }
                    if (t.Pgr_Fase == 2)
                    {
                        TxtSucesso.Text = "Tarefa Lida e Aceita.";
                    }
                    if (t.Pgr_Fase == 3)
                    {
                        TxtSucesso.Text = "Tarefa Baixada.";
                    }
                    if (t.Pgr_Fase == 4)
                    {
                        TxtSucesso.Text = "Tarefa Validada.";
                    }
                    if (t.Pgr_Fase == 3 || t.Pgr_Fase == 4)
                    {
                        await Session.Navigation.Navigation.PushAsync(new Listar(Session.UltOpMenu, Session.UltOpMenu1));
                        Session.RetornaMenu = true;
                        //await Session.Navigation.Navigation.PushAsync(new Menu());
                    }
                    else
                    {
                        PckAcao.SelectedIndex = t.Pgr_Fase;
                    }
                }

            }
            catch (Exception exception)
            {
                
                throw exception;
            }
            
        }

        private void textTap(object sender, EventArgs e)
        {
            TxtTexto.Focus();
            if (TxtTexto.Text == "Descrição")
            {
                TxtTexto.Text = "";
            }
        }

        private void BtnApagar_OnClicked(object sender, EventArgs e)
        {
           
            TxtTexto.Text = "";
            StckSucesso.IsVisible = false;
        }

        private void TxtCliente_OnTextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void Eu_Clicked(object sender, EventArgs e)
        {
            PckPara.SelectedItem = usuarios.Where(s => s.Usw_cod == Session.Usuario.Usw_cod).FirstOrDefault();
        }

        private void PickerLabel_OnTapped(object sender, EventArgs e)
        {
            PckPara.Focus();
        }

        private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabel.Text = PckPara.Items[PckPara.SelectedIndex];
        }

        private void PickerLabelTipo_OnTapped(object sender, EventArgs e)
        {
            PckTipo.Focus();
        }

        private void PickerListTipo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabelTipo.Text = PckTipo.Items[PckTipo.SelectedIndex];
        }

        private void PickerLabelCliente_OnTapped(object sender, EventArgs e)
        {
            PckCliente.Focus();
        }

        private void PickerListCliente_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabelCliente.Text = PckCliente.Items[PckCliente.SelectedIndex];
        }

        private void PickerLabelAcao_OnTapped(object sender, EventArgs e)
        {
            PckAcao.Focus();
        }

        private void PickerListAcao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabelAcao.Text = PckAcao.Items[PckAcao.SelectedIndex];
        }

        public class ListaVM
        {
            public string Anot_DataAnot { get; set; }
            public string Anot_histor { get; set; }

        }

        private void MostrarAnotacoes(object sender, EventArgs e)
        {
            //Filtros.IsVisible = !Filtros.IsVisible;
            //var listavm = new List<ListaVM>();
            //foreach (var l in minhasAnot)
            //{
            //    var itemm = new ListaVM() { Anot_DataAnot = l.Anot_DataAnot.ToString("dd/MM/yyyy"), Anot_histor = l.Anot_histor };
            //    listavm.Add(itemm);
            //}
            //ListaAnot.ItemsSource = listavm;
            //ListaAnot.IsVisible = true;

        }
        private void Lista_OnGridTapped(object sender, GridTappedEventsArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new NovaTarefa((Tarefas)e.RowData));
        }
        private void BtnAnotar_OnClicked(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new Anotacao(tarefa));
        }




    }
}