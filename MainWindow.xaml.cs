using System;
using System.Windows;

namespace ConcatPdf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [STAThread]
        public static int Main()
        {
            MainWindow window = new MainWindow();
            var viewModel = new ViewModel(window);
            window.DataContext = viewModel;
            
            window.ShowDialog();
            return 0;
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
