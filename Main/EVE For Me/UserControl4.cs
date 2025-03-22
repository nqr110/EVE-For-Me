using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVE_For_Me
{
    public partial class UserControl4 : UserControl
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string EvEDataPath = @"..\..\..\..\EVE For Me\Database\evedata.xlsx";
        private const string OrdinaryOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_ordinary ore.xlsx";

        public UserControl4()
        {
            InitializeComponent();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            label5.Text = "数据加载中...";

            try
            {
                var typeIds = ExcelHelper.ReadSecondColumnValues(OrdinaryOrePath);
                var results = new StringBuilder();

                foreach (int typeId in typeIds)
                {
                    try
                    {
                        var (sellMin, buyMax) = await MarketApi.GetMarketData(_httpClient, typeId);
                        results.AppendLine($"{typeId}: 最低售价 {sellMin} | 最高收购 {buyMax}");
                    }
                    catch (Exception ex)
                    {
                        results.AppendLine($"{typeId}: 错误 - {ex.Message}");
                    }

                    UpdateLabelSafe(results.ToString());
                }
            }
            catch (Exception ex)
            {
                UpdateLabelSafe($"初始化失败: {ex.Message}");
            }
            finally
            {
                button2.Enabled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var results = new StringBuilder();
            var oreNames = label2.Text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var name in oreNames)
            {
                int typeId = ExcelHelper.FindTypeIdByName(EvEDataPath, name.Trim());
                results.AppendLine(typeId > 0 ? $"{typeId}" : $"{name}：未找到");
            }

            label4.Text = results.ToString();
        }

        private void UpdateLabelSafe(string text)
        {
            if (label5.InvokeRequired)
            {
                label5.Invoke(new Action(() => label5.Text = text));
            }
            else
            {
                label5.Text = text;
            }
        }
    }

    // 公共帮助类
    public static class ExcelHelper
    {
        public static int FindTypeIdByName(string filePath, string itemName)
        {
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, false);
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
            catch
            {
                return -1;
            }
        }

        public static List<int> ReadSecondColumnValues(string filePath)
        {
            var values = new List<int>();
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, false);
                var workbookPart = doc.WorkbookPart;
                var worksheetPart = workbookPart.WorksheetParts.First();
                var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1);

                foreach (var row in rows)
                {
                    var cells = row.Elements<Cell>().ToList();
                    if (cells.Count >= 2)
                    {
                        var value = GetCellValue(workbookPart, cells[1]);
                        if (int.TryParse(value, out int result))
                        {
                            values.Add(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"读取Excel失败: {ex.Message}");
            }
            return values;
        }

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
    }

    // 市场API接口类
    public static class MarketApi
    {
        public static async Task<(string sellMin, string buyMax)> GetMarketData(HttpClient client, int typeId)
        {
            string apiUrl = $"https://www.ceve-market.org/api/market/region/10000002/type/{typeId}.json";
            var response = await client.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            return (
                sellMin: data.sell.min.ToString() ?? "N/A",
                buyMax: data.buy.max.ToString() ?? "N/A"
            );
        }
    }
}
