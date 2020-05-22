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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DiZhuaChart.NET.Class_API;
using DiZhuaChart.NET.Templates;

namespace DiZhuaChart.NET
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Dzapi api = new Dzapi(@"eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJpc3MiOiJpb3MiLCJpYXQiOjE1ODgwNDcyMTIsImV4cCI6MTU4ODY1MjAxMiwiYXVkIjoiaHR0cHM6Ly9hcGlzLWZmLnphaWguY24vZmxhc2gtYXV0aC92MS9vYXV0aC9qd3QiLCJzdWIiOiJhMWpmYXlsNHpwIiwic2NvcGVzIjpbIm9wZW4iLCJyZWdpc3RlciIsImxvZ2luIl19.2rLFpfQjIoNgKDNQey7cv46jCgoF7iM8Wo8XPKrq-X0");
            foreach(var i in api.GetPartiesInfo())
            {
                textBox.Text += i.name + "\n";
            }
        }

    }
}
