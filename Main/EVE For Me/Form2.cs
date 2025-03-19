using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVE_For_Me
{
    public partial class Form2 : Form
    {
        // -------------------------------------------------------------------------------------
        // 初始化
        private Form1 _form1;
        // -------------------------------------------------------------------------------------

        public Form2(Form1 form1)
        {
            InitializeComponent();

            // 正确接收参数
            _form1 = form1;

            this.FormClosed += (s, args) => _form1.Show();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_Close(object sender, FormClosedEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
