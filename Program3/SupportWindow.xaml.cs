using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Program3
{
    /// <summary>
    /// Логика взаимодействия для SupportWindow.xaml
    /// </summary>
    public partial class SupportWindow : Window
    {
        public SupportWindow()
        {
            InitializeComponent();
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Отправлено!");
            this.Close();
        }
    }
}
