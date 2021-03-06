using Newtonsoft.Json;
using Reinco.Recursos;
using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Plantilla
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPlantilla : ContentPage
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        public ObservableCollection<PlantillaLista> plantillaLista { get; set; }

        public PaginaPlantilla()
        {
            InitializeComponent();
            agregarPlantilla.Clicked += AgregarPlantilla_Clicked;
            plantillaLista = new ObservableCollection<PlantillaLista>();
            CargarPlantillaLista();
            plantillaListView.ItemsSource = plantillaLista;
            
        }
        #region========================cargar plantilla en lista====================================
        private async void CargarPlantillaLista()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioPlantilla.asmx", "MostrarPlantillas");
                foreach (var item in result)
                {
                    plantillaLista.Add(new PlantillaLista
                    {
                        idPlantilla = item.idPlantilla,
                        codigo = item.codigo,
                        nombre = item.nombre,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }

        }
        #endregion
        private void AgregarPlantilla_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPlantilla());
        }

        #region===================// Eliminar Plantilla CRUD //====================
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idPlantilla = ((MenuItem)sender).CommandParameter;
                int IdPlantilla = Convert.ToInt16(idPlantilla);
                bool respuesta = await DisplayAlert("Eliminar", "�Desea eliminar esta plantilla? ", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPlantilla", IdPlantilla } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantilla.asmx", "EliminarPlantilla", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Plantilla", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Plantilla", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
        }
        #endregion
        // ===================// Modificar Plantilla CRUD //====================// actualizar
        public void actualizar(object sender, EventArgs e)
        {
            var idPlantilla = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarPlantilla(idPlantilla));
        }

        // ===================// Modificar Plantilla CRUD //====================// actividades
        public void actividades(object sender, EventArgs e)
        {
            var idPlantilla = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new PaginaActividad(idPlantilla));
        }
        // END ==
    }
}
