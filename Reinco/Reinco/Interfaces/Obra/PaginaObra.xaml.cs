﻿using Newtonsoft.Json;
using Reinco.Gestores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaObra : ContentPage
    {
        HttpClient cliente = new HttpClient();

        public ObservableCollection<ObraItem> obraItem { get; set; }
        public PaginaObra()
        {
            InitializeComponent();
            obraItem = new ObservableCollection<ObraItem>();
            CargarObraItem();
            obrasListView.ItemsSource = obraItem;

            agregarObra.Clicked += AgregarObra_Clicked;
        }
        #region==================cargar obras======================================
        public async void CargarObraItem()
        {
            try
            {
                var client = new HttpClient();
                HttpResponseMessage result = await cliente.GetAsync("http://192.168.1.37:80/ServicioObra.asmx/MostrarObras");
                //recoge los datos json y los almacena en la variable resultado
                var resultado = await result.Content.ReadAsStringAsync();
                //si todo es correcto, muestra la pagina que el usuario debe ver
                dynamic array = JsonConvert.DeserializeObject(resultado);

                foreach (var item in array)
                {
                    obraItem.Add(new ObraItem
                    {
                        idObra = item.idObra,
                        nombre = item.nombre,
                        codigo = item.codigo,
                    });
                }

            }
            catch (Exception ex)
            {

                await DisplayAlert("Error", ex.Message, "Aceptar");
            }
        }
        #endregion
        // ===================// Navegar A la página AgregarObra.xaml //====================//
        private void AgregarObra_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AgregarObra());
        }

        #region=======================eliminar obra====================================
        public async void eliminar(object sender, EventArgs e)
        {
            var idObra = ((MenuItem)sender).CommandParameter;
            int IdObra = Convert.ToInt16(idObra);
           bool respuesta= await DisplayAlert("Eliminar", "Eliminar idObra = " + idObra, "Aceptar","Cancelar");
            using (var cliente = new HttpClient())
            {
                var result = await cliente.GetAsync("http://192.168.1.37:8080/ServicioObra.asmx/EliminarObra?idObra="+ IdObra);
                var json = await result.Content.ReadAsStringAsync();
                string mensaje = Convert.ToString(json);

                if (result.IsSuccessStatusCode)
                {
                    await App.Current.MainPage.DisplayAlert("Eliminar obra", mensaje, "OK");
                    return;
                }
            }
        }
        #endregion
        #region ===================// Modificar Obra CRUD //====================
        public void actualizar(object sender, EventArgs e)
        {
            var idObra = ((MenuItem)sender).CommandParameter;
            Navigation.PushAsync(new AgregarObra(idObra));
        }
        #endregion
    }
}
