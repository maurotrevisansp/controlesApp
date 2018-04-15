using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ControleApp.Business;
using ControleApp.Model;
using ControleApp.Util;
using Newtonsoft.Json;
using Syncfusion.SfDataGrid.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ControleApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Listar : ContentPage
	{
	    private string _tipo;
        private List<Tarefas> tarefas = new List<Tarefas>();
        private List<Tarefas> minhas = new List<Tarefas>();
        private List<Tarefas> delegadas = new List<Tarefas>();

        public Listar (string titulo,string tipo)
		{
		    try
		    {
                InitializeComponent();
                NavigationPage.SetHasBackButton(this, false);
                Title = titulo;
                _tipo = tipo;
		        var como = new List<String>();
                como.Add("Minhas");
                como.Add("Atribuídas");
		        PckComo.ItemsSource = como;
		        PckComo.SelectedItem = "Minhas";
		        PckTIpo.ItemsSource = new List<string>() {"Tarefas"};
		        PckTIpo.SelectedItem = "Tarefas";

            }
		    catch (Exception e)
		    {
		        
		        throw;
		    }
			
		}

        private void DataGrid_QueryRowHeight(object sender, QueryRowHeightEventArgs e)
        {
            GridRowSizingOptions options = new GridRowSizingOptions();
            options.ExcludeColumns.Add("DATA_PROGR");
            if (e.RowIndex == 0)
            {
                e.Height = 30;
            }
            else
            {
                e.Height = Lista.GetRowHeight(e.RowIndex, options);
            }
            e.Handled = true;
        }

        protected override async void OnAppearing()
	    {
	        try
	        {
                base.OnAppearing();
                Session.RetornaMenu = false;
                
                PckDe.NullableDate = null;
	            PckA.NullableDate = null;
                tarefas = await TarefasRN.GetTarefas(_tipo, Session.Usuario.Usw_cod.ToString());
	            minhas = tarefas.Where(t => t.RESPOSAVEL == Session.Usuario.Usw_cod).ToList();
                delegadas = tarefas.Where(t => t.RESPOSAVEL != Session.Usuario.Usw_cod).ToList();
	            TxtMinhas.Text = $"{minhas.Count}";
	            TxtDelegadas.Text = $"{delegadas.Count}";
                if (Session.MinhaDelegada == "Minha")
                {
                    Lista.ItemsSource = minhas;
                }
                else
                {
                    Lista.ItemsSource = delegadas;
                }


            }
            catch (Exception e)
	        {
	            
	            throw;
	        }
	        
	    }

        public class ListaVM
        {
            public string Data { get; set; }
            public string Descrição { get; set; }
            public string Fase { get; set; }
            
	    }

        private void Menu_clicked(object sender, EventArgs e)
        {
            Session.Navigation.Navigation.PushAsync(new Menu());
        }


        private async void Filtrar_clicked(object sender, EventArgs e)
	    {
	        try
	        {
                var itens = await TarefasRN.GetTarefas(_tipo, Session.Usuario.Usw_cod.ToString());
                var lista = new List<Tarefas>();
                if (((string)PckComo.SelectedItem) == "Minhas")
                {
                    lista = itens.Where(i => i.RESPOSAVEL == Session.Usuario.Usw_cod).ToList();
                }
                else
                {
                    lista = itens.Where(i => i.SOLICITANTE == Session.Usuario.Usw_cod).ToList();
                }

                if (PckDe.NullableDate.HasValue)
                {
                    lista = lista.Where(i => i.DATA_PROGR >= PckDe.NullableDate.Value).ToList();
                }
                if (PckA.NullableDate.HasValue)
                {
                    lista = lista.Where(i => i.DATA_PROGR <= PckA.NullableDate.Value).ToList();
                }

                var listavm = new List<ListaVM>();
	            foreach (var l in itens)
	            {
	                var itemm = new ListaVM() {Data = l.DATA_PROGR.ToString("dd/MM/yyyy"), Descrição = l.HISTORICO, Fase = l.Pgr_Fase.ToString() };
                    listavm.Add(itemm);
	            }
                Lista.ItemsSource = listavm;
	            //Lista.Columns[0].Width = 150;
	        }
	        catch (Exception ex)
	        {
	            

	            throw ex;
	        }
            
        }

	    private void FiltrarMinhas(object sender, EventArgs e)
	    {
	        try
	        {
                StackList.Children.Clear();
                var listavm = new List<ListaVM>();
                foreach (var l in minhas)
                {
                    //listavm.Add(new ListaVM() { Data = l.DATA_PROGR.ToString("dd/MM/yyyy"), Descrição = l.HISTORICO });
                    var itemm = new ListaVM() { Data = l.DATA_PROGR.ToString("dd/MM/yyyy"), Descrição = l.HISTORICO, Fase = l.Pgr_Fase.ToString() };
                    listavm.Add(itemm);

                    var item = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        BackgroundColor = Color.Black,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Spacing = 0,
                        Padding = 1
                    };
                    var data = new StackLayout()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        WidthRequest = 95,
                        Margin = new Thickness(0, 0, 0, 0),
                        BackgroundColor = Color.White
                    };
                    item.Children.Add(data);

                    var lbldata = new Label()
                    {
                        Text = l.DATA_PROGR.ToString("dd/MM/yyyy"),
                        FontSize = 14,
                        WidthRequest = 95,
                        LineBreakMode = LineBreakMode.NoWrap,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = 4
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, ee) => {
                        Session.Navigation.Navigation.PushAsync(new NovaTarefa(l));
                    };
                    lbldata.GestureRecognizers.Add(tapGestureRecognizer);
                    data.Children.Add(lbldata);
                    var descricao = new StackLayout()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(1, 0, 0, 0),
                        BackgroundColor = Color.White
                    };
                    var lblDesc = new Label()
                    {
                        Text = l.HISTORICO,

                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        Margin = 4
                    };
                    item.Children.Add(descricao);
                    descricao.Children.Add(lblDesc);
                    StackList.Children.Add(item);
                    Lista.ItemsSource = minhas;
                    Session.MinhaDelegada = "Minha";
                    

                }
                //Lista = new SfDataGrid(){ HorizontalOptions = LayoutOptions.FillAndExpand, ColumnSizer = ColumnSizer.LastColumnFill, AutoGenerateColumnsMode = AutoGenerateColumnsMode.ResetAll };
                //Lista.ItemsSource = listavm;
                //Lista.Columns[0].Width = 150;
            }
	        catch (Exception exception)
	        {
	            
	            throw exception;
	        }
	        
            
	    }

	    private void FiltrarDelegadas(object sender, EventArgs e)
	    {
	        try
	        {
                StackList.Children.Clear();
                var listavm = new List<ListaVM>();
                foreach (var l in delegadas)
                {
                    listavm.Add(new ListaVM() { Data = l.DATA_PROGR.ToString("dd/MM/yyyy"), Descrição = l.HISTORICO, Fase = l.Pgr_Fase.ToString() });
                    var item = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        BackgroundColor = Color.Black,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = 1,
                        Spacing = 0
                    };
                    var data = new StackLayout()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        WidthRequest = 95,
                        Margin = new Thickness(0,0, 0, 0),
                        BackgroundColor = Color.White
                    };
                    item.Children.Add(data);

                    var lbldata = new Label()
                    {
                        Text = l.DATA_PROGR.ToString("dd/MM/yyyy"),
                        WidthRequest = 80,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                        Margin = 4
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, ee) => {
                        Session.Navigation.Navigation.PushAsync(new NovaTarefa(l));
                    };
                    lbldata.GestureRecognizers.Add(tapGestureRecognizer);
                    data.Children.Add(lbldata);
                    var descricao = new StackLayout()
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Margin = new Thickness(1, 0, 0, 0),
                        BackgroundColor = Color.White,

                    };
                    var lblDesc = new Label()
                    {
                        Text = l.HISTORICO,
                        Margin = 4,
                        VerticalOptions = LayoutOptions.Center,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        HorizontalTextAlignment = TextAlignment.Center,
                        FontSize = 14,
                    };
                    item.Children.Add(descricao);
                    descricao.Children.Add(lblDesc);
                    StackList.Children.Add(item);
                }
                Lista.ItemsSource = delegadas;
                Session.MinhaDelegada = "Delegada";

                //Lista.Columns[0].Width = 150;
            }
	        catch (Exception exception)
	        {
	            
	            throw exception;
	        }
            

        }

	    private void MostrarFiltro(object sender, EventArgs e)
	    {
	        Filtros.IsVisible = !Filtros.IsVisible;
	    }

	    private void TapItem(object sender, EventArgs e)
	    {
	        Session.Navigation.Navigation.PushAsync(new NovaTarefa());
	    }

	    private void Lista_OnGridTapped(object sender, GridTappedEventsArgs e)
	    {
	        if (e.RowColumnindex.ColumnIndex == 0)
	        {
                Session.Navigation.Navigation.PushAsync(new NovaTarefa((Tarefas)e.RowData));
            }
	    }
	}
}