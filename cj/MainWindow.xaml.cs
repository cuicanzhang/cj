using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace cj
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Core.SqlAction.AddH(initDic(dt + qh, jh));
            InitializeComponent();
            conn.Init();

            zcwresult();
            //caijingwang();

            //htmlRTB.AppendText(zhcw());

        }
        private void zcwresult()
        {
            JObject init_result = JsonConvert.DeserializeObject(zhcw()) as JObject;
            foreach (JObject j in init_result["list"])
            {
                string qh = "20"+j["issue"].ToString();
                string jh = j["winNum"].ToString().Replace(",", "");

                Core.SqlAction.AddH(initDic(qh, jh));
                //htmlRTB.AppendText("[ QH"+qh+"+  JH"+jh+"  ]插入完成");
                //MessageBox.Show(qh+":"+jh);
            }
        }
        private string caijingwang()
        {
            string url = "https://zst.cjcp.com.cn/cjwk3/view/kuai3_zonghe-jilin-3-70000.html";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.AllowAutoRedirect = false;
            req.ContentType = "application/x-www-form-urlencoded";
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            string html=new StreamReader (res.GetResponseStream()).ReadToEnd();
            //return html;
            //Regex reg = new Regex(@"\d{8,}</td><td class='z_bg_13'>\d{3,}");
            //Match match = reg.Match(html);
            string pattern = @"\d{8,}</td><td class='z_bg_13'>\d{3,}";
            MatchCollection result = Regex.Matches(html, pattern);
            //birthdayMonthCB.SelectedValue = match.Groups[1].Value;
            //birthdayDayCB.SelectedValue = match.Groups[2].Value;
            //MessageBox.Show(match.Groups[10].Value);
            //MessageBox.Show(match.Groups[1].Value);
            using (SQLiteConnection conn = new SQLiteConnection(config.DataSource))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    conn.Open();
                    cmd.Connection = conn;
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    sh.BeginTransaction();
                    foreach (var str in result)
                    {
                        string[] sArray = Regex.Split(str.ToString(), "</td><td class='z_bg_13'>", RegexOptions.IgnoreCase);
                        // MessageBox.Show(sArray[0]);
                        //MessageBox.Show(sArray[1]);

                        string qh = sArray[0].ToString();
                        string jh = sArray[1].ToString();
                        sh.Insert("fcjlk3", initDic(qh, jh));
                       // Core.SqlAction.AddH(initDic(qh, jh));

                    }
                    sh.Commit();

                    conn.Close();
                }
            }

            


            return html;

        }

        private string  zhcw()
        {
            
            string url = "http://data.zhcw.com/k3/index.php?act=kstb&provinceCode=22";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.AllowAutoRedirect = false;
            req.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            string cookies = res.Headers.Get("Set-Cookie");
            //string html=new StreamReader (res.GetResponseStream()).ReadToEnd();
            //return html;
            //MessageBox.Show(cookies);
            //string jsonUrl = "http://data.zhcw.com/port/client_json.php?transactionType=10120105&type=zongzs&daytime=today&timeId=-1";
            string jsonUrl = "http://data.zhcw.com/port/client_json.php?transactionType=10120105&type=zongzs&period=100&timeId=-1";
            HttpWebRequest req1 = (HttpWebRequest)WebRequest.Create(jsonUrl);
            req1.Method = "GET";
            req1.AllowAutoRedirect = false;
            req1.ContentType = "application/x-www-form-urlencoded";
            req1.CookieContainer = new CookieContainer();
            req1.CookieContainer.SetCookies(req1.RequestUri, cookies);
            HttpWebResponse res1 = (HttpWebResponse)req1.GetResponse();
            string html = new StreamReader(res1.GetResponseStream()).ReadToEnd();
            return html;
        }
        private Dictionary<string, object> initDic(string qh, string jh)
        {

            var dic = new Dictionary<string, object>();
            dic["qh"] = qh;
            dic["jh"] = jh;
            return dic;
        }

        private void dispBtn_Click(object sender, RoutedEventArgs e)
        {
            dispDG.ItemsSource = Core.SqlAction.SelectH("").DefaultView;
            dispDG.GridLinesVisibility = DataGridGridLinesVisibility.All;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (searchStr.Text.Replace(" ", "") != "")
            {
                dispDG.ItemsSource = Core.SqlAction.SelectOH(searchStr.Text.Replace(" ", ""), qhCountTB.Text.Replace(" ", "")).DefaultView;
                dispDG.GridLinesVisibility = DataGridGridLinesVisibility.All;
            }
             */   
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (qhCountTB.Text.Replace(" ", "") != "")
            {
                CountDG.ItemsSource = Core.SqlAction.SelectMaxCount(qhCountTB.Text.Replace(" ", "")).DefaultView;
                CountDG.GridLinesVisibility = DataGridGridLinesVisibility.All;
            
            }
            
        }
        private void checkNumber_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!Tools.isInputNumber(e))
            {
                //MessageBox.Show("请输入数字！");
            }
        }

        private void CountDG_Sorting(object sender, DataGridSortingEventArgs e)
        {

        }
        private void CountDG_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView mySelectedElement = (DataRowView)CountDG.SelectedItem;
            if (mySelectedElement != null)
            {
                if (qhCountTB.Text.Replace(" ", "") != "")
                {
                    dispDG.ItemsSource = Core.SqlAction.SelectOH(mySelectedElement[0].ToString(), qhCountTB.Text).DefaultView;
                    dispDG.GridLinesVisibility = DataGridGridLinesVisibility.All;

                }

            }
        }
        private void DGLoadingRow(object sender, DataGridRowEventArgs e)
        {
            //加载行
            e.Row.Header = e.Row.GetIndex() + 1;
        }
    }
}
