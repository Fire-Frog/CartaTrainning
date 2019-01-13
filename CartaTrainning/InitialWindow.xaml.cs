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
    /// Lógica de interacción para InitialWindow.xaml
    /// </summary>
    public partial class InitialWindow : Window
    {
        public InitialWindow()
        {
            InitializeComponent();
        }

        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void menuitemIngredientesPizza_Click(object sender, RoutedEventArgs e)
        {
            Pizza_Ingredientes window = new Pizza_Ingredientes();
            var isclosedwindow = window.ShowDialog();
        }

        private void menuitemValoresPizza_Click(object sender, RoutedEventArgs e)
        {
            Valores_Pizza window = new Valores_Pizza();
            var isclosedwindow = window.ShowDialog();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Q))
            {
                Close();
            }
        }

        private void menuitemValoresIngredientes_Click(object sender, RoutedEventArgs e)
        {
            Valores_Ingredientes window = new Valores_Ingredientes();
            var isclosedwindow = window.ShowDialog();
        }
    }
}