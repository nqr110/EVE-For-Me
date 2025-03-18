using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace EVE_For_Me
{

    public partial class Form1 : Form
    {

        // -------------------------------------------------------------------------------------
        // 全局初始化部分
        private readonly string Tablpage3Url = "https://www.ceve-market.org/home/";
        // -------------------------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();
            // 注册 tab 切换事件（Tabpage3延迟初始化使用）
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // 切换Form2页面
        private void button1_Click(object sender, EventArgs e)
        {
            // 创建实例并传参
            Form2 form2 = new Form2(this);

            // 显示Form2
            form2.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {

        }
        // WebView2 控件声明
        private WebView2 webViewTab3; 
        private async void InitializeWebViewForTab3()
        {
            if (webViewTab3 == null)
            {
                webViewTab3 = new WebView2();
                webViewTab3.Dock = DockStyle.Fill;
                tabPage3.Controls.Add(webViewTab3);

                try
                {
                    await webViewTab3.EnsureCoreWebView2Async(null);
                    webViewTab3.CoreWebView2.Navigate(Tablpage3Url); // 替换成你的目标网址
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"初始化失败: {ex.Message}");
                }
            }
        }

        // 延迟初始化
        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (tabControl1.SelectedTab == tabPage3 && webViewTab3 == null)
            {
                InitializeWebViewForTab3();
            }
        }
        // 重置 WebView2 控件
        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (webViewTab3 != null)
                {
                    tabPage3.Controls.Remove(webViewTab3);
                    webViewTab3.Dispose();
                    webViewTab3 = null;
                }

                InitializeWebViewForTab3();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"重置失败: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}