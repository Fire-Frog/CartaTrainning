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
using System.Windows.Navigation;
using System.Windows.Shapes;
using CartaTrainning.Classes;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using MoreLinq;

namespace CartaTrainning
{
    /// <summary>
    /// Lógica de interacción para old_MainWindow.xaml
    /// </summary>
    public partial class old_MainWindow : Window
    {
        List<Ingrediente> totalIngredientes;
        List<Pizza> totalPizzas;
        List<Ingrediente> ingredientesDistintos;
        Random rndNumber = new Random();
        List<RadioButton> childrenRadioButton;

        List<int> respuestasValor;
        List<Ingrediente> respuestasIngredientes;
        Pizza pizzaSeleccionada;

        public old_MainWindow()
        {
            InitializeComponent();
        }

        private void LoadHelpText()
        {
            StringBuilder helptext = new StringBuilder();
            /*foreach (var item in BaseLabels.Levels)
            {
                helptext.AppendLine(item);
            }

            lblHelp.Text = helptext.ToString();*/

            rbtnLevelEasy.ToolTip = BaseLabels.Levels[0];
            rbtnLevelMedium.ToolTip = BaseLabels.Levels[1];
            rbtnLevelHard.ToolTip = BaseLabels.Levels[2];
        }

        private void LoadJSON()
        {
            var textjson = File.ReadAllText("Resources/format_pizzeria.json");
            var jsonObj = JsonConvert.DeserializeObject<Pizzeria_RO>(textjson);
            totalPizzas = jsonObj.pizzas;
            totalIngredientes = jsonObj.ingredientes;
            ingredientesDistintos = totalIngredientes.Where(x => x.carta).DistinctBy(x => x.precio_small).ToList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            childrenRadioButton = gbAnswers.Children.OfType<RadioButton>().ToList();
            foreach (var item in childrenRadioButton)
            {
                item.IsEnabled = false;
                item.Visibility = Visibility.Hidden;
            }

            LoadHelpText();
            LoadJSON();
            lblQuestion.Text += Environment.NewLine + string.Format("(Pizzas: {0} - Ingredientes: {1})", totalPizzas.Count, totalIngredientes.Where(x => x.carta).Count());
        }

        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            //0-1 ingredientes
            //2-3 pizzas
            //4 ingrediente en pizza

            respuestasIngredientes = new List<Ingrediente>();
            respuestasValor = new List<int>();
            pizzaSeleccionada = new Pizza();

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;

            btnSiguiente.IsEnabled = true;
            btnOmitir.IsEnabled = true;

            foreach (var item in gbLevel.Children.OfType<RadioButton>())
            {
                item.IsEnabled = false;
            }

            lblHelp.Text = string.Empty;

            if (rbtnLevelEasy.IsChecked.Value)
            {
                //4 respuestas + ayuda
                btnHalp.Visibility = Visibility.Visible; //halp
            }
            else if (rbtnLevelMedium.IsChecked.Value)
            {
                //5 respuestas
                var questionNumber = rndNumber.Next(0, BaseLabels.Questions.Length);
                questionNumber = 1;
                if (questionNumber == 1)
                {
                    //0-1 ingredientes
                    var ingreNumber = rndNumber.Next(0, totalIngredientes.Where(x => x.carta).Count());
                    var ingredienteSingle = totalIngredientes[ingreNumber];
                    lblQuestion.Text = string.Format(BaseLabels.Questions[questionNumber], ingredienteSingle.nombre);

                    var copyIngredients = ingredientesDistintos.ToList();
                    copyIngredients.Add(new Ingrediente() { precio_big = 1800, precio_small = 1400 });
                    copyIngredients.Add(new Ingrediente() { precio_big = 700, precio_small = 900 });

                    copyIngredients = copyIngredients.Shuffle().ToList();
                    int indexRespuesta = copyIngredients.FindIndex(x => x.precio_big == ingredienteSingle.precio_big && x.precio_small == ingredienteSingle.precio_small);

                    for (int i = 0; i < copyIngredients.Count; i++)
                    {
                        childrenRadioButton[i].Content = copyIngredients[i].precio_small + " / " + copyIngredients[i].precio_big;
                        childrenRadioButton[i].Visibility = Visibility.Visible;
                        childrenRadioButton[i].IsEnabled = true;
                    }

                    respuestasIngredientes.Add(ingredienteSingle);
                }
                else if (questionNumber == 2)
                {
                    //2-3 pizzas
                    var pizzaNumber = rndNumber.Next(0, totalPizzas.Count);
                    var pizzaSingle = totalPizzas[pizzaNumber];
                    lblQuestion.Text = string.Format(BaseLabels.Questions[questionNumber], pizzaSingle.nombre);
                }
                else
                {
                    //4 ingrediente en pizza
                    var pizzaNumber = rndNumber.Next(0, totalPizzas.Count);
                    var pizzaSingle = totalPizzas[pizzaNumber];
                    lblQuestion.Text = string.Format(BaseLabels.Questions[questionNumber], pizzaSingle.nombre);
                }
            }
            else if (rbtnLevelHard.IsChecked.Value)
            {
                //escoger respuestas
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            btnHalp.Visibility = Visibility.Hidden;
            foreach (var item in gbLevel.Children.OfType<RadioButton>())
            {
                item.IsEnabled = true;
            }

            LoadHelpText();
            lblQuestion.Text = "Iniciar para realizar preguntas";
            lblQuestion.Text += Environment.NewLine + string.Format("(Pizzas: {0} - Ingredientes: {1})", totalPizzas.Count, totalIngredientes.Where(x => x.carta).Count());
        }

        private void btnSiguiente_Click(object sender, RoutedEventArgs e)
        {
            var respuesta = gbAnswers.Children.OfType<RadioButton>().Where(x => x.IsChecked.Value);
        }

        private void btnOmitir_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
