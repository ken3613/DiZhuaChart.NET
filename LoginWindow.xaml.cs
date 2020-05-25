using DiZhuaChart.NET.Class_API;
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
using System.Windows.Threading;
using System.IO;
using Newtonsoft.Json;

namespace DiZhuaChart.NET
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        private DispatcherTimer timer;
        private int code_time = 60;

        public LoginWindow()
        {
            InitializeComponent();
            timer = new DispatcherTimer(DispatcherPriority.Render);
            timer.Tick += Code_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            btn_code.Click += Btn_code_Click;
        }

        private void Code_Tick(object sender, EventArgs e)
        {
            code_time--;
            btn_code.Content = $"{code_time.ToString()}s";
            if (code_time <= 0)
            {
                timer.Stop();
                btn_code.Content = "获取验证码";
                btn_code.IsEnabled = true;
            }
        }

        private void Btn_code_Click(object sender, RoutedEventArgs e)
        {
            Regex regex = new Regex(@"\d{11}");
            if (regex.IsMatch(tb_mobile.Text))
            {
                ((Button)sender).IsEnabled = false;
                code_time = 60;
                timer.Start();
                Dzapi.Verification(tb_mobile.Text);
            }
            else
            {
                MessageBox.Show("请输入正确的手机号码");
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            Regex re_mobile = new Regex(@"\d{11}");
            Regex re_code = new Regex(@"\d{6}");
            if(re_mobile.IsMatch(tb_mobile.Text) && re_code.IsMatch(tb_code.Text))
            {
                string jwt = Dzapi.Login(tb_mobile.Text, tb_code.Text);
                if (jwt != null)
                {
                    MainWindow.Logined = true;
                    WriteJwt(jwt);
                    this.Close();
                }
                else
                {
                    MessageBox.Show("验证码错误");
                    tb_code.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("请输入正确的验证码");
                tb_code.Text = string.Empty;
            }
        }

        private void WriteJwt(string jwt)
        {
            string AuthPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"DZChart\authorization.json");
            string json = JsonConvert.SerializeObject(new
            {
                jwt = jwt
            });
            File.WriteAllText(AuthPath, json);
        }
    }
}
