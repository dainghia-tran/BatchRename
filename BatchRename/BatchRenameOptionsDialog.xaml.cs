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
    /// Interaction logic for BatchRenameOptionsDialog.xaml
    /// </summary>
    public partial class BatchRenameOptionsDialog : Window
    {
        public int DuplicateOption;
        public BatchRenameOptionsDialog(int DuplicateOptions)
        {
            InitializeComponent();
            if (DuplicateOptions == 1)
            {
                CaseRadioButton1.IsChecked = true;
            }
            else
            {
                CaseRadioButton2.IsChecked = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CaseRadioButton1.IsChecked == true)
            {
                DuplicateOption = 1;
            }
            else
            {
                DuplicateOption = 0;
            }
            DialogResult = true;
            Close();
        }
    }
}
