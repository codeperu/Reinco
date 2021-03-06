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

namespace Reinco.Interfaces.Propietario
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListarPropietario : ContentPage
    {
        WebService Servicio = new WebService();
        public VentanaMensaje mensaje;
        string Mensaje;
        public ObservableCollection<PropietarioItem> propietarioItem { get; set; }
       // datatable usuario;
        public ListarPropietario()
        {
            InitializeComponent();
            propietarioItem = new ObservableCollection<PropietarioItem>();
            CargarPropietarioItem();
            propietarioListView.ItemsSource = propietarioItem;
            agregarPropietario.Clicked += AgregarPropietario_Clicked;
            
        }
        #region=============cargar propietarios==================
        private async void CargarPropietarioItem()
        {
            try
            {
                dynamic result = await Servicio.MetodoGet("ServicioPropietario.asmx", "MostrarPropietarios");
                foreach (var item in result)
                {
                    propietarioItem.Add(new PropietarioItem
                    {
                        idPropietario = item.idPropietario,
                        nombre = item.nombre,
                        fotoPerfil = "icon.png",
                    });
                }

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
            
        }
        #endregion


        private void AgregarPropietario_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarPropietario());
        }


        // ===================// Eliminar Plantilla CRUD //====================// eliminar
        public async void eliminar(object sender, EventArgs e)
        {
            try
            {
                var idPropietario = ((MenuItem)sender).CommandParameter;
                int IdPropietario = Convert.ToInt16(idPropietario);
                bool respuesta = await DisplayAlert("Eliminar", "¿Desea eliminar al propietario?", "Aceptar", "Cancelar");
                object[,] variables = new object[,] { { "idPropietario", IdPropietario} };
                dynamic result = await Servicio.MetodoGetString("ServicioPropietario.asmx", "EliminarPropietario", variables);
                Mensaje = Convert.ToString(result);
                if (result != null)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar Usuario", Mensaje, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                await mensaje.MostrarMensaje("Eliminar Propietario", "Error en el dispositivo o URL incorrecto: " + ex.ToString());
            }
           
        }

        // ===================// Modificar Plantilla CRUD //====================// actualizar
        public void actualizar(object sender, EventArgs e)
        {
            var idPropietario = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarPropietario(idPropietario));
        }

    }
}
