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
    /// Interaction logic for ChangeCaseDialog.xaml
    /// </summary>
    public partial class ChangeCaseDialog : Window
    {
        public delegate void OptArgsDelegate(string ChosenCase);
        public event OptArgsDelegate OptArgsChange = null;
        CaseArgs args;

        public ChangeCaseDialog(StringArgs Arguments)
        {
            InitializeComponent();
            args = Arguments as CaseArgs;

            if (args.Case == "Lower")
            {
                CaseRadioButton1.IsChecked = true;
            }

            if (args.Case == "Upper")
            {
                CaseRadioButton2.IsChecked = true;
            }

            if (args.Case == "Upper First Letter")
            {
                CaseRadioButton3.IsChecked = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string result = "";
            if (CaseRadioButton1.IsChecked == true)
            {
                result = (string)CaseRadioButton1.Content;
            }

            if (CaseRadioButton2.IsChecked == true)
            {
                result = (string)CaseRadioButton2.Content;
            }

            if (CaseRadioButton3.IsChecked == true)
            {
                result = (string)CaseRadioButton3.Content;
            }
            if (OptArgsChange != null)
            {
                OptArgsChange.Invoke(result);
            }
            DialogResult = true;
            Close();
        }
    }
}
