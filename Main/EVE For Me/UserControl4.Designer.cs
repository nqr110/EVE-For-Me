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
            button2 = new System.Windows.Forms.Button();
            label5 = new System.Windows.Forms.Label();
            button1 = new System.Windows.Forms.Button();
            label4 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            tabPage3 = new System.Windows.Forms.TabPage();
            tabPage4 = new System.Windows.Forms.TabPage();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
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
            tabControl1.Font = new System.Drawing.Font("楷体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 134);
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
            tabPage2.Controls.Add(button2);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(button1);
            tabPage2.Controls.Add(label4);
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
            // button2
            // 
            button2.Location = new System.Drawing.Point(506, 82);
            button2.Name = "button2";
            button2.Size = new System.Drawing.Size(147, 45);
            button2.TabIndex = 4;
            button2.Text = "开始";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            label5.Location = new System.Drawing.Point(308, 13);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(58, 24);
            label5.TabIndex = 3;
            label5.Text = "价格";
            // 
            // button1
            // 
            button1.Location = new System.Drawing.Point(506, 13);
            button1.Name = "button1";
            button1.Size = new System.Drawing.Size(147, 45);
            button1.TabIndex = 2;
            button1.Text = "开始";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            label4.Location = new System.Drawing.Point(163, 13);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(82, 24);
            label4.TabIndex = 1;
            label4.Text = "TypeID";
            label4.Click += label4_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("楷体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 134);
            label2.Location = new System.Drawing.Point(16, 13);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(106, 504);
            label2.TabIndex = 0;
            label2.Text = "灼烧岩\r\n干焦岩\r\n凡晶石\r\n双多特石\r\n斜长岩\r\n杰斯贝矿\r\n同位原矿\r\n水硼砂\r\n克洛基石\r\n奥贝尔石\r\n希莫非特\r\n艾克诺岩\r\n灰岩\r\n塔拉岩\r\n亮灰岩\r\n片麻岩\r\n拉克岩\r\n黑赭石\r\n条纹赭\r\n基腹断岩\r\n贝兹岩\r\n";
            label2.Click += label2_Click;
            // 
            // tabPage3
            // 
            tabPage3.Font = new System.Drawing.Font("楷体", 26.25F);
            tabPage3.Location = new System.Drawing.Point(4, 29);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new System.Drawing.Size(992, 567);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "冰矿";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Font = new System.Drawing.Font("楷体", 26.25F);
            tabPage4.Location = new System.Drawing.Point(4, 29);
            tabPage4.Name = "tabPage4";
            tabPage4.Size = new System.Drawing.Size(992, 567);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "卫星矿";
            tabPage4.UseVisualStyleBackColor = true;
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}
