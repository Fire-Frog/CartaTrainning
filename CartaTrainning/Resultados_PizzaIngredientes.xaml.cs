using CartaTrainning.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CartaTrainning
{
    /// <summary>
    /// Lógica de interacción para Resultados_PizzaIngredientes.xaml
    /// </summary>
    public partial class Resultados_PizzaIngredientes : Window
    {
        decimal respCorrectas = 0;
        decimal totalPreguntas = 0;
        decimal porcCorrectas = 0;
        TimeSpan totalTiempoUsuario;
        List<RespuestasPizzas> respuestasPizzas;
        List<Ingrediente> totalIngredientes;

        public Resultados_PizzaIngredientes(List<RespuestasPizzas> respPizzas, List<Ingrediente> todosIngredientes, TimeSpan tiempoTotal)
        {
            InitializeComponent(); //initialize window

            respuestasPizzas = new List<RespuestasPizzas>(respPizzas);
            totalIngredientes = new List<Ingrediente>(todosIngredientes);
            totalTiempoUsuario = tiempoTotal;
        }

        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblRespuestasCorrectas.Content = respCorrectas.ToString();

            respCorrectas = respuestasPizzas.Count(x => x.EsRespuestaCorrecta);
            totalPreguntas = respuestasPizzas.Last().NumeroRespuesta + 1;
            porcCorrectas = respCorrectas / totalPreguntas;

            lblTotalPreguntas.Content = totalPreguntas.ToString();
            lblCorrectasPorcentaje.Content = porcCorrectas.ToString("P1");
            lblTotalTiempo.Content = totalTiempoUsuario.ToString(@"hh\:mm\:ss");

            lbPizzas.ItemsSource = respuestasPizzas;
            lbIngredientesCorrectos.DisplayMemberPath = "nombre";
            lbIngredientesUsuario.DisplayMemberPath = "nombre";
        }

        private void lbPizzas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pizzaSeleccionada = lbPizzas.SelectedItem as RespuestasPizzas;

            lbIngredientesUsuario.Items.Clear();
            lbIngredientesCorrectos.Items.Clear();

            foreach (var item in pizzaSeleccionada.ingredientes)
            {
                var ingrBsq = totalIngredientes.Single(x => x.id == item);
                lbIngredientesCorrectos.Items.Add(ingrBsq);
            }

            foreach (var item in pizzaSeleccionada.IngredientesUsuario)
            {
                var ingrBsq = totalIngredientes.Single(x => x.id == item);
                lbIngredientesUsuario.Items.Add(ingrBsq);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Q))
            {
                Close();
            }
        }
    }
}
