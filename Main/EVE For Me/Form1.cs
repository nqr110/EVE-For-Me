﻿using System;
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

    // 声明此窗体仅支持Windows平台
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]

    public partial class Form1 : Form
    {
        // -------------------------------------------------------------------------------------
        // 全局初始化部分

        // 市场
        private readonly string ShiChangUrl = "https://www.ceve-market.org/home/";
        // 蓝图计算
        private readonly string LanTuUrl = "http://www.ceve-industry.cn/";
        // 合同货柜分析
        private readonly string HeTongUrl = "https://tools.ceve-market.org/contract/";
        // 跳跃导航计算
        private readonly string TiaoYueUrl = "https://eve.sgfans.org/navigator/jump_path_layout";

        // 使用字典管理所有需要延迟加载的Tab页
        private Dictionary<TabPage, (string Url, WebView2 WebView)> webViewTabs = new Dictionary<TabPage, (string, WebView2)>();

        // -------------------------------------------------------------------------------------

        public Form1()
        {
            InitializeComponent();
            // 注册 tab 切换事件（Tabpage3延迟初始化使用）
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            // 初始化字典配置
            // 可以在这里修改Page页所对应的Url
            webViewTabs.Add(tabPage3, (ShiChangUrl, null));
            webViewTabs.Add(tabPage4, (LanTuUrl, null));
            webViewTabs.Add(tabPage5, (TiaoYueUrl, null));
            webViewTabs.Add(tabPage6, (HeTongUrl, null));
        }

        // -------------------------------------------------------------------------------------

        // 统一的Tab页切换处理
        // 统一在这里延迟初始化
        private async void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var currentTab = tabControl1.SelectedTab;

            if (webViewTabs.TryGetValue(currentTab, out var tabInfo))
            {
                if (tabInfo.WebView == null)
                {
                    await InitializeWebView(currentTab, tabInfo.Url);
                }
            }
        }

        // 通用的WebView初始化方法
        private async Task InitializeWebView(TabPage tabPage, string url)
        {
            var webView = new WebView2
            {
                Dock = DockStyle.Fill,
                Visible = false  // 初始隐藏防止闪烁
            };

            try
            {
                tabPage.SuspendLayout();
                tabPage.Controls.Add(webView);
                await webView.EnsureCoreWebView2Async(null);
                webView.CoreWebView2.Navigate(url);

                // 更新字典中的WebView实例
                webViewTabs[tabPage] = (url, webView);
                webView.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"初始化{tabPage.Text}失败: {ex.Message}");
                tabPage.Controls.Remove(webView);
                webView.Dispose();
            }
            finally
            {
                tabPage.ResumeLayout();
            }
        }

        // 统一处理Tab页重新加载
        private async void ReloadTabPage(TabPage tabPage)
        {
            if (webViewTabs.TryGetValue(tabPage, out var tabInfo))
            {
                string url = tabInfo.Url;
                WebView2 webView = tabInfo.WebView;

                if (webView != null)
                {
                    try
                    {
                        // 确保WebView2核心已初始化
                        if (webView.CoreWebView2 == null)
                        {
                            await webView.EnsureCoreWebView2Async(null);
                        }
                        webView.CoreWebView2.Navigate(url);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"重新加载{tabPage.Text}失败: {ex.Message}");
                    }
                }
                else
                {
                    // 如果WebView未初始化，则执行初始化流程
                    await InitializeWebView(tabPage, url);
                }
            }
        }

        // 修改按钮点击事件处理程序：

        // button2对应tabPage3（市场）
        private void button2_Click(object sender, EventArgs e)
        {
            ReloadTabPage(tabPage3);
        }

        // button5对应tabPage4（蓝图计算）
        private void button5_Click(object sender, EventArgs e)
        {
            ReloadTabPage(tabPage4);
        }

        // button7对应tabPage5（跳跃导航）
        private void button7_Click(object sender, EventArgs e)
        {
            ReloadTabPage(tabPage5);
        }

        // button8对应tabPage6（合同分析）
        private void button8_Click(object sender, EventArgs e)
        {
            ReloadTabPage(tabPage6);
        }

        // -------------------------------------------------------------------------------------

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

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage4;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage5;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage6;
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }
    }
}
