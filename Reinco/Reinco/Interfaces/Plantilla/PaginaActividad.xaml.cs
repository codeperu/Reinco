﻿using Newtonsoft.Json;
using Reinco.Gestores;
using Reinco.Recursos;
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
    public partial class PaginaActividad : ContentPage
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        int IdPlantilla;
        public ObservableCollection<ActividadItems> actividadItems { get; set; }
        public PaginaActividad(object idPlantilla)
        {
            InitializeComponent();
            IdPlantilla = Convert.ToInt16(idPlantilla);
            actividadItems = new ObservableCollection<ActividadItems>();
            actividadListView.ItemsSource = actividadItems;
            CargarActividadItems();
            // eventos
            agregarActividad.Clicked += AgregarActividad_Clicked;
        }

        private void AgregarActividad_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarActividad(IdPlantilla));
        }

        private async void CargarActividadItems()
        {
            byte x = 01; // utilizada para la enumeracion de las actividades
            try
            {
                object[,] OidPlantilla = new object[,] { { "idPlantilla", IdPlantilla } };
                dynamic result = await Servicio.MetodoGet("ServicioPlantillaActividad.asmx", "MostrarActividadxIdPlantilla", OidPlantilla);
                foreach (var item in result)
                {
                    actividadItems.Add(new ActividadItems
                    {
                        idActividad = item.idPlantilla_Actividad,
                        nombre = item.nombre,
                        tolerancia = item.tolerancia,
                        enumera = x++,
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        // ===================// Eliminar Plantilla CRUD //====================// eliminar
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idActividad = ((MenuItem)sender).CommandParameter;
                int IdActividad = Convert.ToInt16(idActividad);
                bool respuesta = await DisplayAlert("Eliminar", "¿Desea eliminar este Actividad?", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPlantillaActividad", IdActividad } };
                dynamic result = await Servicio.MetodoGetString("ServicioPlantillaActividad.asmx", "EliminarPlantillaActividad", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Actividad", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Actividad", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }

        }

        // ===================// Modificar Plantilla CRUD //====================// actualizar
        public void actualizar(object sender, EventArgs e)
        {
            var idActividad = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarActividad(idActividad,IdPlantilla));
        }
    }
}
