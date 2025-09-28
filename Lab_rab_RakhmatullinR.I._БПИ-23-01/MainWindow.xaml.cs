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

namespace Lab_rab_RakhmatullinR.I._БПИ_23_01
{
    public partial class MainWindow : Window
    {
        private ComplexNumberCalculator _complexNumberCalculator;
        public ComplexNumberCalculator ComplexNumberCalculatorA
        {
            get => _complexNumberCalculator;
            set => _complexNumberCalculator = value;
        }

        private List<string> calculationHistory = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateModulusBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput() && TryUpdateComplexNumber())
            {
                double modulus = ComplexNumberCalculatorA.CalculateModulus();
                ModulusResultText.Text = $"{modulus:F4}";

                string historyEntry = $"{DateTime.Now:HH:mm:ss} - Модуль числа {ComplexNumberCalculatorA.GetComplexNumberString()} = {modulus:F4}";
                calculationHistory.Add(historyEntry);
                UpdateHistory();
            }
        }

        private void CalculateArgumentBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateInput() && TryUpdateComplexNumber())
            {
                double argument = ComplexNumberCalculatorA.CalculateArgument();
                ArgumentResultText.Text = $"{argument:F2}°";

                string historyEntry = $"{DateTime.Now:HH:mm:ss} - Аргумент числа {ComplexNumberCalculatorA.GetComplexNumberString()} = {argument:F2}°";
                calculationHistory.Add(historyEntry);
                UpdateHistory();
            }
        }

        private bool TryUpdateComplexNumber()
        {
            if (ComplexNumberCalculator.TryCreateComplexNumber(
                RealPartTextBox.Text, ImaginaryPartTextBox.Text, out ComplexNumberCalculator complex))
            {
                ComplexNumberCalculatorA = complex;
                return true;
            }
            return false;
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(RealPartTextBox.Text) ||
                string.IsNullOrWhiteSpace(ImaginaryPartTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, введите обе части комплексного числа.",
                              "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!double.TryParse(RealPartTextBox.Text, out _) ||
                !double.TryParse(ImaginaryPartTextBox.Text, out _))
            {
                MessageBox.Show("Пожалуйста, введите корректные числовые значения.",
                              "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void UpdateHistory()
        {
            HistoryListBox.Items.Clear();
            foreach (var entry in calculationHistory)
            {
                HistoryListBox.Items.Add(entry);
            }

            if (HistoryListBox.Items.Count > 0)
            {
                HistoryListBox.ScrollIntoView(HistoryListBox.Items[HistoryListBox.Items.Count - 1]);
            }
        }

        private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            string currentText = textBox.Text;
            string newText = currentText.Insert(textBox.SelectionStart, e.Text);
            if (e.Text == "-")
            {
                e.Handled = textBox.SelectionStart != 0 || currentText.Contains("-");
            }
            else if (e.Text == ".")
            {
                e.Handled = currentText.Contains(".");
            }
            else if (e.Text == ",")
            {
                e.Handled = true;
                textBox.Text = currentText.Insert(textBox.SelectionStart, ".");
                textBox.SelectionStart = textBox.SelectionStart + 1;
            }
            else
            {
                e.Handled = !char.IsDigit(e.Text, 0);
            }
        }

        private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                e.Handled = true;
            }
        }

        private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ModulusResultText.Text = "";
            ArgumentResultText.Text = "";
        }
    }
}

