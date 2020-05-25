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
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using DiZhuaChart.NET.Class_API;
using DiZhuaChart.NET.Templates;
using System.IO;

namespace DiZhuaChart.NET
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dzapi api;
        public static bool Logined = false;
        private static readonly string DocPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), @"DZChart");
        private static readonly string AuthPath = Path.Combine(DocPath, @"authorization.json");
        private static readonly string HtmlPath = Path.Combine(DocPath, @"chart.html");

        public MainWindow()
        {
            InitializeComponent();

            if (!Directory.Exists(DocPath))
            {
                Directory.CreateDirectory(DocPath);
            }
            
            if (File.Exists(AuthPath))
            {
                Logined = true;
                api = new Dzapi(JsonConvert.DeserializeObject<JObject>(File.ReadAllText(AuthPath))["jwt"].ToString());
                tb_Status.Text = "帐号状态：已登录";
                InitUserInfo();
            }
            else
            {
                tb_Status.Text = "帐号状态：未登录";
                Login();
            }

            if (Logined)
            {
                api = new Dzapi(JsonConvert.DeserializeObject<JObject>(File.ReadAllText(AuthPath))["jwt"].ToString());
                tb_Status.Text = "帐号状态：已登录";
                InitUserInfo();
            }
            else
            {
                MessageBox.Show("未登录，请重新打开应用登录");
                this.Close();
            }
            btn_analyze.Click += Analyze;
            
        }

        private void Analyze(object sender, RoutedEventArgs e)
        {
            var parties = api.GetPartiesInfo();
            var friends = api.GetRelationships();
            var best_friend = (from friend in friends orderby friend.close_grade descending select friend).First();
            var pie_nickname = new PieChart("昵称", Analyzer.AnalyzeNickname(parties), "常用昵称Top20");
            var pie_name = new PieChart("局数", Analyzer.AnalyzeParties(parties), $"共参加 {parties.Count()} 局");
            var pie_friends = new PieChart("爪级", Analyzer.AnalyzeFriends(friends), $"共有 {friends.Count()} 爪友 其中 {best_friend.nickname} 和你最亲密");
            new ChartPage().Add(pie_name).Add(pie_friends).Add(pie_nickname).Render(HtmlPath);
            System.Diagnostics.Process.Start(HtmlPath);
        }

        private void Login()
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
        }

        private void InitUserInfo()
        {
            var ht = api.GetUserInfo();
            string nickname = ht["nickname"].ToString();
            string exp = ht["topic_exp_info"].ToString();
            string gift = ht["total_gift"].ToString();
            string uid = ht["uid"].ToString();
            bool parlor = (bool)ht["enable_parlor"];
            tb_nickname.Text = nickname;
            tb_exp.Text = exp;
            tb_gift.Text = $"投喂 {gift}";
            if (parlor)
            {
                tb_parlor.Text = "客厅主人";
            }
            else
            {
                tb_parlor.Text = "普通爪er";
            }
            img_avatar.Source = new BitmapImage(new Uri(ht["avatar"].ToString()));
            tb_Status.Text += $"\tuid：{uid}";
        }


        
    }
}
