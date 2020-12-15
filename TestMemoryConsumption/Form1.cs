using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;

namespace TestMemoryConsumption {
    public partial class Form1 : XtraForm {
        const int MB = 1024 * 1024; 

        Series series;
        double memoryBeforeDataLoading;
        bool isChartPreparing = false;
        Dictionary<ViewType, int> seriesViewValueCount;

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
            this.seriesViewValueCount = new Dictionary<ViewType, int>();
            this.seriesViewValueCount.Add(ViewType.Area, 1);
            this.seriesViewValueCount.Add(ViewType.BoxPlot, 7);
            this.seriesViewValueCount.Add(ViewType.Bubble, 2);
            this.seriesViewValueCount.Add(ViewType.CandleStick, 4);
            this.seriesViewValueCount.Add(ViewType.FullStackedArea, 1);
            this.seriesViewValueCount.Add(ViewType.FullStackedBar, 1);
            this.seriesViewValueCount.Add(ViewType.FullStackedSplineArea, 1);
            this.seriesViewValueCount.Add(ViewType.Line, 1);
            this.seriesViewValueCount.Add(ViewType.RangeBar, 2);
            this.seriesViewValueCount.Add(ViewType.Point, 1);
            this.seriesViewValueCount.Add(ViewType.ScatterLine, 1);
            this.seriesViewValueCount.Add(ViewType.SideBySideStackedBar, 1);
            this.seriesViewValueCount.Add(ViewType.SideBySideRangeBar, 2);
            this.seriesViewValueCount.Add(ViewType.SideBySideFullStackedBar, 1);
            this.seriesViewValueCount.Add(ViewType.SplineArea, 1);
            this.seriesViewValueCount.Add(ViewType.StackedBar, 1);
            this.seriesViewValueCount.Add(ViewType.StackedSplineArea, 1);
            this.seriesViewValueCount.Add(ViewType.StepLine, 1);
            this.seriesViewValueCount.Add(ViewType.Stock, 4);
            this.seriesViewValueCount.Add(ViewType.Waterfall, 1);
            this.seriesViewValueCount.Add(ViewType.Bar, 1);
        }
        void InitializeSeriesCreationType() {
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.SeriesDataSource);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.Manual);
            this.seriesCreationTypeComboBox.Properties.Items.Add(DataCreationMode.ChartDataSource);
            this.seriesCreationTypeComboBox.SelectedIndex = 0;
        }
        void InitializeChart() {
            this.chartControl1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            this.chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;

        }
        void InitializeSeriesTypes() {
            object[] viewTypes = new object[] {
                ViewType.Area, ViewType.BoxPlot, ViewType.Bubble, ViewType.CandleStick, ViewType.FullStackedArea,
                ViewType.FullStackedBar, ViewType.FullStackedSplineArea, ViewType.Line, ViewType.RangeBar, ViewType.Point,
                ViewType.ScatterLine, ViewType.Bar, ViewType.SideBySideStackedBar, ViewType.SideBySideFullStackedBar,
                ViewType.SideBySideRangeBar, ViewType.SplineArea, ViewType.StackedArea, ViewType.StackedBar, ViewType.StackedSplineArea,
                ViewType.StepLine, ViewType.Stock, ViewType.Waterfall
            };
            this.seriesTypeComboBox.Properties.Items.AddRange(viewTypes);
            this.seriesTypeComboBox.SelectedItem = ViewType.Line;
        }
        void InitializePointsCount() {
            this.pointsCountComboBox.Properties.Items.AddRange(new object[5] { 1000, 10000, 100000, 1000000, 10000000 });
            this.pointsCountComboBox.SelectedIndex = 3;
        }
        void RecreateSeries() {
            if(this.seriesCreationTypeComboBox.SelectedItem == null)
                return;
            if((DataCreationMode)this.seriesCreationTypeComboBox.SelectedItem == DataCreationMode.Manual)
                RecreateManualCreatedSeries();
            else
                RecreateBindingSeries();
        }
        void RecreateBindingSeries() {
            List<DataItem> dataSource = DataItem.GenerateDataSource((int)this.pointsCountComboBox.SelectedItem);
            Prepare();
            this.series.ValueDataMembers.AddRange(DataItem.GetValueMembers(this.seriesViewValueCount[(ViewType)this.seriesTypeComboBox.SelectedItem]));
            this.series.ArgumentDataMember = "Argument";

            if((DataCreationMode)this.seriesCreationTypeComboBox.SelectedItem == DataCreationMode.SeriesDataSource)
                this.series.DataSource = dataSource;
            else
                this.chartControl1.DataSource = dataSource;
        }
        void RecreateManualCreatedSeries() {
            Prepare();
            SeriesPoint[] points = DataItem.CreateSeriesPoints((int)this.pointsCountComboBox.SelectedItem);
            this.series.Points.AddRange(points);
        }
        void Prepare() {
            this.isChartPreparing = true;
            this.chartControl1.DataSource = null;
            this.chartControl1.Series.Clear();
            this.series = new Series();
            this.chartControl1.Series.Add(this.series);
            this.series.ChangeView((ViewType)this.seriesTypeComboBox.SelectedItem);
            this.chartControl1.Update();
            this.memoryBeforeDataLoading = GetTotalMemory();
            this.isChartPreparing = false;
        }
        void UpdateMemoryUsage() {
            if(!this.isChartPreparing) {
                double totalMemory = GetTotalMemory();
                double resultMemory = totalMemory - this.memoryBeforeDataLoading;
                this.resultLabel.Text = string.Format("Series Memory {0:0.00} MB Total Memory {1:0.00} MB Before Memory {2:0.00}", resultMemory, totalMemory,  this.memoryBeforeDataLoading);
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
        Manual, SeriesDataSource, ChartDataSource
    }

    public class DataItem {
        public static List<DataItem> GenerateDataSource(int pointCount) {
            List<DataItem> dataSource = new List<DataItem>();
            for(int i = 0; i < pointCount; ++i)
                dataSource.Add(new DataItem(i, i - 1, i, i + 1, i + 2, i + 3, i + 4, i + 5));
            return dataSource;
        }
        public static SeriesPoint[] CreateSeriesPoints(int pointsCount) {
            SeriesPoint[] result = new SeriesPoint[pointsCount];
            for(int i = 0; i < pointsCount; ++i)
                result[i] = new SeriesPoint(i, i - 1, i, i + 1, i + 2, i + 3, i + 4, i + 5);
            return result;
        }
        public static string[] GetValueMembers(int n) {
            string[] valueNames = new string[n];
            for(int i = 0; i < n; i++)
                valueNames[i] = "Value" + (i + 1).ToString();
            return valueNames;
        }

        public double Argument { get; }
        public double Value1 { get; }
        public double Value2 { get; }
        public double Value3 { get; }
        public double Value4 { get; }
        public double Value5 { get; }
        public double Value6 { get; }
        public double Value7 { get; }

        public DataItem(double argument, double value1, double value2, double value3, double value4, double value5, double value6, double value7) {
            Argument = argument;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
            Value5 = value5;
            Value6 = value6;
            Value7 = value7;
        }
    }
}
