using System;
using System.Collections.Generic;
using System.IO;
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
using MahApps.Metro.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ScoreKeeper.Model;
using ScoreKeeper.ViewModels;
using ScoreKeeper.Views;

namespace ScoreKeeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            this.DataContext = new MainWindowViewModel(NavigateToView);
        }

        private void NavigateToView(UserControl view)
        {
            MainArea.Content = view;
        }
        
    }
}
