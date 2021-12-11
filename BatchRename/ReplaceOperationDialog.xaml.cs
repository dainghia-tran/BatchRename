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
    /// Interaction logic for ReplaceStringDialog.xaml
    /// </summary>
    public partial class ReplaceStringDialog : Window
    {
        public delegate void OptArgsDelegate (string From, string To);
        public event OptArgsDelegate OptArgsChange = null;
        ReplaceArgs args;

        public ReplaceStringDialog(StringArgs Arguments)
        {
            InitializeComponent();
            args = Arguments as ReplaceArgs;
            FromTextBox.Text = args.From;
            ToTextBox.Text = args.To;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (OptArgsChange != null)
            {
                OptArgsChange.Invoke(FromTextBox.Text, ToTextBox.Text);
            }
            DialogResult = true;
            Close();
        }
    }
}
