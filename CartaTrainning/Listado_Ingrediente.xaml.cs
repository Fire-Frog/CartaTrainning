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
    /// Lógica de interacción para Listado_Ingrediente.xaml
    /// </summary>
    public partial class Listado_Ingrediente : Window
    {
        List<Ingrediente> totalIngredientes;
        List<Pizza> totalPizzas;

        public Listado_Ingrediente()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var jsonObj = Pizzeria_RO.LoadJSON();
            totalPizzas = jsonObj.pizzas.OrderBy(x => x.id).ToList();
            totalIngredientes = jsonObj.ingredientes.Where(x=> x.carta).OrderBy(x => x.id).ToList();

            lbIngredientes.ItemsSource = totalIngredientes;
            lbIngredientes.DisplayMemberPath = "nombre";
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
            var ingrElegido = lbIngredientes.SelectedItem as Ingrediente;
            var indLabel = 1;

            lblPizza.Content = ingrElegido.nombre;
            lblValorGrande.Content = ingrElegido.precio_big.ToString("C0");
            lblValorIndividual.Content = ingrElegido.precio_small.ToString("C0");

            var pizzasBuscadas = totalPizzas.Where(x => x.ingredientes.Contains(ingrElegido.id)).ToList();

            foreach (var item in pizzasBuscadas)
            {
                var ctrlLabel = gbDetalle.FindName("lblPizzas" + indLabel) as Label;
                ctrlLabel.Content = item.nombre;
                indLabel++;
            }

            for (int i = indLabel; i <= 8; i++)
            {
                var ctrlLabel = gbDetalle.FindName("lblPizzas" + i) as Label;
                ctrlLabel.Content = string.Empty;
            }
        }
    }
}
