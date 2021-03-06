﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Reinco.Interfaces.Obra
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaPrueva : ContentPage
    {
        public PaginaPrueva()
        {
            InitializeComponent();
        }
        public PaginaPrueva(string idObra, string nombre, string codigo)
        {
            DisplayAlert("Confirmacion", idObra + nombre + codigo, "Aceptar");
        }
    }
}
