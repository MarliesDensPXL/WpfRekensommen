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
using System.Windows.Threading;
using Microsoft.VisualBasic;

namespace Rekensommen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random _rng = new Random();
        int _expectedResult;
        DispatcherTimer _stopWatch = new DispatcherTimer();
        DateTime _stopWatchBegin;
        TimeSpan _elapsedTime;
        TimeSpan _highScore = TimeSpan.MaxValue; // heel grote startwaarde, zodat de gebruiker zeker minder heeft en er vanaf ronde 1 een nieuwe highscore komt. 
        
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
            

            
            //int number2 = _rng.Next(0,101);

            //TODO: calculate result and store the value in _expectedResult
            string randomOperator = GetRandomOperator();

            int number1 = 0;
            int number2 = 0;
            int max = int.Parse(maximumResultTextBox.Text);

            if (randomOperator.Equals("+"))
            {                
                number1 = (applyMaximumRadioButton.IsChecked == true) ? _rng.Next(0, max + 1) : _rng.Next(0, 101);
                number2 = (applyMaximumRadioButton.IsChecked == true) ? _rng.Next(0, (max - number1) + 1) : _rng.Next(0, 101);
            }
            else
            {
                number1 = (applyMaximumRadioButton.IsChecked == true) ? _rng.Next(0, max + 1) : _rng.Next(0, 101);
                number2 = (disallowNegativeOutcomeRadioButton.IsChecked == true) ? _rng.Next(0, number1 + 1) : _rng.Next(0, 101);
            }

                _expectedResult = (randomOperator.Equals("+")) ? (number1 + number2) : (number1 - number2); //+ of - bewerking afhankelijk van GetRandomOperator

            firstNumberLabel.Content = number1.ToString();
            operatorLabel.Content = randomOperator;
            secondNumberLabel.Content = number2.ToString();
                       

            resultTextBox.Focus();

            //TODO: call InitStopWatch
            InitStopWatch();
        }

        private void ResultTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (CheckResult(resultTextBox))
                {
                    resultTextBox.IsEnabled = false;
                    _stopWatch.Stop(); //timer stoppen
                   
                    if (_elapsedTime < _highScore)
                    {
                        MessageBox.Show("Nieuwe recordtijd gehaald!", "RECORD", MessageBoxButton.OK, MessageBoxImage.Information);
                        _highScore = _elapsedTime;
                    }
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

            //bool isNumber = int.TryParse(textBox.Text, out int result);
            //bool isCorrect = result == _expectedResult;

            //if (!isNumber || !isCorrect)
            //{
            //    textBox.Background = Brushes.LightCoral;
            //}
            //else
            //{
            //    textBox.Background = Brushes.LightGreen;
            //}

            //return (isNumber && isCorrect);

            bool isNumber = int.TryParse(textBox.Text, out int result);
            if (!isNumber || result != _expectedResult)
            {
                textBox.Background = Brushes.LightCoral;
                return false;
            }          
                      
              textBox.Background = Brushes.LightGreen;
              return true;    
        }

        private void OnShowTimeButtonClicked(object sender, RoutedEventArgs e)
        {
            DateTime dateTime = DateTime.Now;

            MessageBox.Show(dateTime.ToString("ddd dd MMMM yyyy HH:mm"), "Datum en tijd", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void InitStopWatch()
        {
            _stopWatch.Interval = TimeSpan.FromMilliseconds(1);
            _stopWatch.Tick += _stopWatch_Tick;
            _stopWatch.Start();
            _stopWatchBegin = DateTime.Now;
        }

        private void _stopWatch_Tick(object? sender, EventArgs e)
        {
            _elapsedTime = DateTime.Now - _stopWatchBegin;
            timerLabel.Content = ($"{_elapsedTime.Minutes:00}:{_elapsedTime.Seconds:00}:{_elapsedTime.Milliseconds:000}");

        }

        private string GetRandomOperator()
        {
            if (addOperatorCheckBox.IsChecked == true && subtractOperatorCheckBox.IsChecked == true)
            {
                int randomOperator = _rng.Next(0, 2);
                if (randomOperator == 0)
                {
                    return "+";
                }
                else
                {
                    return "-";
                }
            }
            else if (addOperatorCheckBox.IsChecked == true)
            {
                return "+";
            }

            return "-";          
        }

        private void ApplyMaximum_CheckChanged(object sender, RoutedEventArgs e)
        {
            if (applyMaximumRadioButton.IsChecked == true)
            {
                maximumResultTextBox.IsEnabled = true;
            }
            else
            {
                maximumResultTextBox.IsEnabled = false;
            }
        }
    }
}