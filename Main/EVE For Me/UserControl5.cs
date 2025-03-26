using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Web.WebView2.WinForms;

namespace EVE_For_Me
{
    public partial class UserControl5 : UserControl
    {
        private WebView2 webView;
        private string _url = "https://www.ceve-market.org/home/";

        public UserControl5()
        {
            InitializeComponent();
            InitializeWebView();
            this.Disposed += (_, __) => DisposeWebView();
        }

        [Browsable(true)]
        [Category("Web Settings")]
        [Description("设置要加载的网页URL")]
        public string TargetUrl
        {
            get => _url;
            set => _url = value;
        }

        private void InitializeWebView()
        {
            webView = new WebView2
            {
                Dock = DockStyle.Fill,
                Visible = false,
                CreationProperties = new CoreWebView2CreationProperties()
                {
                    UserDataFolder = System.IO.Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "EVE_For_Me\\WebView2Cache")
                }
            };
            Controls.Add(webView);
        }

        private async void UserControl5_Load(object sender, EventArgs e)
        {
            try
            {
                if (webView.CoreWebView2 == null)
                {
                    await webView.EnsureCoreWebView2Async(null);
                }
                webView.CoreWebView2.Navigate(_url);
                webView.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"网页加载失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                DisposeWebView();
            }
        }

        public async void ReloadWebView()
        {
            if (webView != null && !webView.IsDisposed)
            {
                try
                {
                    await webView.EnsureCoreWebView2Async(null);
                    webView.CoreWebView2.Navigate(_url);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"重新加载失败: {ex.Message}", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DisposeWebView()
        {
            if (webView != null)
            {
                if (webView.InvokeRequired)
                {
                    webView.Invoke(new Action(() =>
                    {
                        webView.Dispose();
                        webView = null;
                    }));
                }
                else
                {
                    webView.Dispose();
                    webView = null;
                }
            }
        }
    }
}