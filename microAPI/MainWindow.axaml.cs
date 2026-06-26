using microAPI.ViewModels;
using Avalonia.Interactivity;
using Avalonia.Input;
using Avalonia.Controls;

namespace microAPI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        public MainWindow(MainViewModel viewModel) : this()
        {
            DataContext = viewModel;
        }
        // Inicia edição ao dar clique duplo no nome
        private void TextBlock_DoubleTapped(object? sender, TappedEventArgs e)
        {
            if (sender is TextBlock textBlock && textBlock.DataContext != null)
            {
                dynamic vm = textBlock.DataContext;
                vm.StartEditCommand.Execute(null);
            }
        }

        // Salva ao perder o foco (clicar fora)
        private void TextBox_LostFocus(object? sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox && textBox.DataContext != null)
            {
                dynamic vm = textBox.DataContext;
                vm.EndEditCommand.Execute(null);
            }
        }

        // Salva ao pressionar a tecla Enter
        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Enter)
            {
                if (sender is TextBox textBox && textBox.DataContext != null)
                {
                    dynamic vm = textBox.DataContext;
                    vm.EndEditCommand.Execute(null);
                }
            }
        }
    }
}
