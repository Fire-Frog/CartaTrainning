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

        public Valores_Ingredientes()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(txbValorIndividual, OnPaste);
            DataObject.AddPastingHandler(txbValorGrande, OnPaste);
        }

        private void menuitemSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnIniciar_Click(object sender, RoutedEventArgs e)
        {
            timerIniciado = DateTime.Now;
        }

        private void btnFinalizar_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control && Keyboard.IsKeyDown(Key.Q))
            {
                Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            aTimer.Elapsed += ATimer_Elapsed;
            aTimer.Interval = 1000;
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
            FormatTextBox(sender as TextBox);
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
    }
}
