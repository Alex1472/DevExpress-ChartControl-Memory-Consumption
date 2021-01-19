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
        Dictionary<ViewType, ChartDataMemberType[]> seriesViewDataMembers;

        ViewType SeriesViewType => (ViewType)this.seriesTypeComboBox.SelectedItem;
        DataCreationMode SeriesCreationMode => (DataCreationMode)this.seriesCreationTypeComboBox.SelectedItem;
        int PointsCount => (int)this.pointsCountComboBox.SelectedItem;
        bool IsNewApi => SeriesCreationMode == DataCreationMode.ManualNewApi || SeriesCreationMode == DataCreationMode.SeriesDataSourceNewApi || SeriesCreationMode == DataCreationMode.ChartDataSourceNewApi;
        bool IsManualCreation => SeriesCreationMode == DataCreationMode.ManualNewApi || SeriesCreationMode == DataCreationMode.ManualOldApi;
        bool IsChartDataSource => SeriesCreationMode == DataCreationMode.ChartDataSourceOldApi || SeriesCreationMode == DataCreationMode.ChartDataSourceNewApi;
        int SeriesValuesCount => this.seriesViewDataMembers[SeriesViewType].Length;
        DataSourceAdapter DataAdapter => this.series.DataAdapter as DataSourceAdapter;

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
            this.seriesCreationTypeComboBox.SelectedIndex = 0;
        }
        void InitializeChart() {
            this.chartControl1.CrosshairEnabled = DevExpress.Utils.DefaultBoolean.False;
            this.chartControl1.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        }
        void InitializeSeriesTypes() {
            this.seriesTypeComboBox.Properties.Items.AddRange(this.seriesViewDataMembers.Keys);
            this.seriesTypeComboBox.SelectedItem = ViewType.Line;
        }
        void InitializePointsCount() {
            this.pointsCountComboBox.Properties.Items.AddRange(new object[5] { 1000, 10000, 100000, 1000000, 10000000 });
            this.pointsCountComboBox.SelectedIndex = 3;
        }
        void RecreateSeries() {
            if(this.seriesCreationTypeComboBox.SelectedItem == null)
                return;
            if(IsManualCreation)
                RecreateManualCreatedSeries();
            else
                RecreateBindingSeries();
        }
        void RecreateBindingSeries() {
            object dataSource = DataGenerator.GenerateDataSource(PointsCount, SeriesValuesCount);
            Prepare();
            if(IsNewApi)
                PrepareDataMembersNewApi();
            else
                PrepareDataMembersOldApi();
            AssignDataSource(dataSource);
        }
        void AssignDataSource(object dataSource) {
            if(IsChartDataSource) {
                this.chartControl1.DataSource = dataSource;
                return;
            }
            if(IsNewApi)
                DataAdapter.DataSource = dataSource;
            else
                this.series.DataSource = dataSource;
        }
        void PrepareDataMembersNewApi() {
            DataSourceAdapter dataAdapter = new DataSourceAdapter();
            dataAdapter.DataMembers.Add(new DataMember(ChartDataMemberType.Argument, "Argument"));
            ChartDataMemberType[] valueDataMembers = this.seriesViewDataMembers[SeriesViewType];
            string[] columnNames = DataGenerator.GetValueMembers(SeriesValuesCount);
            for(int i = 0; i < valueDataMembers.Length; i++)
                dataAdapter.DataMembers.Add(new DataMember(valueDataMembers[i], columnNames[i]));
            this.series.DataAdapter = dataAdapter;
        }
        void PrepareDataMembersOldApi() {
            this.series.ValueDataMembers.AddRange(DataGenerator.GetValueMembers(SeriesValuesCount));
            this.series.ArgumentDataMember = "Argument";
        }
        void RecreateManualCreatedSeries() {
            Prepare();
            AssingPoints(DataGenerator.CreateSeriesPoints(PointsCount, SeriesValuesCount));
        }
        void AssingPoints(SeriesPoint[] points) {
            if(IsNewApi) {
                SeriesPointCollection adapter = new SeriesPointCollection();
                adapter.AddRange(points);
                this.series.DataAdapter = adapter;
            } else 
                this.series.Points.AddRange(points);
        }
        void Prepare() {
            this.isChartPreparing = true;
            this.chartControl1.DataSource = null;
            this.chartControl1.Series.Clear();
            this.series = new Series();
            this.chartControl1.Series.Add(this.series);
            this.series.ChangeView(SeriesViewType);
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
        ManualOldApi, SeriesDataSourceOldApi, ChartDataSourceOldApi, ManualNewApi, SeriesDataSourceNewApi, ChartDataSourceNewApi
    }
}
