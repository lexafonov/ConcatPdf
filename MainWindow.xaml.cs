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
        public static int Main(string[] args)
        {
            MainWindow window = new MainWindow();
            window.ShowDialog();
            return 0;
        }
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
