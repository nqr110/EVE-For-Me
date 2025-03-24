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
            public ComboBox SheetSelector { get; set; }     // 新增ComboBox关联
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
        private void InitializeTabConfigs()
        {
            _tabConfigs = new Dictionary<TabPage, TabPageConfig>
        {
            {
                tabPage2, new TabPageConfig
                {
                    DataPath = OrdinaryOrePath,
                    SheetSelector = comboBox1,
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
                var sheetName = GetCurrentSheetName(config.SheetSelector);
                var names = ExcelHelper.ReadFirstColumnNames(config.DataPath, sheetName);
                config.NameLabel.Text = string.Join(Environment.NewLine, names);
            }
        }
        private string GetCurrentSheetName(ComboBox comboBox)
        {
            return comboBox.SelectedIndex == 0 ? "Common" : "Compression";
        }
        private void WireUpEvents()
        {
            tabControl1.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            button2.Click += RefreshButton_Click;
            button3.Click += RefreshButton_Click;
            button1.Click += RefreshButton_Click;
            // 添加ComboBox事件处理
            comboBox1.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            comboBox2.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            comboBox3.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
        }
        private async void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var comboBox = sender as ComboBox;
            var config = _tabConfigs.Values.FirstOrDefault(c => c.SheetSelector == comboBox);
            if (config != null)
            {
                // 更新名称标签
                var sheetName = GetCurrentSheetName(comboBox);
                var names = ExcelHelper.ReadFirstColumnNames(config.DataPath, sheetName);
                UpdateLabelSafe(config.NameLabel, string.Join(Environment.NewLine, names));

                // 自动刷新数据
                await ExecuteWebDataLoading(config, forceReload: true);
            }
        }
        private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabControl1.SelectedTab;
            Console.WriteLine($"切换到: {selectedTab?.Name}");

            if (_tabConfigs.TryGetValue(selectedTab, out var config))
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
                var sheetName = GetCurrentSheetName(config.SheetSelector);
                var typeIDs = ExcelHelper.ReadSecondColumnValues(config.DataPath, sheetName);
                if (typeIDs.Count == 0)
                {
                    UpdateLabelSafe(config.DataLabel, "未找到有效矿石数据");
                    return;
                }

                var marketDataTasks = typeIDs.Select(async typeID =>
                {
                    try { return await MarketApi.GetMarketData(_httpClient, typeID); }
                    catch { return (SellMin: "N/A", BuyMax: "N/A"); }
                });

                var results = await Task.WhenAll(marketDataTasks);

                // 修改后的格式化代码
                var sb = new StringBuilder();
                foreach (var data in results)
                {
                    // 使用更宽松的对齐方式
                    sb.AppendLine($"卖出：{FormatForDisplay(data.SellMin),-18}买入：{FormatForDisplay(data.BuyMax),18}");
                }

                UpdateLabelSafe(config.DataLabel, sb.ToString());
                UpdateTimeLabel(config.TimeLabel, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                config.IsFirstLoad = false;
            }
            catch (Exception ex)
            {
                UpdateLabelSafe(config.DataLabel, $"操作失败: {ex.Message}");
            }
        }
        // 新增显示格式化方法
        private static string FormatForDisplay(string price)
        {
            if (decimal.TryParse(price.Replace(",", ""), out decimal value))
            {
                return value.ToString("#,##0.00").PadLeft(15);
            }
            return price.PadLeft(15);
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
            var config = _tabConfigs[tabPage2]; // 直接获取对应配置
            await ExecuteWebDataLoading(config, forceReload: true);
        }
        private void label4_Click_1(object sender, EventArgs e)
        {

        }
        // TabPage2 控件组 End
        private void label4_Click(object sender, EventArgs e)
        {

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

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    // 公共帮助类
    public static class ExcelHelper
    {
        private static WorksheetPart GetWorksheetPart(WorkbookPart workbookPart, string sheetName)
        {
            var sheet = workbookPart.Workbook.Descendants<Sheet>()
                .FirstOrDefault(s => s.Name.Value.Equals(sheetName, StringComparison.OrdinalIgnoreCase));

            return sheet == null
                ? null
                : workbookPart.GetPartById(sheet.Id) as WorksheetPart;
        }
        // 原始方法保持兼容
        public static List<int> ReadSecondColumnValues(string filePath, string sheetName)
        {
            return ReadNumericColumn(filePath, sheetName, 1);
        }
        // 新增第一列读取方法
        public static List<string> ReadFirstColumnNames(string filePath, string sheetName)
        {
            return ReadTextColumn(filePath, sheetName, 0);
        }

        // 专用数值列读取方法
        private static List<int> ReadNumericColumn(string filePath, string sheetName, int columnIndex)
        {
            var values = new List<int>();
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, false);
                var workbookPart = doc.WorkbookPart;
                var worksheetPart = GetWorksheetPart(workbookPart, sheetName);

                if (worksheetPart == null) return values;

                var rows = worksheetPart.Worksheet.Descendants<Row>().Skip(1);
                foreach (var row in rows)
                {
                    var cells = row.Elements<Cell>().ToList();
                    if (cells.Count > columnIndex)
                    {
                        var cellValue = GetCellValue(workbookPart, cells[columnIndex]);
                        if (int.TryParse(cellValue, out int numericValue))
                        {
                            values.Add(numericValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Excel读取错误: {ex.Message}");
            }
            return values;
        }

        // 专用文本列读取方法
        private static List<string> ReadTextColumn(string filePath, string sheetName, int columnIndex)
        {
            var values = new List<string>();
            try
            {
                using var doc = SpreadsheetDocument.Open(filePath, false);
                var workbookPart = doc.WorkbookPart;
                var worksheetPart = GetWorksheetPart(workbookPart, sheetName);

                if (worksheetPart == null) return values;

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
                // 保持错误处理
            }
            return values;
        }
        // 添加兼容重载
        public static List<string> ReadFirstColumnNames(string filePath)
        {
            return ReadTextColumn(filePath, "Common", 0);
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
            try
            {
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                var response = await client.GetAsync(
                    $"https://www.ceve-market.org/api/market/region/10000002/type/{typeId}.json",
                    cts.Token);

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(json);

                return (
                    SellMin: FormatPrice(data.sell.min),
                    BuyMax: FormatPrice(data.buy.max)
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); // 使用 ex
                return (SellMin: "ERR", BuyMax: "ERR");
            }
        }
        private static string FormatPrice(dynamic value)
        {
            try
            {
                // 强制转换为 decimal 类型进行格式化
                decimal price = Convert.ToDecimal(value);
                return price.ToString("N2").PadLeft(10); // 统一为 10 字符宽度
            }
            catch
            {
                return "N/A".PadLeft(10);
            }
        }
    }
}
