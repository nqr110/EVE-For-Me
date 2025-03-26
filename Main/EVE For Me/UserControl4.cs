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
        // 初始化部分
        private readonly HttpClient _httpClient = new HttpClient();
        private const string EvEDataPath = @"..\..\..\..\EVE For Me\Database\evedata.xlsx";
        private const string OrdinaryOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_ordinary ore.xlsx";
        private const string GlacialRockPath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_glacial rock.xlsx";
        private const string SatelliteOrePath = @"E:\Visual Studio\EVE For Me\Main\EVE For Me\Database\EVE_TypeID_satellite ore.xlsx";
        private Dictionary<TabPage, TabPageConfig> _tabConfigs;

        private class TabPageConfig
        {
            public string DataPath { get; set; }
            public Label NameLabel { get; set; }
            public Label DataLabel { get; set; }
            public Label TimeLabel { get; set; }
            public Button RefreshButton { get; set; }
            public bool IsFirstLoad { get; set; } = true;
            public ComboBox SheetSelector { get; set; }
            public Task InitializationTask { get; set; } // 用于跟踪初始化任务
        }

        public UserControl4()
        {
            InitializeComponent();
            InitializeTabConfigs();
            WireUpEvents();
            InitializeComboBoxSelection(); // 修改这里
            BeginAsyncInitialization(); // 开始异步初始化
        }

        private void BeginAsyncInitialization()
        {
            Task.Run(async () =>
            {
                // 并行预加载所有Excel数据
                var loadTasks = _tabConfigs.Values.Select(config =>
                    Task.Run(() => PreloadExcelData(config.DataPath)));

                await Task.WhenAll(loadTasks);

                // UI更新需要回到主线程
                this.Invoke(new Action(() =>
                {
                    SetupInitialLabels();
                    InitializeComboBoxSelection();
                }));
            });
        }
        private async void button2_Click(object sender, EventArgs e)
        {
            var config = _tabConfigs[tabPage2]; // 直接获取对应配置
            await ExecuteWebDataLoading(config, forceReload: true);
        }
        private void PreloadExcelData(string filePath)
        {
            // 通过访问缓存来预加载数据
            var commonData = ExcelHelper.ReadFirstColumnNames(filePath, "Common");
            var compressionData = ExcelHelper.ReadFirstColumnNames(filePath, "Compression");
        }

        private void InitializeComboBoxSelection()
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
                var sheetName = GetCurrentSheetName(comboBox);
                var names = ExcelHelper.ReadFirstColumnNames(config.DataPath, sheetName);
                UpdateLabelSafe(config.NameLabel, string.Join(Environment.NewLine, names));

                await ExecuteWebDataLoading(config, forceReload: true);
            }
        }

        private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedTab = tabControl1.SelectedTab;
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

                var marketDataTasks = typeIDs.Select(typeID =>
                    GetMarketDataWithTimeout(typeID, TimeSpan.FromSeconds(10)));

                var results = await Task.WhenAll(marketDataTasks);

                var sb = new StringBuilder();
                foreach (var data in results)
                {
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

        private async Task<(string SellMin, string BuyMax)> GetMarketDataWithTimeout(int typeID, TimeSpan timeout)
        {
            var cts = new CancellationTokenSource();
            cts.CancelAfter(timeout);

            try
            {
                return await MarketApi.GetMarketData(_httpClient, typeID, cts.Token);
            }
            catch
            {
                return ("N/A".PadLeft(15), "N/A".PadLeft(15));
            }
        }

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

        // 保留所有空事件处理方法
        private void tabPage1_Click(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
        private void label5_Click(object sender, EventArgs e) { }
        private void label4_Click_1(object sender, EventArgs e) { }
        private void label4_Click(object sender, EventArgs e) { }
        private void label9_Click(object sender, EventArgs e) { }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { }
        private void label3_Click(object sender, EventArgs e) { }
    }

    public static class ExcelHelper
    {
        private static readonly Lazy<ReaderWriterLockSlim> _cacheLock =
            new Lazy<ReaderWriterLockSlim>(() => new ReaderWriterLockSlim());

        private static readonly Dictionary<string, Dictionary<string, (List<string>, List<int>)>> _globalCache =
            new Dictionary<string, Dictionary<string, (List<string>, List<int>)>>();

        public static List<string> ReadFirstColumnNames(string filePath, string sheetName) =>
            ReadCachedData(filePath, sheetName).Item1;

        public static List<int> ReadSecondColumnValues(string filePath, string sheetName) =>
            ReadCachedData(filePath, sheetName).Item2;

        private static (List<string>, List<int>) ReadCachedData(string filePath, string sheetName)
        {
            try
            {
                _cacheLock.Value.EnterUpgradeableReadLock();

                if (TryGetFromCache(filePath, sheetName, out var cachedData))
                    return cachedData;

                return LoadAndCacheData(filePath, sheetName);
            }
            finally
            {
                _cacheLock.Value.ExitUpgradeableReadLock();
            }
        }

        private static bool TryGetFromCache(string filePath, string sheetName,
            out (List<string>, List<int>) data)
        {
            if (_globalCache.TryGetValue(filePath, out var fileCache))
            {
                if (fileCache.TryGetValue(sheetName, out data))
                    return true;
            }
            data = default;
            return false;
        }

        private static (List<string>, List<int>) LoadAndCacheData(string filePath, string sheetName)
        {
            try
            {
                _cacheLock.Value.EnterWriteLock();

                // 二次检查缓存
                if (TryGetFromCache(filePath, sheetName, out var cachedData))
                    return cachedData;

                using var doc = SpreadsheetDocument.Open(filePath, false);
                var workbookPart = doc.WorkbookPart;
                var sheet = workbookPart.Workbook.Descendants<Sheet>()
                    .FirstOrDefault(s => s.Name.Equals(sheetName));

                if (sheet == null) return (new List<string>(), new List<int>());

                var (names, ids) = ProcessWorksheet(workbookPart, sheet);

                UpdateCache(filePath, sheetName, names, ids);
                return (names, ids);
            }
            finally
            {
                _cacheLock.Value.ExitWriteLock();
            }
        }

        // 修改ExcelHelper类中的ProcessWorksheet方法
private static (List<string>, List<int>) ProcessWorksheet(WorkbookPart workbookPart, Sheet sheet)
{
    var wsPart = workbookPart.GetPartById(sheet.Id) as WorksheetPart;
    var rows = wsPart.Worksheet.Descendants<Row>().Skip(1).ToList();

    // 预先单线程加载所有数据
    var cellData = rows.Select(row => 
    {
        var cells = row.Elements<Cell>().Take(2).ToList();
        return (
            GetCellValue(workbookPart, cells[0]),
            GetCellValue(workbookPart, cells[1])
        );
    }).ToList();

    // 并行处理已加载的数据
    var names = new List<string>(rows.Count);
    var ids = new List<int>(rows.Count);
    
    Parallel.ForEach(cellData, data =>
    {
        lock (names)
        {
            names.Add(data.Item1);
            ids.Add(int.Parse(data.Item2));
        }
    });

    return (names, ids);
}

        private static void UpdateCache(string filePath, string sheetName,
            List<string> names, List<int> ids)
        {
            if (!_globalCache.ContainsKey(filePath))
                _globalCache[filePath] = new Dictionary<string, (List<string>, List<int>)>();

            _globalCache[filePath][sheetName] = (names, ids);
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

    public static class MarketApi
    {
        public static async Task<(string SellMin, string BuyMax)> GetMarketData(
            HttpClient client, int typeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await client.GetAsync(
                    $"https://www.ceve-market.org/api/market/region/10000002/type/{typeId}.json",
                    cancellationToken);

                response.EnsureSuccessStatusCode();
                var json = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<MarketData>(json);

                return (
                    FormatPrice(data?.sell?.min),
                    FormatPrice(data?.buy?.max)
                );
            }
            catch
            {
                return ("N/A".PadLeft(15), "N/A".PadLeft(15));
            }
        }

        private static string FormatPrice(object value) =>
            decimal.TryParse(value?.ToString(), out var price) ?
                price.ToString("#,##0.00").PadLeft(15) :
                "N/A".PadLeft(15);

        private class MarketData
        {
            public PriceInfo sell { get; set; }
            public PriceInfo buy { get; set; }
        }

        private class PriceInfo
        {
            public object min { get; set; }
            public object max { get; set; }
        }
    }
}