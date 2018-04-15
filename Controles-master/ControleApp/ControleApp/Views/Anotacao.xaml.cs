using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Business;
using ControleApp.Model;
using ControleApp.Util;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Anotacao : ContentPage
    {
        private Tarefas tarefa;

        public Anotacao(Tarefas t = null)
        {
            InitializeComponent();
            if (t != null)
            {
                tarefa = t;
            }

            NavigationPage.SetHasBackButton(this, false);

        }
        private void Menu_clicked(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new Menu());
        }

        protected override void OnAppearing()
        {

            if (tarefa != null)
            {
                TxtDataFim.Date = tarefa.DATA_PROGR;
                if (tarefa.HISTORICO.Length > 45)
                {
                    TxtTarefaDesc.Text = tarefa.HISTORICO.Substring(0, 45) + " ...";

                }
                else
                {
                    TxtTarefaDesc.Text = tarefa.HISTORICO;

                }
                //PckAcao.SelectedIndex = tarefa.Pgr_Fase;
                //TxtTexto.IsVisible = false;
                //ScrollEditor.ScrollToAsync(0, 0, false);
            }
        }

        private async void Enviar(object sender, EventArgs e)
        {
            try
            {
                StckSucesso.IsVisible = false;
                var t = new Tarefas();
                //t.CLIENTE = (PckCliente.SelectedIndex == -1) ? Convert.ToInt32(TxtCliente.Text) : ((Cliente)PckCliente.SelectedItem).Id;
                t.DATA_PROGR = TxtDataFim.Date;
                t.SOLICITANTE = Session.Usuario.Usw_cod;
                t.RESPOSAVEL = tarefa.RESPOSAVEL;
                t.HISTORICO = Session.Usuario.Usw_Nome.Substring(0, Session.Usuario.Usw_Nome.IndexOf(" ") + 2) + ": " + TxtTexto.Text;
                t.CodPro = tarefa.CodPro;
                t.Pgr_Fase = 100;

                var retorno = await TarefasRN.IncluirAnotacao(t);
                if (String.IsNullOrEmpty(retorno))
                {
                    StckSucesso.IsVisible = true;
                    TxtSucesso.Text = "Anotação Incuida com Sucesso.";
                }

            }
            catch (Exception exception)
            {

                throw exception;
            }

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

        private void Eu_Clicked(object sender, EventArgs e)
        {
            //PckPara.SelectedItem = usuarios.Where(s => s.Usw_cod == Session.Usuario.Usw_cod).FirstOrDefault();
        }

        //private void PickerLabel_OnTapped(object sender, EventArgs e)
        //{
        //    PckPara.Focus();
        //}
        private void textTap(object sender, EventArgs e)
        {
            TxtTexto.Focus();
            if (TxtTexto.Text == "Descrição")
            {
                TxtTexto.Text = "";
            }
        }
        private void Enviar_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Erro", "Por favor preencha os campos necessários.", "OK");
        }
        private void PickerLabelAcao_OnTapped(object sender, EventArgs e)
        {
            PckAcao.Focus();
        }
        private void PickerListAcao_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            PickerLabelAcao.Text = PckAcao.Items[PckAcao.SelectedIndex];
        }

        private void BtnApagar_OnClicked(object sender, EventArgs e)
        {

            TxtTexto.Text = "";
            StckSucesso.IsVisible = false;
        }
        //private void PickerList_OnSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PickerLabel.Text = PckPara.Items[PckPara.SelectedIndex];
        //}
    }

}