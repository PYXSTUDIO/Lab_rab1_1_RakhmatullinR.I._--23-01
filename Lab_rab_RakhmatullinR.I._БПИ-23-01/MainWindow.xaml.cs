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
            private List<string> calculationHistory = new List<string>();

            public MainWindow()
            {
                InitializeComponent();
            }

            // Обработчик для вычисления модуля
            private void CalculateModulusBtn_Click(object sender, RoutedEventArgs e)
            {
                if (ValidateInput() && ComplexNumberCalculator.TryCreateComplexNumber(
                    RealPartTextBox.Text, ImaginaryPartTextBox.Text, out ComplexNumberCalculator complex))
                {
                    double modulus = complex.CalculateModulus();
                    ModulusResultText.Text = $"{modulus:F4}";

                    string historyEntry = $"{DateTime.Now:HH:mm:ss} - Модуль числа {complex.GetComplexNumberString()} = {modulus:F4}";
                    calculationHistory.Add(historyEntry);
                    UpdateHistory();
                }
            }

            // Обработчик для вычисления аргумента
            private void CalculateArgumentBtn_Click(object sender, RoutedEventArgs e)
            {
                if (ValidateInput() && ComplexNumberCalculator.TryCreateComplexNumber(
                    RealPartTextBox.Text, ImaginaryPartTextBox.Text, out ComplexNumberCalculator complex))
                {
                    double argument = complex.CalculateArgument();
                    ArgumentResultText.Text = $"{argument:F2}°";

                    string historyEntry = $"{DateTime.Now:HH:mm:ss} - Аргумент числа {complex.GetComplexNumberString()} = {argument:F2}°";
                    calculationHistory.Add(historyEntry);
                    UpdateHistory();
                }
            }

            // Валидация ввода
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

            // Обновление истории вычислений
            private void UpdateHistory()
            {
                HistoryListBox.Items.Clear();
                foreach (var entry in calculationHistory)
                {
                    HistoryListBox.Items.Add(entry);
                }

                // Автопрокрутка к последнему элементу
                if (HistoryListBox.Items.Count > 0)
                {
                    HistoryListBox.ScrollIntoView(HistoryListBox.Items[HistoryListBox.Items.Count - 1]);
                }
            }

            // Валидация вводимых данных - только числа
            private void NumberTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
            {
                TextBox textBox = sender as TextBox;
                string currentText = textBox.Text;
                string newText = currentText.Insert(textBox.SelectionStart, e.Text);

                // Разрешаем: цифры, минус, точка (только одна)
                if (e.Text == "-")
                {
                    // Минус можно только в начале
                    e.Handled = textBox.SelectionStart != 0 || currentText.Contains("-");
                }
                else if (e.Text == ".")
                {
                    // Точка может быть только одна
                    e.Handled = currentText.Contains(".");
                }
                else if (e.Text == ",")
                {
                    // Заменяем запятую на точку
                    e.Handled = true;
                    textBox.Text = currentText.Insert(textBox.SelectionStart, ".");
                    textBox.SelectionStart = textBox.SelectionStart + 1;
                }
                else
                {
                    // Проверяем, что вводится цифра
                    e.Handled = !char.IsDigit(e.Text, 0);
                }
            }

            // Запрет пробелов
            private void NumberTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
            {
                if (e.Key == Key.Space)
                {
                    e.Handled = true;
                }
            }

            // Очистка результатов при изменении ввода
            private void NumberTextBox_TextChanged(object sender, TextChangedEventArgs e)
            {
                ModulusResultText.Text = "";
                ArgumentResultText.Text = "";
            }
        }
    }

