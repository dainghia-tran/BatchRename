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

namespace BatchRename
{
    /// <summary>
    /// Interaction logic for ChangesAlertDialog.xaml
    /// </summary>
    public partial class ChangesAlertDialog : Window
    {
        List<String> changedPaths;
        public ChangesAlertDialog(List<String> input)
        {
            InitializeComponent();
            changedPaths = new List<String>(input);
            ChangedFilesList.ItemsSource = changedPaths;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
