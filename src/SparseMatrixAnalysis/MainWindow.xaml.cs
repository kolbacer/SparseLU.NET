using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using SparseMatrixAnalysis.Tests;

namespace SparseMatrixAnalysis
{
    public class WindowSettings : INotifyPropertyChanged
    {
        private Visibility showLoaderTab1;
        private Visibility showLoaderTab2;
        private string loaderTextTab1;
        private string loaderTextTab2;
        public event PropertyChangedEventHandler PropertyChanged;

        public WindowSettings()
        {
            showLoaderTab1 = Visibility.Hidden;
            showLoaderTab2 = Visibility.Hidden;
            loaderTextTab1 = "Loading";
            loaderTextTab2 = "Loading";
        }

        public Visibility ShowLoaderTab1
        {
            get => showLoaderTab1;
            set
            {
                showLoaderTab1 = value;
                OnPropertyChanged();
            }
        }
        
        public Visibility ShowLoaderTab2
        {
            get => showLoaderTab2;
            set
            {
                showLoaderTab2 = value;
                OnPropertyChanged();
            }
        }
        
        public string LoaderTextTab1
        {
            get => loaderTextTab1;
            set
            {
                loaderTextTab1 = value;
                OnPropertyChanged();
            }
        }
        
        public string LoaderTextTab2
        {
            get => loaderTextTab2;
            set
            {
                loaderTextTab2 = value;
                OnPropertyChanged();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public WindowSettings Settings { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            ResolutionTextBox.Text = "100";
            Settings = new WindowSettings();
            this.DataContext = Settings;
        }
        
        private void RunSparsityPatternAnalyzerButton_Click(object sender, RoutedEventArgs e)
        {
            FactorizationSparsityPatternTest.resolution = UInt32.Parse(ResolutionTextBox.Text);
            FactorizationSparsityPatternTest.interpolation = InterpolationCheckBox.IsChecked.Value;
            string filepath = fileTextBox.Text;
            Task.Factory.StartNew(() =>
            {
                try
                {
                    FactorizationSparsityPatternTest.Run(filepath);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            });
        }
        
        private void RunBenchmarkButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    FactorizationBenchmarksTest.Run(false);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            });
        }
        
        private void RunBenchmarkParallelButton_Click(object sender, RoutedEventArgs e)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    FactorizationBenchmarksTest.Run(true);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                }
            });
        }
        
        private void FileChooser_Click(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();


            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text Files (*.txt,*.mtx)|*.txt;*.mtx";


            // Display OpenFileDialog by calling ShowDialog method 
            Nullable<bool> result = dlg.ShowDialog();


            // Get the selected file name and display in a TextBox 
            if (result == true)
            {
                // Open document 
                string filename = dlg.FileName;
                fileTextBox.Text = filename;
            }
        }
        
        private void ResolutionValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}