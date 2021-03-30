using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

namespace TestMemoryConsumption {
    public partial class Form1 : XtraForm {
        const int MB = 1024 * 1024; 

        double memoryBeforeDataLoading;
        bool isChartPreparing = false;
        Dictionary<ViewType, ChartDataMemberType[]> seriesViewDataMembers;

        ViewType SeriesViewType => (ViewType)this.seriesTypeComboBox.SelectedItem;
        DataCreationMode SeriesCreationMode => (DataCreationMode)this.seriesCreationTypeComboBox.SelectedItem;
        int PointsCount => (int)this.pointsCountComboBox.SelectedItem;
        

        public Form1() {
            InitializeComponent();
            InitializeSeriesViewValueDict();
            InitializeChart();
            InitializeSeriesTypes();
            InitializePointsCount();
            InitializeSeriesCreationType();
            this.timer1.Start();
        }
        void InitializeSeriesViewValueDict() {
            this.seriesViewDataMembers = new Dictionary<ViewType, ChartDataMemberType[]>();
            this.seriesViewDataMembers.Add(ViewType.Area, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.BoxPlot, new ChartDataMemberType[] { ChartDataMemberType.BoxPlotMin, ChartDataMemberType.BoxPlotQuartile_1,
            ChartDataMemberType.BoxPlotMean, ChartDataMemberType.BoxPlotMedian, ChartDataMemberType.BoxPlotQuartile_3, ChartDataMemberType.BoxPlotMax, ChartDataMemberType.BoxPlotOutliers});
            this.seriesViewDataMembers.Add(ViewType.Bubble, new ChartDataMemberType[] { ChartDataMemberType.Value, ChartDataMemberType.Weight });
            this.seriesViewDataMembers.Add(ViewType.CandleStick, new ChartDataMemberType[] { ChartDataMemberType.Low, ChartDataMemberType.High, ChartDataMemberType.Open,
            ChartDataMemberType.Close});
            this.seriesViewDataMembers.Add(ViewType.FullStackedArea, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.FullStackedBar, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.FullStackedSplineArea, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.Line, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.RangeBar, new ChartDataMemberType[] { ChartDataMemberType.Value_1, ChartDataMemberType.Value_2 });
            this.seriesViewDataMembers.Add(ViewType.Point, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.ScatterLine, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.SideBySideStackedBar, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.SideBySideRangeBar, new ChartDataMemberType[] { ChartDataMemberType.Value_1, ChartDataMemberType.Value_2 });
            this.seriesViewDataMembers.Add(ViewType.SideBySideFullStackedBar, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.SplineArea, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.StackedBar, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.StackedSplineArea, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.StepLine, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.Stock, new ChartDataMemberType[] { ChartDataMemberType.Low, ChartDataMemberType.High, ChartDataMemberType.Open,
            ChartDataMemberType.Close});
            this.seriesViewDataMembers.Add(ViewType.Waterfall, new ChartDataMemberType[] { ChartDataMemberType.Value });
            this.seriesViewDataMembers.Add(ViewType.Bar, new ChartDataMemberType[] { ChartDataMemberType.Value });
        }
        void InitializeSeriesCreationType() {
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.SeriesDataSourceOldApi);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.ManualOldApi);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.ChartDataSourceOldApi);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.SeriesDataSourceNewApi);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.ManualNewApi);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.ChartDataSourceNewApi);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.ChartCustomAdapter);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.SeriesCustomAdapter);
            this.seriesCreationTypeComboBox.SelectedIndex = 7;
        }
        void InitializeChart() {
            this.chartControl1.AutoLayout = false;
            this.chartControl1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            this.chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        }
        void InitializeSeriesTypes() {
            this.seriesTypeComboBox.Properties.Items.AddRange(this.seriesViewDataMembers.Keys);
            this.seriesTypeComboBox.SelectedItem = ViewType.Line;
        }
        void InitializePointsCount() {
            this.pointsCountComboBox.Properties.Items.AddRange(new object[5] { 1000, 10000, 100000, 1000000, 20000000 });
            this.pointsCountComboBox.SelectedIndex = 3;
        }
        void RecreateSeries() {
            if(this.seriesCreationTypeComboBox.SelectedItem == null)
                return;
            Prepare();
            MemoryConsumptionTestsBase tester = MemoryConsumptionTestsBase.Create(SeriesCreationMode, this.chartControl1);
            tester.SetUp();
            tester.Test(SeriesViewType, this.seriesViewDataMembers[SeriesViewType].Length, PointsCount);
        }
        Stopwatch sw = new Stopwatch();
        void Prepare() {
            this.isChartPreparing = true;
            this.chartControl1.DataSource = null;
            this.chartControl1.SeriesTemplate.ArgumentDataMember = null;
            this.chartControl1.SeriesTemplate.ValueDataMembers.Clear();
            this.chartControl1.SeriesTemplate.SeriesDataMember = null;
            this.chartControl1.Series.Clear();
            this.chartControl1.Update();
            Application.DoEvents();
            this.memoryBeforeDataLoading = GetTotalMemory();
            this.sw.Restart();
            this.isChartPreparing = false;
        }
        void UpdateMemoryUsage() {
            if(!this.isChartPreparing) {
                long time = this.sw.ElapsedMilliseconds;
                this.sw.Stop();
                double totalMemory = GetTotalMemory();
                double resultMemory = totalMemory - this.memoryBeforeDataLoading;
                this.resultLabel.Text = $"Series Memory {resultMemory:0.00} MB Total Memory {totalMemory:0.00} MB Before Memory {memoryBeforeDataLoading:0.00}; Time {time}ms";
            }
        }
        double GetTotalMemory() {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return (double)GC.GetTotalMemory(true) / MB;
        }
        void Timer1_Tick(object sender, EventArgs e) {
            UpdateMemoryUsage();
        }
        void SeriesTypeComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            RecreateSeries();
        }
        void PointsCountComboBox_SelectedIndexChanged_1(object sender, EventArgs e) {
            RecreateSeries();
        }
        void SeriesCreationTypeComboBox_SelectedIndexChanged_1(object sender, EventArgs e) {
            RecreateSeries();
        }
    }

    public enum DataCreationMode {
        ManualOldApi, SeriesDataSourceOldApi, ChartDataSourceOldApi, ManualNewApi, SeriesDataSourceNewApi, ChartDataSourceNewApi, ChartCustomAdapter, SeriesCustomAdapter
    }
}
