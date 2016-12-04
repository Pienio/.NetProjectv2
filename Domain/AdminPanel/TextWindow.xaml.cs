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
using System.Windows.Shapes;
using AdminPanel.DataServiceReference;
using MahApps.Metro.Controls;

namespace AdminPanel
{
    /// <summary>
    /// Interaction logic for DescriptionWindow.xaml
    /// </summary>
    public partial class TextWindow : MetroWindow
    {
        public string TextInserted
        {
            get { return (string)GetValue(TextInsertedProperty); }
            set { SetValue(TextInsertedProperty, value); }
        }
        // Using a DependencyProperty as the backing store for TextInserted.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextInsertedProperty =
            DependencyProperty.Register("TextInserted", typeof(string), typeof(TextWindow), new PropertyMetadata(""));
        
        public TextWindow()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void accept_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextInserted))
            {
                MessageBox.Show("Tekst nie może być pusty.", App.Name, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
