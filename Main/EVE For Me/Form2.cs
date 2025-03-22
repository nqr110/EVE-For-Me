using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Net.Http;
using Newtonsoft.Json;

// 在Form2.cs文件顶部的using区域添加
using DrawColor = System.Drawing.Color;  // 给System.Drawing.Color定义别名


namespace EVE_For_Me
{

    // 声明此窗体仅支持Windows平台
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]

    public partial class Form2 : Form
    {
        // -------------------------------------------------------------------------------------
        // 初始化
        private readonly Form1 _form1;
        // 在类级别声明当前用户控件引用
        private UserControl currentUserControl;
        // -------------------------------------------------------------------------------------
        // 通用的用户控件加载方法
        private void LoadUserControl<T>() where T : UserControl, new()
        {
            panel1.Controls.Clear();

            var control = new T();
            control.Dock = DockStyle.Fill;
            panel1.Controls.Add(control);

            currentUserControl?.Dispose(); // 释放之前的控件
            currentUserControl = control;
        }
        // -------------------------------------------------------------------------------------
        // 左侧按钮控件切换
        private void button2_Click(object sender, EventArgs e)
        {
            LoadUserControl<UserControl3>();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadUserControl<UserControl1>();
        }
        // -------------------------------------------------------------------------------------
        // 布局初始化方法
        private void InitializeLayout()
        {
            // 设置左侧按钮容器属性
            panel2.Width = 120;  // 固定宽度（可根据需求调整）
            panel2.Dock = DockStyle.Left;
            panel2.BackColor = SystemColors.Control;

            // 设置右侧内容面板属性
            //panel1.Dock = DockStyle.Fill;
            //panel1.BackColor = DrawColor.White;
            // 调整控件层级顺序（确保panel1在panel2之后加载）
            Controls.SetChildIndex(panel1, 0);
            Controls.SetChildIndex(panel2, 1);

            //// 设置按钮在panel2内的布局
            //int buttonHeight = 50;
            //int spacing = 10;
            //foreach (System.Windows.Forms.Control ctrl in panel2.Controls)
            //{
            //    if (ctrl is Button btn)
            //    {
            //        btn.Height = buttonHeight;
            //        btn.Dock = DockStyle.Top;
            //        btn.BringToFront();
            //    }
            //}
            //// 添加间隔（可选）
            //panel2.Controls.Add(new Panel { Height = spacing, Dock = DockStyle.Top });
        }

        // -------------------------------------------------------------------------------------
        public Form2(Form1 form1)
        {
            InitializeComponent();
            // 布局初始化方法
            InitializeLayout();
            // 正确接收参数
            _form1 = form1;
            // 关闭本窗口后可以显示Form1
            this.FormClosed += (s, args) => _form1.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // 加载默认用户控件
            LoadUserControl<UserControl1>();
        }

        private void Form2_Close(object sender, FormClosedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadUserControl<UserControl4>();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}