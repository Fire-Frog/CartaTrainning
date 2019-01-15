﻿using CartaTrainning.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MoreLinq;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;

namespace CartaTrainning
{
    /// <summary>
    /// Lógica de interacción para Valores_Ingredientes.xaml
    /// </summary>
    public partial class Valores_Ingredientes : Window
    {
        BitmapImage imagenOk = new BitmapImage(new Uri("Resources/Iconos/icons8-ok-48.png", UriKind.Relative));
        BitmapImage imagenError = new BitmapImage(new Uri("Resources/Iconos/icons8-delete-48.png", UriKind.Relative));
        BitmapImage imagenPepega = new BitmapImage(new Uri("Resources/Iconos/pepega.png", UriKind.Relative));
        BitmapImage imagenLULW = new BitmapImage(new Uri("Resources/Iconos/LULW.png", UriKind.Relative));
        System.Timers.Timer aTimer = new System.Timers.Timer();
        DateTime timerIniciado;
        TimeSpan tiempoTranscurrido;
        List<Ingrediente> totalIngredientes;
        List<Ingrediente> preguntasIngredientes;
        List<RespuestasIngredientes> respuestasIngredientes;
        int numPreg = 0;
        RespuestasIngredientes respTemp;

        public Valores_Ingredientes()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(txbValorIndividual, OnPaste);
            DataObject.AddPastingHandler(txbValorGrande, OnPaste);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var jsonObj = Pizzeria_RO.LoadJSON();
            totalIngredientes = jsonObj.ingredientes.Where(x => x.carta).OrderBy(x => x.nombre).ToList(); //only menu ingredients

            aTimer.Elapsed += ATimer_Elapsed;
            aTimer.Interval = 1000;

            btnAyuda.IsEnabled = false;

            respuestasIngredientes = new List<RespuestasIngredientes>();
            respTemp = new RespuestasIngredientes();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            timerIniciado = DateTime.Now;
            aTimer.Start();
            preguntasIngredientes = totalIngredientes.Shuffle().ToList();
            gbRespuestas.IsEnabled = true;
            numPreg = 0;
            lblPregunta.Text = string.Format(BaseLabels.Questions[1], preguntasIngredientes[numPreg].nombre);
            btnIniciar.IsEnabled = false;
            gbNiveles.IsEnabled = false;
            btnFinalizar.IsEnabled = true;
            if (rbtnEasy.IsChecked.Value) btnAyuda.IsEnabled = true;
            else btnAyuda.IsEnabled = false;
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

            if (respuestasIngredientes.Count > 0)
            {
                Resultados_ValoresIngredientes window = new Resultados_ValoresIngredientes(respuestasIngredientes, totalIngredientes, tiempoTranscurrido);
                respuestasIngredientes.Clear(); //clear after window
                gbUltimaRespuesta.IsEnabled = false;
                var isOpen = window.ShowDialog();
                Focus(); //for ctrl+q command
            }
        }

        private void btnAceptar_Click(object sender, RoutedEventArgs e)
        {
            gbUltimaRespuesta.IsEnabled = true;
            int valorSmall = 0, valorBig = 0;
            string strSmall = txbValorIndividual.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');
            string strBig = txbValorGrande.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');

            if (strSmall.Length == 0) strSmall = "0";
            if (strBig.Length == 0) strBig = "0";

            if (!int.TryParse(strSmall, out valorSmall) || !int.TryParse(strBig, out valorBig))
            {
                MessageBox.Show("Hubo un error al ingresar los valor. Favor corregir e intentar nuevamente.",
                                "Error al ingresar valores",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                return; //why are you like this dude, pls no :(
            }

            respTemp.NumeroRespuesta = numPreg;
            respTemp.nombre = preguntasIngredientes[numPreg].nombre;
            respTemp.id = preguntasIngredientes[numPreg].id;
            respTemp.precio_big = preguntasIngredientes[numPreg].precio_big;
            respTemp.precio_small = preguntasIngredientes[numPreg].precio_small;
            respTemp.carta = preguntasIngredientes[numPreg].carta;
            respTemp.precio_small_usuario = valorSmall;
            respTemp.precio_big_usuario = valorBig;
            respTemp.EsRespuestaCorrecta = (respTemp.precio_big == respTemp.precio_big_usuario) && (respTemp.precio_small == respTemp.precio_small_usuario); //both values

            respuestasIngredientes.Add(respTemp);

            CalcularTotales();

            txbValorGrande.Text = "0";
            txbValorIndividual.Text = "0";

            lblUltimoIngrediente.Content = preguntasIngredientes[numPreg].nombre;
            lblValorGrandeCorrecto.Content = preguntasIngredientes[numPreg].precio_big.ToString("C0");
            lblValorIndividualCorrecto.Content = preguntasIngredientes[numPreg].precio_small.ToString("C0");
            lblValorGrandeUsuario.Content = respuestasIngredientes[numPreg].precio_big_usuario.ToString("C0");
            lblValorIndividualUsuario.Content = respuestasIngredientes[numPreg].precio_small_usuario.ToString("C0");

            var numMensajeRandom = new Random().Next(0, BaseLabels.ValidacionOK.Length);

            if (respuestasIngredientes[numPreg].EsRespuestaCorrecta)
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
            respTemp = new RespuestasIngredientes();

            if (numPreg < preguntasIngredientes.Count)
            {
                lblPregunta.Text = string.Format(BaseLabels.Questions[1], preguntasIngredientes[numPreg].nombre); //nueva pregunta
            }
            else
            {
                btnFinalizar_Click(sender, e);
            }
        }

        private void CalcularTotales()
        {
            decimal respCorrectas = respuestasIngredientes.Count(x => x.EsRespuestaCorrecta);
            decimal totalPreguntas = respuestasIngredientes.Last().NumeroRespuesta + 1;
            decimal porcCorrectas = 0;
            if (totalPreguntas > 0) porcCorrectas = respCorrectas / totalPreguntas;

            lblRespuestasCorrectas.Text = string.Format("Respuestas Correctas: {0}", respCorrectas.ToString());
            lblTotalPreguntas.Text = string.Format("Total Preguntas: {0}", totalPreguntas.ToString());
            lblPorcentajeCorrectas.Text = string.Format("Correctas: {0}", porcCorrectas.ToString("P1"));
        }

        private void btnAyuda_Click(object sender, RoutedEventArgs e)
        {
            int valorSmall = 0, valorBig = 0;
            string strSmall = txbValorIndividual.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');
            string strBig = txbValorGrande.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');

            if (strSmall.Length == 0) strSmall = "0";
            if (strBig.Length == 0) strBig = "0";

            if (!int.TryParse(strSmall, out valorSmall) || !int.TryParse(strBig, out valorBig))
            {
                MessageBox.Show("Hubo un error al ingresar los valor. Favor corregir e intentar nuevamente.",
                                "Error al ingresar valores (*)",
                                MessageBoxButton.OK, MessageBoxImage.Exclamation, MessageBoxResult.OK);
                return; //why are you like this dude, pls no :(
            }

            var ingrAyuda = preguntasIngredientes[numPreg];
            if (valorSmall != ingrAyuda.precio_small)
            {
                respTemp.precio_small_usuario = ingrAyuda.precio_small; //just in case
                txbValorIndividual.Text = ingrAyuda.precio_small.ToString();
            }
            else if (valorBig != ingrAyuda.precio_big)
            {
                respTemp.precio_big_usuario = ingrAyuda.precio_big;
                txbValorGrande.Text = ingrAyuda.precio_big.ToString();
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Q))
            {
                Close();
            }
        }

        private void ATimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            tiempoTranscurrido = timerIniciado - e.SignalTime;
            Dispatcher.Invoke(() =>
            {
                lblTiempo.Text = string.Format("Tiempo: {0}", tiempoTranscurrido.ToString(@"hh\:mm\:ss"));
            });
        }

        private void txbVariable_TextChanged(object sender, TextChangedEventArgs e)
        {
            var sndTxb = sender as TextBox;
            FormatTextBox(sndTxb);
        }

        private void FormatTextBox(TextBox txb)
        {
            //Remove previous formatting, or the decimal check will fail including leading zeros
            string value = txb.Text.Replace(",", "").Replace("$", "").Replace(".", "").TrimStart('0');
            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                //ul /= 100;
                //Unsub the event so we don't enter a loop
                txb.TextChanged -= txbVariable_TextChanged;
                //Format the text as currency
                txb.Text = string.Format(System.Globalization.CultureInfo.CreateSpecificCulture("es-CL"), "{0:C0}", ul);
                txb.TextChanged += txbVariable_TextChanged;
                txb.Select(txb.Text.Length, 0);
            }
            bool goodToGo = !TextEnteredIsValid(txb.Text);
            //enterButton.Enabled = goodToGo;
            if (!goodToGo || value.Length <= 0)
            {
                txb.Text = "$0";
                txb.Select(txb.Text.Length, 0);
            }
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;

            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            var goodToGo = !TextEnteredIsValid(text);
            if (!goodToGo) e.CancelCommand();
        }

        private bool TextEnteredIsValid(string txt)
        {
            Regex regex = new Regex("[^0-9]+$"); //regex that matches disallowed text
            return regex.IsMatch(txt);
        }

        private void txbVariable_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = TextEnteredIsValid(e.Text);
        }
        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void txbValorGeneral_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnAceptar);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }
    }
}
