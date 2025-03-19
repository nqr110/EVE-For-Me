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


namespace EVE_For_Me
{
    public partial class Form2 : Form
    {
        // -------------------------------------------------------------------------------------
        // 初始化
        private Form1 _form1;
        private const string ExcelPath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\evedata.xlsx"; // 修改为实际Excel路径
        // -------------------------------------------------------------------------------------

        public Form2(Form1 form1)
        {
            InitializeComponent();

            // 正确接收参数
            _form1 = form1;
            // 关闭本窗口后可以显示Form1
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var itemName = textBox1.Text.Trim();
                if (string.IsNullOrEmpty(itemName))
                {
                    label1.Text = "请输入物品名称";
                    return;
                }

                var typeId = FindTypeIdByName(itemName);
                label1.Text = typeId > 0 ? $"TypeID: {typeId}" : "未找到该物品";
            }
            catch (Exception ex)
            {
                label1.Text = $"错误: {ex.Message}";
            }
        }

        private int FindTypeIdByName(string itemName)
        {
            using var doc = SpreadsheetDocument.Open(ExcelPath, false);
            var workbookPart = doc.WorkbookPart;
            var worksheetPart = workbookPart.WorksheetParts.First();
            var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1); // 跳过标题行

            foreach (var row in rows)
            {
                var cells = row.Elements<Cell>().ToList();

                // 假设：
                // 第1列 (A列) 是 TypeID
                // 第2列 (B列) 是物品名称
                if (cells.Count >= 2 &&
                    GetCellValue(workbookPart, cells[1]).Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return int.Parse(GetCellValue(workbookPart, cells[0]));
                }
            }
            return -1;
        }

        private string GetCellValue(WorkbookPart workbookPart, Cell cell)
        {
            if (cell?.CellValue == null) return string.Empty;

            var value = cell.CellValue.Text;
            if (cell.DataType?.Value == CellValues.SharedString)
            {
                return workbookPart.SharedStringTablePart?
                    .SharedStringTable
                    .ElementAt(int.Parse(value))
                    .InnerText ?? string.Empty;
            }
            return value;
        }


        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
