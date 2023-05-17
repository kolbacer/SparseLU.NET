using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ResolutionTextBox.Text = "100";
        }
        
        private void RunSparsityPatternAnalyzerButton_Click(object sender, RoutedEventArgs e)
        {
            FactorizationSparsityPatternTest.resolution = UInt32.Parse(ResolutionTextBox.Text);
            FactorizationSparsityPatternTest.interpolation = InterpolationCheckBox.IsChecked.Value;
            FactorizationSparsityPatternTest.Run(fileTextBox.Text);
        }
        
        private void RunBenchmarkButton_Click(object sender, RoutedEventArgs e)
        {
            FactorizationBenchmarksTest.Run(false);
        }
        
        private void RunBenchmarkParallelButton_Click(object sender, RoutedEventArgs e)
        {
            FactorizationBenchmarksTest.Run(true);
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