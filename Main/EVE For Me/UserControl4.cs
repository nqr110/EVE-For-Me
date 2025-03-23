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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EVE_For_Me
{
    public partial class UserControl4 : UserControl
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private const string EvEDataPath = @"..\..\..\..\EVE For Me\Database\evedata.xlsx";
        // 普通矿石路径
        private const string OrdinaryOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_ordinary ore.xlsx";
        // 冰矿路径
        private const string GlacialRockPath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_glacial rock.xlsx";
        // 卫星矿路径
        private const string SatelliteOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_satellite ore.xlsx";
        private bool _isFirstLoad = true;  // 添加首次加载标记

        public UserControl4()
        {
            InitializeComponent();

            // 读取第一列名称
            var oreNames = ExcelHelper.ReadFirstColumnNames(OrdinaryOrePath);
            label2.Text = string.Join(Environment.NewLine, oreNames);
            // 订阅TabControl切换事件
            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;
        }


        // Tab页切换事件处理
        private async void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage2)
            {
                await ExecuteWebDataLoading(false); // 正常预加载
            }
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
            try
            {
                await ExecuteWebDataLoading(true); // 强制刷新
            }
            finally
            {
                button2.Enabled = true;
            }
        }


        // 加载功能
        private async Task ExecuteWebDataLoading(bool forceReload)
        {
            try
            {
                // 检查是否需要执行加载
                if (!forceReload && !_isFirstLoad)
                    return;

                // 更新首次加载标记（仅当非强制刷新时）
                if (!forceReload)
                    _isFirstLoad = false;

                UpdateLabelSafe(forceReload ? "正在刷新市场数据..." : "正在加载市场数据...");

                var typeIds = ExcelHelper.ReadSecondColumnValues(OrdinaryOrePath);

                // 使用并行处理
                var tasks = typeIds.Select(async typeId =>
                {
                    try
                    {
                        return (typeId, await MarketApi.GetMarketData(_httpClient, typeId));
                    }
                    catch (Exception ex)
                    {
                        return (typeId, (SellMin: $"错误", BuyMax: ex.Message));
                    }
                });

                var results = await Task.WhenAll(tasks);

                var sb = new StringBuilder();
                foreach (var result in results)
                {
                    sb.AppendLine($"{"最低售价",-6}{result.Item2.SellMin,-8} | {"最高收购",-6}{result.Item2.BuyMax,-8}");
                }

                UpdateLabelSafe(sb.ToString());

                // 可选：加载完成后更新时间
                UpdateTimeLabel(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                UpdateLabelSafe($"操作失败: {ex.Message}");
            }
        }


        // 新增时间更新方法
        private void UpdateTimeLabel(string time)
        {
            if (label4.InvokeRequired)
            {
                label4.BeginInvoke(new Action(() =>
                {
                    label4.Text = $"{time}";
                }));
            }
            else
            {
                label4.Text = $"{time}";
            }
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

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }

    // 公共帮助类
    public static class ExcelHelper
    {
        // 原始方法保持兼容
        public static List<int> ReadSecondColumnValues(string filePath)
        {
            return ReadNumericColumn(filePath, 1);
        }
        // 新增第一列读取方法
        public static List<string> ReadFirstColumnNames(string filePath)
        {
            return ReadTextColumn(filePath, 0);
        }

        // 专用数值列读取方法
        private static List<int> ReadNumericColumn(string filePath, int columnIndex)
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
                    if (cells.Count > columnIndex)
                    {
                        var value = GetCellValue(workbookPart, cells[columnIndex]);
                        if (int.TryParse(value, out var numericValue))
                        {
                            values.Add(numericValue);
                        }
                    }
                }
            }
            catch
            {
                // 保持原始错误处理风格
            }
            return values;
        }

        // 专用文本列读取方法
        private static List<string> ReadTextColumn(string filePath, int columnIndex)
        {
            var values = new List<string>();
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, false);
                var workbookPart = doc.WorkbookPart;
                var worksheetPart = workbookPart.WorksheetParts.First();
                var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1);

                foreach (var row in rows)
                {
                    var cells = row.Elements<Cell>().ToList();
                    if (cells.Count > columnIndex)
                    {
                        values.Add(GetCellValue(workbookPart, cells[columnIndex]));
                    }
                }
            }
            catch
            {
                // 保持原始错误处理风格
            }
            return values;
        }


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

        //public static List<int> ReadSecondColumnValues(string filePath)
        //{
        //    var values = new List<int>();
        //    try
        //    {
        //        using var doc = SpreadsheetDocument.Open(filePath, false);
        //        var workbookPart = doc.WorkbookPart;
        //        var worksheetPart = workbookPart.WorksheetParts.First();
        //        var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1);

        //        foreach (var row in rows)
        //        {
        //            var cells = row.Elements<Cell>().ToList();
        //            if (cells.Count >= 2)
        //            {
        //                var value = GetCellValue(workbookPart, cells[1]);
        //                if (int.TryParse(value, out int result))
        //                {
        //                    values.Add(result);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"读取Excel失败: {ex.Message}");
        //    }
        //    return values;
        //}

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
        public static async Task<(string SellMin, string BuyMax)> GetMarketData(HttpClient client, int typeId)
        {
            // 添加超时控制
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var response = await client.GetAsync(
                $"https://www.ceve-market.org/api/market/region/10000002/type/{typeId}.json",
                cts.Token);

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(json);

            return (
                SellMin: data.sell.min.ToString() ?? "N/A",
                BuyMax: data.buy.max.ToString() ?? "N/A"
            );
        }
    }

}
