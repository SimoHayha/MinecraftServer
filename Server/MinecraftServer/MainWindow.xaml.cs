using MinecraftServer.GUI;
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
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MinecraftServer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ViewModel viewModel;

        public MainWindow()
        {
            //viewModel = new ViewModel();
            //DataContext = viewModel;

            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {

            }

            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            viewModel = DataContext as ViewModel;
            viewModel.Log("Type \"help\" to list all the commands available.");
            input.KeyDown += viewModel.ConsoleKeyDown;
        }
    }
}
