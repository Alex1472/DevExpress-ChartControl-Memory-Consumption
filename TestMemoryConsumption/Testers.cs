using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace TestMemoryConsumption {
    abstract class MemoryConsumptionTestsBase {
        public static MemoryConsumptionTestsBase Create(DataCreationMode creationMode, ChartControl chart) {
            switch (creationMode) {
                case DataCreationMode.ChartDataSourceNewApi: return new MemoryConsumptionChartDataSourceTests_NewApi(chart);
                case DataCreationMode.ChartDataSourceOldApi: return new MemoryConsumptionChartDataSourceTests_OldApi(chart);
                //case DataCreationMode.ManualNewApi: return new MemoryConsumptionManualCreationTests_NewApi(chart); maybe next time
                case DataCreationMode.ManualOldApi: return new MemoryConsumptionManualCreationTests(chart);
                case DataCreationMode.SeriesDataSourceNewApi: return new MemoryConsumptionSeriesDataSourceTests_NewApi(chart);
                case DataCreationMode.SeriesDataSourceOldApi: return new MemoryConsumptionSeriesDataSourceTests_OldApi(chart);
                case DataCreationMode.ChartCustomAdapter: return new MemoryConsumptionChartCustomAdapter(chart);
                case DataCreationMode.SeriesCustomAdapter: return new MemoryConsumptionSeriesCustomAdapter(chart);
                default: throw new ArgumentException();
            }
        }

        protected ChartControl Chart { get; }

        protected MemoryConsumptionTestsBase(ChartControl chart) {
            Chart = chart;
        }

        protected abstract void InitializeChart(ViewType view, int valuesCount, int pointsCount);
        public virtual void SetUp() {
        }
        public void Test(ViewType view, int valuesCount, int pointsCount) {
            InitializeChart(view, valuesCount, pointsCount);
            Application.DoEvents();
        }
    }
    abstract class MemoryConsumptionDataSourceBaseTests : MemoryConsumptionTestsBase {
        protected abstract SeriesBase Series { get; }

        protected MemoryConsumptionDataSourceBaseTests(ChartControl chart)
            : base(chart) {
        }

        protected sealed override void InitializeChart(ViewType view, int valuesCount, int pointsCount) {
            object dataSource = GenerateDataSource(pointsCount, valuesCount);
            InitializeSeries(view, valuesCount, pointsCount);
            ApplyDataSource(dataSource);
        }
        protected virtual object GenerateDataSource(int pointsCount, int valuesCount) {
            return DataGenerator.GenerateDataSource(pointsCount, valuesCount);
        }
        protected abstract void ApplyDataSource(object dataSource);
        protected abstract void InitializeSeries(ViewType view, int valuesCount, int pointsCount);
    }
    abstract class MemoryConsumptionDataSourceBaseTests_OldApi : MemoryConsumptionDataSourceBaseTests {
        protected MemoryConsumptionDataSourceBaseTests_OldApi(ChartControl chart)
            : base(chart) {
        }
        
        protected override void InitializeSeries(ViewType view, int valuesCount, int pointsCount) {
            Series.ArgumentDataMember = "Argument";
            Series.ValueDataMembers.AddRange(DataGenerator.GetValueMembers(valuesCount));
            Series.DataSorted = true;
            Series.ChangeView(view);
        }
    }
    abstract class MemoryConsumptionDataSourceBaseTests_NewApi : MemoryConsumptionDataSourceBaseTests {
        protected MemoryConsumptionDataSourceBaseTests_NewApi(ChartControl chart)
            : base(chart) {
        }

        protected abstract DataSourceAdapterBase CreateAdapter();
        protected virtual void InitializeAdapter(DataSourceAdapterBase adapter, ViewType view, int valuesCount) {
            adapter.DataMembers.Add(new DataMember(ChartDataMemberType.Argument, "Argument", ScaleType.Numerical));
            Dictionary<ChartDataMemberType, string> dataMembers = DataGenerator.GetValueMembers(view, valuesCount);
            foreach (var dataMember in dataMembers)
                adapter.DataMembers.Add(new DataMember(dataMember.Key, dataMember.Value, ScaleType.Numerical));
            adapter.DataSorted = true;
        }
        protected sealed override void InitializeSeries(ViewType view, int valuesCount, int pointsCount) {
            DataSourceAdapterBase adapter = CreateAdapter();
            InitializeAdapter(adapter, view, valuesCount);
            Series.ChangeView(view);
        }
    }
    class MemoryConsumptionSeriesDataSourceTests_OldApi : MemoryConsumptionDataSourceBaseTests_OldApi {
        Series series;

        protected override SeriesBase Series => series;
        
        public MemoryConsumptionSeriesDataSourceTests_OldApi(ChartControl chart)
            : base(chart) {
        }

        public override void SetUp() {
            base.SetUp();
            this.series = new Series();
            Chart.Series.Add(this.series);
        }
        protected override void ApplyDataSource(object dataSource) {
            this.series.DataSource = dataSource;
        }
    }
    class MemoryConsumptionChartDataSourceTests_OldApi : MemoryConsumptionDataSourceBaseTests_OldApi {
        protected override SeriesBase Series => Chart.SeriesTemplate;

        public MemoryConsumptionChartDataSourceTests_OldApi(ChartControl chart)
            : base(chart) {
        }
        
        protected override void ApplyDataSource(object dataSource) {
            Chart.SeriesDataMember = "Name";
            Chart.DataSource = dataSource;
        }
    }
    class MemoryConsumptionSeriesDataSourceTests_NewApi : MemoryConsumptionDataSourceBaseTests_NewApi {
        Series series;

        protected override SeriesBase Series => series;

        public MemoryConsumptionSeriesDataSourceTests_NewApi(ChartControl chart)
            : base(chart) {
        }
        
        public override void SetUp() {
            base.SetUp();
            this.series = new Series();
            Chart.Series.Add(this.series);
        }

        protected override DataSourceAdapterBase CreateAdapter() {
            return new DataSourceAdapter();
        }
        protected override void InitializeAdapter(DataSourceAdapterBase adapter, ViewType view, int valuesCount) {
            base.InitializeAdapter(adapter, view, valuesCount);
            this.series.DataAdapter = (DataSourceAdapter)adapter;
        }
        protected override void ApplyDataSource(object dataSource) {
            ((DataSourceAdapter)series.DataAdapter).DataSource = dataSource;
        }
    }
    class MemoryConsumptionChartDataSourceTests_NewApi : MemoryConsumptionDataSourceBaseTests_NewApi {
        protected override SeriesBase Series => Chart.SeriesTemplate;
        
        public MemoryConsumptionChartDataSourceTests_NewApi(ChartControl chart)
            : base(chart) {
        }

        protected override DataSourceAdapterBase CreateAdapter() {
            return new SeriesTemplateAdapter();
        }
        protected override void InitializeAdapter(DataSourceAdapterBase adapter, ViewType view, int valuesCount) {
            base.InitializeAdapter(adapter, view, valuesCount);
            adapter.DataMembers.Add(new DataMember(ChartDataMemberType.Series, "Name"));
            Chart.SeriesTemplate.DataAdapter = (SeriesTemplateAdapter)adapter;
        }
        protected override void ApplyDataSource(object dataSource) {
            Chart.DataSource = dataSource;
        }
    }
    abstract class MemoryConsumptionCustomAdapterBase : MemoryConsumptionDataSourceBaseTests {
        #region custom adapter
        public abstract class CustomAdapter : ISeriesAdapter, ISeriesTemplateAdapter {
            public abstract object DataSource { get; }
            public bool DataSorted => true;
            public abstract int ItemsCount { get; }

            event NotifyChartDataChangedEventHandler IChartDataAdapter.DataChanged {
                add { }
                remove { }
            }

            public DateTime GetDateTimeValue(int index, ChartDataMemberType dataMember) {
                throw new NotImplementedException();
            }
            public string GetQualitativeValue(int index, ChartDataMemberType dataMember) {
                throw new NotImplementedException();
            }
            public TimeSpan GetTimeSpanValue(int index, ChartDataMemberType dataMember) {
                throw new NotImplementedException();
            }
            public ActualScaleType GetScaleType(ChartDataMemberType dataMember) {
                return ActualScaleType.Numerical;
            }
            public abstract object Clone();
            public abstract double GetNumericalValue(int index, ChartDataMemberType dataMember);
            public abstract object GetKey(int index);
            public virtual object GetObjectValue(int index, ChartDataMemberType dataMember) {
                if (dataMember == ChartDataMemberType.Series)
                    return "Name";
                throw new NotImplementedException();
            }
        }
        public abstract class CustomAdapter<T> : CustomAdapter {
            protected abstract int ValuesCount { get; }
            protected T[] Items { get; }
            public override int ItemsCount => Items.Length;
            public override object DataSource => Items;

            protected CustomAdapter(int pointCount) {
                Items = (T[])DataGenerator.GenerateDataSource(pointCount, ValuesCount);
            }

            public override object GetKey(int index) {
                return Items[index];
            }
        }

        public class CustomAdapter1 : CustomAdapter<DataItem1> {
            protected override int ValuesCount => 1;

            public CustomAdapter1(int pointCount)
                : base(pointCount) {
            }

            public override object Clone() {
                return this;
            }
            public override double GetNumericalValue(int index, ChartDataMemberType dataMember) {
                switch (dataMember) {
                    case ChartDataMemberType.Argument: return Items[index].Argument;
                    case ChartDataMemberType.Value: return Items[index].Value1;
                }
                return double.NaN;
            }
        }
        public class CustomAdapter2 : CustomAdapter<DataItem2> {
            protected override int ValuesCount => 2;

            public CustomAdapter2(int pointsCount)
                : base(pointsCount) {
            }

            public override object Clone() {
                return this;
            }
            public override double GetNumericalValue(int index, ChartDataMemberType dataMember) {
                switch (dataMember) {
                    case ChartDataMemberType.Argument: return Items[index].Argument;
                    case ChartDataMemberType.Value_1: return Items[index].Value1;
                    case ChartDataMemberType.Value_2: return Items[index].Value2;
                }
                return double.NaN;
            }
        }
        public class CustomAdapter4 : CustomAdapter<DataItem4> {
            protected override int ValuesCount => 4;

            public CustomAdapter4(int pointsCount)
                : base(pointsCount) {
            }

            public override object Clone() {
                return this;
            }
            public override double GetNumericalValue(int index, ChartDataMemberType dataMember) {
                switch (dataMember) {
                    case ChartDataMemberType.Argument: return Items[index].Argument;
                    case ChartDataMemberType.Open: return Items[index].Value1;
                    case ChartDataMemberType.High: return Items[index].Value2;
                    case ChartDataMemberType.Low: return Items[index].Value3;
                    case ChartDataMemberType.Close: return Items[index].Value4;
                }
                return double.NaN;
            }
        }
        public class CustomAdapter7 : CustomAdapter<DataItem7> {
            protected override int ValuesCount => 7;

            public CustomAdapter7(int pointsCount)
                : base(pointsCount) {
            }

            public override object Clone() {
                return this;
            }
            public override double GetNumericalValue(int index, ChartDataMemberType dataMember) {
                switch (dataMember) {
                    case ChartDataMemberType.Argument: return Items[index].Argument;
                    case ChartDataMemberType.BoxPlotMin: return Items[index].Value1;
                    case ChartDataMemberType.BoxPlotQuartile_1: return Items[index].Value2;
                    case ChartDataMemberType.BoxPlotMedian: return Items[index].Value3;
                    case ChartDataMemberType.BoxPlotQuartile_3: return Items[index].Value4;
                    case ChartDataMemberType.BoxPlotMax: return Items[index].Value5;
                    case ChartDataMemberType.BoxPlotMean: return Items[index].Value6;
                }
                return double.NaN;
            }

            public override object GetObjectValue(int index, ChartDataMemberType dataMember) {
                if (dataMember == ChartDataMemberType.BoxPlotOutliers)
                    return Items[index].Value7;
                return base.GetObjectValue(index, dataMember);
            }
        }
        #endregion

        protected IChartDataAdapter DataAdapter { get; private set; }

        protected MemoryConsumptionCustomAdapterBase(ChartControl chart)
            : base(chart) {
        }

        protected override object GenerateDataSource(int pointsCount, int valuesCount) {
            return DataAdapter = DataGenerator.GetDataAdapter(valuesCount, pointsCount);
        }
    }
    class MemoryConsumptionSeriesCustomAdapter : MemoryConsumptionCustomAdapterBase {
        Series series;

        protected override SeriesBase Series => series;
        
        public MemoryConsumptionSeriesCustomAdapter(ChartControl chart)
            : base(chart) {
        }

        public override void SetUp() {
            base.SetUp();
            this.series = new Series();
            Chart.Series.Add(this.series);
        }
        protected override void InitializeSeries(ViewType view, int valuesCount, int pointsCount) {
            this.series.ChangeView(view);
            this.series.DataAdapter = (ISeriesAdapter)DataAdapter;
        }
        protected override void ApplyDataSource(object dataSource) {
        }
    }
    class MemoryConsumptionChartCustomAdapter : MemoryConsumptionCustomAdapterBase {
        protected override SeriesBase Series => Chart.SeriesTemplate;
        
        public MemoryConsumptionChartCustomAdapter(ChartControl chart)
            : base(chart) {
        }

        protected override void ApplyDataSource(object dataSource) {
            Chart.DataSource = ((CustomAdapter)dataSource).DataSource;
        }
        protected override void InitializeSeries(ViewType view, int valuesCount, int pointsCount) {
            Chart.SeriesTemplate.ChangeView(view);
            Chart.SeriesTemplate.DataAdapter = (ISeriesTemplateAdapter)DataAdapter;
        }
    }
    class MemoryConsumptionManualCreationTests : MemoryConsumptionTestsBase {
        public MemoryConsumptionManualCreationTests(ChartControl chart) 
            : base(chart) { 
        }

        protected override void InitializeChart(ViewType view, int valuesCount, int pointsCount) {
            Series series = new Series("s", view);
            series.Points.AddRange(DataGenerator.CreateSeriesPoints(pointsCount, valuesCount));
            Chart.Series.Add(series);
        }
    }
}
