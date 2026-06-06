using System.Diagnostics.Eventing.Reader;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rekensommen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random _rng = new Random();
        int _expectedResult;
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Range_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            
            bool isNumber = int.TryParse(currentTextBox.Text, out int value);

            if (!isNumber || value > 100 || value < 0)
            {
                currentTextBox.Background = Brushes.LightCoral;
            }
            else
            {
                currentTextBox.Background = Brushes.White;
            }
        }

        private void Range_KeyDown(object sender, KeyEventArgs e)
        {
            if (!((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Enter))
            {
                e.Handled = true;
            }
        }

        private void OnEqualsMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            StartExercise();
        }

        private void StartExercise()
        {
            resultTextBox.Clear();
            resultTextBox.Background = Brushes.White;
            resultTextBox.IsEnabled = true;

            //TODO: generate random numbers
            int number1 = _rng.Next(0, 101);
            int number2 = _rng.Next(0,101);

            //TODO: calculate result and store the value in _expectedResult
            _expectedResult = number1 + number2;

            firstNumberLabel.Content = number1.ToString();
            operatorLabel.Content = "+";
            secondNumberLabel.Content = number2.ToString();

            //TODO: call InitStopWatch

            resultTextBox.Focus();
        }

        private void ResultTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CheckResult(resultTextBox))
                {
                    resultTextBox.IsEnabled = false;
                }
                else
                {
                    resultTextBox.SelectAll(); //tekst niet verwijderen maar selecteren zodat gebruiker ziet wat hij fout heeft gedaan
                }
            }
        }

        private bool CheckResult(TextBox textBox)
        {
            //TODO:
            //check if the input from resultTextBox is a number (TIP: use TryParse)            
            //check if the input is equal to _expectedResult
            //change the backgroundcolor to lightgreen or lightcoral

            bool isNumber = int.TryParse(textBox.Text, out int result);
            bool isCorrect = result == _expectedResult;

            if (!isNumber || !isCorrect)
            {
                textBox.Background = Brushes.LightCoral;
            }
            else
            {
                textBox.Background = Brushes.LightGreen;
            }

            return (isNumber && isCorrect);
        
        }
       
    }
}