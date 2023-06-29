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

namespace WpfApp1
{
    /// <summary>
    /// Setting.xaml 的交互逻辑
    /// </summary>
    public partial class Setting : Window
    {
        MainWindow? mainWindow = null;
        public Setting()
        {

            InitializeComponent();
            //mainWindow = this.Owner as MainWindow;
            Task task = Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("IsNull!");
                await Task.Delay(1000);
                if (this.Owner == null)
                {
                    Console.WriteLine("IsNull!");
                    Console.WriteLine(this.Owner);
                }
                else
                {
                    Console.WriteLine("Yes!");
                    Console.WriteLine(this.Owner);
                }

                if (mainWindow == null)
                {
                    return;
                }

                Console.WriteLine("Setting!");
                skipMilliseconds.Text = mainWindow.SkipMilliseconds.ToString();
            });

            

        }

        private void Save(object sender, RoutedEventArgs e)
        {
            if (mainWindow == null)
                return;
            double.TryParse(this.skipMilliseconds.Text, out double num);
            mainWindow.SkipMilliseconds = num;
        }
    }
}
