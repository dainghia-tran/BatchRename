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
    /// Interaction logic for MoveOperationDialog.xaml
    /// </summary>
    public partial class MoveOperationDialog : Window
    {
        public delegate void StringArgsDelegate(MoveArgs ChangedArgs);
        public event StringArgsDelegate StringArgsChange = null;

        public MoveOperationDialog(StringArgs Arguments)
        {
            InitializeComponent();
            MoveArgs args = Arguments as MoveArgs;
            Num.Text = args.Number.ToString();
            if (args.Mode == "Front")
            {
                RadioButton1.IsChecked = true;
            }
            else
            {
                RadioButton2.IsChecked = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            MoveArgs result = new MoveArgs();
            result.Number = int.Parse(Num.Text);
            if (RadioButton1.IsChecked == true)
            {
                result.Mode = RadioButton1.Content.ToString();
            }
            else
            {
                result.Mode = RadioButton2.Content.ToString();
            }
            if (StringArgsChange != null)
            {
                StringArgsChange.Invoke(result);
            }
            DialogResult = true;
            Close();
        }
    }
}
