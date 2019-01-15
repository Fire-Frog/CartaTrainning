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
    /// Lógica de interacción para Resultados_ValoresIngredientes.xaml
    /// </summary>
    public partial class Resultados_ValoresIngredientes : Window
    {
        decimal respCorrectas = 0;
        decimal totalPreguntas = 0;
        decimal porcCorrectas = 0;
        TimeSpan totalTiempoUsuario;
        List<RespuestasIngredientes> respuestasIngredientes;
        List<Ingrediente> totalIngredientes;

        public Resultados_ValoresIngredientes(List<RespuestasIngredientes> respIngr, List<Ingrediente> todosIngredientes, TimeSpan tiempoTotal)
        {
            InitializeComponent();

            respuestasIngredientes = new List<RespuestasIngredientes>(respIngr);
            totalIngredientes = new List<Ingrediente>(todosIngredientes);
            totalTiempoUsuario = tiempoTotal;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblRespuestasCorrectas.Content = respCorrectas.ToString();

            respCorrectas = respuestasIngredientes.Count(x => x.EsRespuestaCorrecta);
            totalPreguntas = respuestasIngredientes.Last().NumeroRespuesta + 1;
            porcCorrectas = respCorrectas / totalPreguntas;

            lblRespuestasCorrectas.Content = respCorrectas.ToString();
            lblTotalPreguntas.Content = totalPreguntas.ToString();
            lblCorrectasPorcentaje.Content = porcCorrectas.ToString("P1");
            if (porcCorrectas >= 0.75m)
                lblCorrectasPorcentaje.Foreground = new SolidColorBrush(Colors.Green);
            else if (porcCorrectas >= 0.50m)
                lblCorrectasPorcentaje.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#d6c038"));
            else
                lblCorrectasPorcentaje.Foreground = new SolidColorBrush(Colors.Red);
            lblTotalTiempo.Content = totalTiempoUsuario.ToString(@"hh\:mm\:ss");

            lbIngredientes.ItemsSource = respuestasIngredientes;
        }

        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Q))
            {
                Close();
            }
        }

        private void lbIngredientes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var ingredienteSeleccionado = lbIngredientes.SelectedItem as RespuestasIngredientes;

            lblValorIndividualCorrecto.Content = ingredienteSeleccionado.precio_small.ToString("C0");
            lblValorGrandeCorrecto.Content = ingredienteSeleccionado.precio_big.ToString("C0");

            lblValorIndividualUsuario.Content = ingredienteSeleccionado.precio_small_usuario.ToString("C0");
            lblValorGrandeUsuario.Content = ingredienteSeleccionado.precio_big_usuario.ToString("C0");
        }
    }
}
