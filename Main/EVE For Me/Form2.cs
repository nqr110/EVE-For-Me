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


namespace EVE_For_Me
{

    // 声明此窗体仅支持Windows平台
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]

    public partial class Form2 : Form
    {
        // -------------------------------------------------------------------------------------
        // 初始化
        private readonly Form1 _form1;
        // 修改为实际Excel路径
        private const string ExcelPath = @"..\..\..\..\EVE For Me\Database\evedata.xlsx";
        // 添加新变量存储当前TypeID
        private int currentTypeId = -1;
        // 添加HttpClient实例
        private readonly HttpClient _httpClient = new();
        // -------------------------------------------------------------------------------------
        private static string GetCellValue(WorkbookPart workbookPart, Cell cell)
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
        private static int FindTypeIdByName(string itemName)
        {
            using var doc = SpreadsheetDocument.Open(ExcelPath, false);
            var workbookPart = doc.WorkbookPart;
            var worksheetPart = workbookPart.WorksheetParts.First();
            var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1);

            foreach (var row in rows)
            {
                var cells = row.Elements<Cell>().ToList();
                if (cells.Count >= 2 &&
                    GetCellValue(workbookPart, cells[1]).Equals(itemName, StringComparison.OrdinalIgnoreCase))
                {
                    return int.Parse(GetCellValue(workbookPart, cells[0]));
                }
            }
            return -1;
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                var itemName = textBox1.Text.Trim();
                if (string.IsNullOrEmpty(itemName))
                {
                    label1.Text = "请输入物品名称";
                    currentTypeId = -1;
                    return;
                }

                var typeId = FindTypeIdByName(itemName);
                if (typeId > 0)
                {
                    currentTypeId = typeId;
                    label1.Text = $"TypeID: {typeId}";
                }
                else
                {
                    currentTypeId = -1;
                    label1.Text = "未找到该物品";
                }
            }
            catch (Exception ex)
            {
                label1.Text = $"错误: {ex.Message}";
                currentTypeId = -1;
            }
        }


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






        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (currentTypeId <= 0)
                {
                    label2.Text = "请先获取有效TypeID";
                    label3.Text = "";
                    return;
                }

                string apiUrl = $"https://www.ceve-market.org/api/market/region/10000002/type/{currentTypeId}.json";

                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                label2.Text = $"Buy Max: {data.buy.max}";
                label3.Text = $"Sell Min: {data.sell.min}";
            }
            catch (HttpRequestException ex)
            {
                label2.Text = $"网络请求失败: {ex.Message}";
                label3.Text = "";
            }
            catch (Exception ex)
            {
                label2.Text = $"发生错误: {ex.Message}";
                label3.Text = "";
            }
        }
    }
}