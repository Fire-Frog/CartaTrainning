using CartaTrainning.Classes;
using Newtonsoft.Json;
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
using System.Windows.Threading;
using MoreLinq;

namespace CartaTrainning
{
    /// <summary>
    /// Lógica de interacción para Pizza_Ingredientes.xaml
    /// </summary>
    public partial class Pizza_Ingredientes : Window
    {
        List<Ingrediente> totalIngredientes;
        List<Pizza> totalPizzas;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        DateTime timerIniciado;
        List<Ingrediente> ingredientesElegidos;
        List<Pizza> preguntasPizzas;
        int numPreg = 0;
        List<RespuestasPizzas> respuestasPizzas;
        BitmapImage imagenOk = new BitmapImage(new Uri("Resources/Iconos/icons8-ok-48.png", UriKind.Relative));
        BitmapImage imagenError = new BitmapImage(new Uri("Resources/Iconos/icons8-delete-48.png", UriKind.Relative));
        BitmapImage imagenPepega = new BitmapImage(new Uri("Resources/Iconos/pepega.png", UriKind.Relative));
        BitmapImage imagenLULW = new BitmapImage(new Uri("Resources/Iconos/LULW.png", UriKind.Relative));
        TimeSpan tiempoTranscurrido;

        public Pizza_Ingredientes()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var jsonObj = Pizzeria_RO.LoadJSON(); //cargar archivo json

            totalPizzas = jsonObj.pizzas.OrderBy(x => x.id).ToList();
            totalIngredientes = jsonObj.ingredientes.OrderBy(x => x.nombre).ToList();

            cboxIngredientes.ItemsSource = totalIngredientes;
            cboxIngredientes.DisplayMemberPath = "nombre";

            ingredientesElegidos = new List<Ingrediente>();
            lbIngredientesElegidos.DisplayMemberPath = "nombre";
            lbIngredientesElegidos.ItemsSource = ingredientesElegidos;

            //lblTotalPreguntas.Text = "Total Preguntas: " + totalPizzas.Count;

            btnAyuda.IsEnabled = false;
            tbAyuda.Visibility = Visibility.Hidden;

            aTimer.Elapsed += ATimer_Elapsed;
            aTimer.Interval = 1000;

            preguntasPizzas = new List<Pizza>();
            respuestasPizzas = new List<RespuestasPizzas>();
        }


        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void cboxIngredientes_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Add)
            {
                AgregarIngredienteListBox();
            }
        }

        private void lbIngredientesElegidos_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Subtract || e.Key == Key.Delete)
            {
                QuitarIngredienteListBox();
            }
        }

        private void btnAgregar_Click(object sender, RoutedEventArgs e)
        {
            AgregarIngredienteListBox();
        }

        private void btnQuitar_Click(object sender, RoutedEventArgs e)
        {
            QuitarIngredienteListBox();
        }

        private void AgregarIngredienteListBox()
        {
            var item = cboxIngredientes.SelectedItem as Ingrediente;
            if (item != null) //&& !ingredientesElegidos.Any(x => x.id == item.id)) // los ingredientes pueden ser mas de uno del mismo en una misma pizza lul
            {
                ingredientesElegidos.Add(item);
            }

            lbIngredientesElegidos.Items.Refresh();

            if (rbtnEasy.IsChecked.Value)
            {
                tbAyuda.Text = string.Format("Total de Ingredientes: {0}/{1}", ingredientesElegidos.Count, preguntasPizzas[numPreg].ingredientes.Count);
            }
        }

        private void QuitarIngredienteListBox()
        {
            var item = lbIngredientesElegidos.SelectedItem as Ingrediente;
            if (item != null)
            {
                //seleccion
                ingredientesElegidos.Remove(item);
            }
            else if (lbIngredientesElegidos.Items.Count > 0)
            {
                var primerItem = ingredientesElegidos.First();
                ingredientesElegidos.Remove(primerItem);
            }

            lbIngredientesElegidos.Items.Refresh();

            if (rbtnEasy.IsChecked.Value)
            {
                tbAyuda.Text = string.Format("Total de Ingredientes: {0}/{1}", ingredientesElegidos.Count, preguntasPizzas[numPreg].ingredientes.Count);
            }
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            timerIniciado = DateTime.Now;
            aTimer.Start();
            gbRespuestas.IsEnabled = true;
            preguntasPizzas = totalPizzas.Shuffle().ToList(); //randomize questions
            numPreg = 0;
            lblPregunta.Text = string.Format(BaseLabels.Questions[0], preguntasPizzas[numPreg].nombre);
            btnIniciar.IsEnabled = false;
            gbNiveles.IsEnabled = false;
            btnFinalizar.IsEnabled = true;
            if (rbtnEasy.IsChecked.Value)
            {
                btnAyuda.IsEnabled = true;
                tbAyuda.Visibility = Visibility.Visible;
                tbAyuda.Text = string.Format("Total de Ingredientes: {0}/{1}", ingredientesElegidos.Count, preguntasPizzas[numPreg].ingredientes.Count);
            }
            else
            {
                btnAyuda.IsEnabled = false;
                tbAyuda.Visibility = Visibility.Visible;
                tbAyuda.Text = string.Empty;
            }

            lbRespuestaCorrecta.Items.Clear();
            lbRespuestaUsuario.Items.Clear();
            lblPizzaRespuesta.Content = "--";
        }

        private void ATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tiempoTranscurrido = timerIniciado - e.SignalTime;
            Dispatcher.Invoke(() =>
            {
                lblTiempo.Text = string.Format("Tiempo: {0}", tiempoTranscurrido.ToString(@"hh\:mm\:ss"));
            });
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {
            aTimer.Stop();
            lblPregunta.Text = "Iniciar para realizar preguntas";
            btnIniciar.IsEnabled = true;
            btnFinalizar.IsEnabled = false;
            gbNiveles.IsEnabled = true;

            imageRespuestaIncorrecta.Source = null;
            imageRespuestaCorrecta.Source = null;
            tbSubtituloValidacion.Text = string.Empty;
            gbRespuestas.IsEnabled = false;

            if (respuestasPizzas.Count > 0)
            {
                //MessageBox.Show(this, sb.ToString(), "Resultados", MessageBoxButton.OK);
                Resultados_PizzaIngredientes window = new Resultados_PizzaIngredientes(respuestasPizzas, totalIngredientes, tiempoTranscurrido);
                respuestasPizzas.Clear(); //clear after window

                var isOpen = window.ShowDialog();
                Focus(); //for ctrl+q command
            }

            tbAyuda.Text = string.Empty;
            tbAyuda.Visibility = Visibility.Visible;

            ingredientesElegidos.Clear();
            lbIngredientesElegidos.Items.Refresh();
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            var ingredientesRespuestas = lbIngredientesElegidos.Items.Cast<Ingrediente>().ToList();
            var respIngr = new int[ingredientesRespuestas.Count];

            for (int i = 0; i < ingredientesRespuestas.Count; i++) respIngr[i] = ingredientesRespuestas[i].id;

            respuestasPizzas.Add(new RespuestasPizzas()
            {
                NumeroRespuesta = numPreg,
                nombre = preguntasPizzas[numPreg].nombre,
                id = preguntasPizzas[numPreg].id,
                precio_big = preguntasPizzas[numPreg].precio_big,
                precio_small = preguntasPizzas[numPreg].precio_small,
                vegetariana = preguntasPizzas[numPreg].vegetariana,
                IngredientesUsuario =  respIngr,
                ingredientes = preguntasPizzas[numPreg].ingredientes,
                EsRespuestaCorrecta = false
            });

            respuestasPizzas[numPreg].EsRespuestaCorrecta = respuestasPizzas[numPreg].IngredientesUsuario.All(x=> preguntasPizzas[numPreg].ingredientes.Contains(x)) //2 listas iguales sin importar orden de ingreso
                                                            && respuestasPizzas[numPreg].IngredientesUsuario.Length == preguntasPizzas[numPreg].ingredientes.Count; //cantidad de objetos iguales
            CalcularTotales();

            lblPizzaRespuesta.Content = respuestasPizzas[numPreg].nombre;
            lbRespuestaCorrecta.DisplayMemberPath = "nombre";
            lbRespuestaUsuario.DisplayMemberPath = "nombre";

            lbRespuestaCorrecta.Items.Clear();
            lbRespuestaUsuario.Items.Clear();

            foreach (var item in respuestasPizzas[numPreg].ingredientes.OrderBy(x => x))
            {
                var ingredienteRespuesta = totalIngredientes.Single(x => x.id == item);
                lbRespuestaCorrecta.Items.Add(ingredienteRespuesta);
            }

            foreach (var item in respuestasPizzas[numPreg].IngredientesUsuario.OrderBy(x => x))
            {
                var ingredienteRespuesta = totalIngredientes.Single(x => x.id == item);
                lbRespuestaUsuario.Items.Add(ingredienteRespuesta);
            }

            var numMensajeRandom = new Random().Next(0, BaseLabels.ValidacionOK.Length);

            if (respuestasPizzas[numPreg].EsRespuestaCorrecta)
            {
                var textoOk = BaseLabels.ValidacionOK[numMensajeRandom];
                imageRespuestaCorrecta.Source = (numMensajeRandom > 0) ? imagenOk : imagenPepega;
                imageRespuestaIncorrecta.Source = null;
                tbSubtituloValidacion.Text = textoOk;
            }
            else
            {
                imageRespuestaIncorrecta.Source = (numMensajeRandom > 0) ? imagenError : imagenLULW;
                imageRespuestaCorrecta.Source = null;
                tbSubtituloValidacion.Text = BaseLabels.ValidacionError[numMensajeRandom];
            }

            numPreg++;

            ingredientesElegidos.RemoveRange(0, ingredientesElegidos.Count);
            lbIngredientesElegidos.Items.Refresh();
            cboxIngredientes.SelectedIndex = -1;
            if (numPreg < preguntasPizzas.Count)
            {
                lblPregunta.Text = string.Format(BaseLabels.Questions[0], preguntasPizzas[numPreg].nombre); //nueva pregunta
                if (rbtnEasy.IsChecked.Value)
                {
                    tbAyuda.Text = string.Format("Total de Ingredientes: {0}/{1}", ingredientesElegidos.Count, preguntasPizzas[numPreg].ingredientes.Count);
                }
            }
            else
            {
                btnFinalizar_Click(sender, e);
            }
        }

        private void CalcularTotales()
        {
            decimal respCorrectas = respuestasPizzas.Count(x => x.EsRespuestaCorrecta);
            decimal totalPreguntas = respuestasPizzas.Last().NumeroRespuesta + 1;
            decimal porcCorrectas = 0;
            if (totalPreguntas > 0) porcCorrectas = respCorrectas / totalPreguntas;

            lblRespuestasCorrectas.Text = string.Format("Respuestas Correctas: {0}", respCorrectas.ToString());
            lblTotalPreguntas.Text = string.Format("Total Preguntas: {0}", totalPreguntas.ToString());
            lblPorcentajeCorrectas.Text = string.Format("Correctas: {0}", porcCorrectas.ToString("P1"));
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Q))
            {
                Close();
            }
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            var pizzaAyuda = preguntasPizzas[numPreg];
            var rndIngrediente = new Ingrediente();

            if (ingredientesElegidos.Count < pizzaAyuda.ingredientes.Count)
            {
                do
                {
                    var rndAyuda = new Random().Next(0, pizzaAyuda.ingredientes.Count);
                    rndIngrediente = totalIngredientes.Single(x => x.id == pizzaAyuda.ingredientes[rndAyuda]);
                } while (ingredientesElegidos.Any(x => x.id == rndIngrediente.id));

                ingredientesElegidos.Add(rndIngrediente);
                lbIngredientesElegidos.Items.Refresh();
            }

            tbAyuda.Visibility = Visibility.Visible;
            tbAyuda.Text = string.Format("Total de Ingredientes: {0}/{1}", ingredientesElegidos.Count, pizzaAyuda.ingredientes.Count);
        }
    }
}
