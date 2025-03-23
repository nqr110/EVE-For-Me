namespace EVE_For_Me
{
    partial class UserControl4
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new System.Windows.Forms.Label();
            tabControl1 = new System.Windows.Forms.TabControl();
            tabPage1 = new System.Windows.Forms.TabPage();
            label3 = new System.Windows.Forms.Label();
            tabPage2 = new System.Windows.Forms.TabPage();
            comboBox1 = new System.Windows.Forms.ComboBox();
            label4 = new System.Windows.Forms.Label();
            button2 = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            tabPage3 = new System.Windows.Forms.TabPage();
            comboBox2 = new System.Windows.Forms.ComboBox();
            button3 = new System.Windows.Forms.Button();
            label8 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            tabPage4 = new System.Windows.Forms.TabPage();
            comboBox3 = new System.Windows.Forms.ComboBox();
            button1 = new System.Windows.Forms.Button();
            label11 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            label9 = new System.Windows.Forms.Label();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.LightGray;
            label1.Font = new System.Drawing.Font("楷体", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
            label1.Location = new System.Drawing.Point(10, 10);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(180, 33);
            label1.TabIndex = 1;
            label1.Text = "矿石小帮手";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            tabControl1.Font = new System.Drawing.Font("楷体", 15F);
            tabControl1.ItemSize = new System.Drawing.Size(100, 25);
            tabControl1.Location = new System.Drawing.Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(1000, 600);
            tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label1);
            tabPage1.Font = new System.Drawing.Font("楷体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
            tabPage1.Location = new System.Drawing.Point(4, 29);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new System.Windows.Forms.Padding(3);
            tabPage1.Size = new System.Drawing.Size(992, 567);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "帮助";
            tabPage1.UseVisualStyleBackColor = true;
            tabPage1.Click += tabPage1_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
            label3.Location = new System.Drawing.Point(24, 64);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(187, 56);
            label3.TabIndex = 3;
            label3.Text = "自述文件：\r\n    1. 显示当天矿价";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(comboBox1);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label2);
            tabPage2.Font = new System.Drawing.Font("楷体", 26.25F);
            tabPage2.Location = new System.Drawing.Point(4, 29);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new System.Windows.Forms.Padding(3);
            tabPage2.Size = new System.Drawing.Size(992, 567);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "普矿";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // comboBox1
            // 
            comboBox1.Font = new System.Drawing.Font("楷体", 15F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "Common", "Compression" });
            comboBox1.Location = new System.Drawing.Point(659, 124);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new System.Drawing.Size(121, 28);
            comboBox1.TabIndex = 6;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            label4.Location = new System.Drawing.Point(659, 84);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(89, 20);
            label4.TabIndex = 5;
            label4.Text = "Datetime";
            label4.Click += label4_Click_1;
            // 
            // button2
            // 
            button2.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            button2.Location = new System.Drawing.Point(659, 42);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(111, 30);
            button2.TabIndex = 4;
            button2.Text = "再次刷新";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            label5.Location = new System.Drawing.Point(187, 13);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(58, 24);
            label5.TabIndex = 3;
            label5.Text = "价格";
            label5.Click += label5_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            label2.Location = new System.Drawing.Point(16, 13);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(58, 24);
            label2.TabIndex = 0;
            label2.Text = "Name";
            label2.Click += label2_Click;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(comboBox2);
            tabPage3.Controls.Add(button3);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(label7);
            tabPage3.Controls.Add(label6);
            tabPage3.Font = new System.Drawing.Font("楷体", 26.25F);
            tabPage3.Location = new System.Drawing.Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(992, 567);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "冰矿";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // comboBox2
            // 
            comboBox2.Font = new System.Drawing.Font("楷体", 15F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "Common", "Compression" });
            comboBox2.Location = new System.Drawing.Point(659, 124);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new System.Drawing.Size(121, 28);
            comboBox2.TabIndex = 4;
            // 
            // button3
            // 
            button3.Font = new System.Drawing.Font("楷体", 15F);
            button3.Location = new System.Drawing.Point(659, 42);
            button3.Name = "button3";
            button3.Size = new System.Drawing.Size(111, 30);
            button3.TabIndex = 3;
            button3.Text = "再次刷新";
            button3.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new System.Drawing.Font("楷体", 15F);
            label8.Location = new System.Drawing.Point(659, 84);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(89, 20);
            label8.TabIndex = 2;
            label8.Text = "Datetime";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new System.Drawing.Font("楷体", 18F);
            label7.Location = new System.Drawing.Point(187, 13);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(58, 24);
            label7.TabIndex = 1;
            label7.Text = "价格";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new System.Drawing.Font("楷体", 18F);
            label6.Location = new System.Drawing.Point(16, 13);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(58, 24);
            label6.TabIndex = 0;
            label6.Text = "Name";
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(comboBox3);
            tabPage4.Controls.Add(button1);
            tabPage4.Controls.Add(label11);
            tabPage4.Controls.Add(label10);
            tabPage4.Controls.Add(label9);
            tabPage4.Font = new System.Drawing.Font("楷体", 26.25F);
            tabPage4.Location = new System.Drawing.Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new System.Drawing.Size(992, 567);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "卫星矿";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // comboBox3
            // 
            comboBox3.Font = new System.Drawing.Font("楷体", 15F);
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "Common", "Compression" });
            comboBox3.Location = new System.Drawing.Point(659, 124);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new System.Drawing.Size(121, 28);
            comboBox3.TabIndex = 4;
            // 
            // button1
            // 
            button1.Font = new System.Drawing.Font("楷体", 15F);
            button1.Location = new System.Drawing.Point(659, 42);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(111, 30);
            button1.TabIndex = 3;
            button1.Text = "再次刷新";
            button1.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new System.Drawing.Font("楷体", 15F);
            label11.Location = new System.Drawing.Point(659, 84);
            label11.Name = "label11";
            label11.Size = new System.Drawing.Size(89, 20);
            label11.TabIndex = 2;
            label11.Text = "Datetime";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new System.Drawing.Font("楷体", 18F);
            label10.Location = new System.Drawing.Point(187, 13);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(58, 24);
            label10.TabIndex = 1;
            label10.Text = "价格";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("楷体", 18F);
            label9.Location = new System.Drawing.Point(16, 13);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(58, 24);
            label9.TabIndex = 0;
            label9.Text = "Name";
            label9.Click += label9_Click;
            // 
            // UserControl4
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(tabControl1);
            Name = "UserControl4";
            Size = new System.Drawing.Size(1000, 600);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.ComboBox comboBox3;
    }
}
