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
    /// Lógica de interacción para Listado_Completo.xaml
    /// </summary>
    public partial class Listado_Pizza : Window
    {
        List<Ingrediente> totalIngredientes;
        List<Pizza> totalPizzas;

        public Listado_Pizza()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var jsonObj = Pizzeria_RO.LoadJSON();
            totalPizzas = jsonObj.pizzas.OrderBy(x => x.id).ToList();
            totalIngredientes = jsonObj.ingredientes.OrderBy(x => x.id).ToList();

            lbPizzas.ItemsSource = totalPizzas;
            lbPizzas.DisplayMemberPath = "nombre";
        }

        private void lbPizzas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pizzaElegida = lbPizzas.SelectedItem as Pizza;
            var indLabel = 1;

            lblPizza.Content = pizzaElegida.nombre;
            lblValorGrande.Content = pizzaElegida.precio_big.ToString("C0");
            lblValorIndividual.Content = pizzaElegida.precio_small.ToString("C0");
            lblVegetariana.Content = (pizzaElegida.vegetariana) ? "Si" : "No";

            foreach (var item in pizzaElegida.ingredientes)
            {
                var ingrBuscado = totalIngredientes.Single(x => x.id == item);
                var ctrlLabel = gbDetalle.FindName("lblIngrediente" + indLabel) as Label;
                ctrlLabel.Content = ingrBuscado.nombre;
                indLabel++;
            }

            for (int i = indLabel; i <= 5; i++)
            {
                var ctrlLabel = gbDetalle.FindName("lblIngrediente" + i) as Label;
                ctrlLabel.Content = string.Empty;
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
