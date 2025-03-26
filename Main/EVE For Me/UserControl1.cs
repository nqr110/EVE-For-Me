// UserControl1.cs
using System;
using System.Windows.Forms;

namespace EVE_For_Me
{
    public partial class UserControl1 : UserControl
    {
        public event EventHandler<string> SelectedModeChanged;

        // 添加公共属性访问选择值
        public string SelectedMode
        {
            get { return comboBox1?.SelectedItem?.ToString() ?? "用户模式"; }
        }
        public UserControl1()
        {
            InitializeComponent();
            InitializeComboBox();
        }

        private void InitializeComboBox()
        {
            //comboBox1.Items.AddRange(new object[] { "用户模式", "API调试模式" });
            comboBox1.SelectedIndex = 0;
            comboBox1.SelectedIndexChanged += ComboBox1_SelectedIndexChanged;

        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedModeChanged?.Invoke(this, comboBox1.SelectedItem.ToString());
        }
        // 添加设计器引用的Load事件处理方法
        private void UserControl1_Load(object sender, EventArgs e)
        {
            // 可在此处添加初始化代码
        }
    }
}