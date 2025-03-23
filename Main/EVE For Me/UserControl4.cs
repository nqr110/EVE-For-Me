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
    /// <summary>
    /// EVE Online矿产市场数据查看控件
    /// 功能：
    /// 1. 支持多分类矿产数据展示（普通矿、冰矿、卫星矿）
    /// 2. 从Excel文件加载矿产配置数据
    /// 3. 实时获取CEVE市场API数据
    /// 4. 支持按需刷新和首次加载优化
    /// 5. 多线程安全UI更新
    /// </summary>
    public partial class UserControl4 : UserControl
    {
        // 初始化部分
        // ---------------------------------------------------------------------------------
        private readonly HttpClient _httpClient = new HttpClient();
        private const string EvEDataPath = @"..\..\..\..\EVE For Me\Database\evedata.xlsx";
        // 普通矿石路径
        private const string OrdinaryOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_ordinary ore.xlsx";
        // 冰矿路径
        private const string GlacialRockPath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_glacial rock.xlsx";
        // 卫星矿路径
        private const string SatelliteOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_satellite ore.xlsx";
        private bool _isFirstLoad = true;  // 添加首次加载标记
        private Dictionary<TabPage, TabPageConfig> _tabConfigs;
        // ---------------------------------------------------------------------------------

        // 配置类：封装单个Tab页的数据和控件配置
        private class TabPageConfig
        {
            public string DataPath { get; set; }            // Excel数据文件路径
            public Label NameLabel { get; set; }            // 显示矿产名称的标签
            public Label DataLabel { get; set; }            // 显示市场数据的标签
            public Label TimeLabel { get; set; }            // 显示更新时间的标签
            public Button RefreshButton { get; set; }       // 刷新按钮
            public bool IsFirstLoad { get; set; } = true;   // 首次加载标记
        }

        public UserControl4()
        {
            InitializeComponent();
            InitializeTabConfigs();
            SetupInitialLabels();
            WireUpEvents();
            InitializeComboBox();
        }
        private void InitializeComboBox()
        {
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
        }
        // 初始化配置时关联ComboBox
        private void InitializeTabConfigs()
        {
            _tabConfigs = new Dictionary<TabPage, TabPageConfig>
        {
            {
                tabPage2, new TabPageConfig
                {
                    DataPath = OrdinaryOrePath,
                    SheetSelector = comboBox1,  // 关联控件
                    NameLabel = label2,
                    DataLabel = label5,
                    TimeLabel = label4,
                    RefreshButton = button2
                }
            },
            {
                tabPage3, new TabPageConfig
                {
                    DataPath = GlacialRockPath,
                    SheetSelector = comboBox2,
                    NameLabel = label6,
                    DataLabel = label7,
                    TimeLabel = label8,
                    RefreshButton = button3
                }
            },
            {
                tabPage4, new TabPageConfig
                {
                    DataPath = SatelliteOrePath,
                    SheetSelector = comboBox3,
                    NameLabel = label9,
                    DataLabel = label10,
                    TimeLabel = label11,
                    RefreshButton = button1
                }
            }
        };
        }
        private void SetupInitialLabels()
        {
            foreach (var config in _tabConfigs.Values)
            {
                var names = ExcelHelper.ReadFirstColumnNames(config.DataPath);
                config.NameLabel.Text = string.Join(Environment.NewLine, names);
            }
        }
        private void WireUpEvents()
        {
            tabControl1.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            button2.Click += RefreshButton_Click;
            button3.Click += RefreshButton_Click;
            button1.Click += RefreshButton_Click;
        }
        private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_tabConfigs.TryGetValue(tabControl1.SelectedTab, out var config))
            {
                await ExecuteWebDataLoading(config, forceReload: false);
            }
        }
        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            var button = sender as Button;
            var config = _tabConfigs.Values.FirstOrDefault(c => c.RefreshButton == button);
            if (config != null)
            {
                button.Enabled = false;
                try
                {
                    await ExecuteWebDataLoading(config, forceReload: true);
                }
                finally
                {
                    button.Enabled = true;
                }
            }
        }
        private async Task ExecuteWebDataLoading(TabPageConfig config, bool forceReload)
        {
            try
            {
                if (!forceReload && !config.IsFirstLoad) return;
                config.IsFirstLoad = false;

                UpdateLabelSafe(config.DataLabel, "正在加载市场数据...");

                var typeIds = ExcelHelper.ReadSecondColumnValues(config.DataPath);

                var tasks = typeIds.Select(async typeId =>
                {
                    try
                    {
                        return (typeId, await MarketApi.GetMarketData(_httpClient, typeId));
                    }
                    catch
                    {
                        return (typeId, (SellMin: "错误", BuyMax: "错误"));
                    }
                });

                var results = await Task.WhenAll(tasks);

                var sb = new StringBuilder();
                foreach (var result in results)
                {
                    sb.AppendLine($"{"最低售价",-6}{result.Item2.SellMin,-8} | {"最高收购",-6}{result.Item2.BuyMax,-8}");
                }

                UpdateLabelSafe(config.DataLabel, sb.ToString());
                UpdateTimeLabel(config.TimeLabel, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception ex)
            {
                UpdateLabelSafe(config.DataLabel, $"操作失败: {ex.Message}");
            }
        }
        private void UpdateTimeLabel(Label label, string time)
        {
            if (label.InvokeRequired)
            {
                label.BeginInvoke(new Action(() => label.Text = time));
            }
            else
            {
                label.Text = time;
            }
        }
        private void UpdateLabelSafe(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(new Action(() => label.Text = text));
            }
            else
            {
                label.Text = text;
            }
        }
        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        // TabPage2 控件组 Start
        private void label2_Click(object sender, EventArgs e)
        {

        }
        private void label5_Click(object sender, EventArgs e)
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
        private void label4_Click_1(object sender, EventArgs e)
        {

        }
        // TabPage2 控件组 End
        private void label4_Click(object sender, EventArgs e)
        {

        }

        //加载功能
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

        //新增时间更新方法
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
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
