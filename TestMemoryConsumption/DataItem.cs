using System;
using DevExpress.XtraCharts;

namespace TestMemoryConsumption {
    public static class DataGenerator {
        static object GenerateDataSource1(int pointCount) {
            DataItem1[] dataSource = new DataItem1[pointCount];
            for(int i = 0; i < pointCount; i++)
                dataSource[i] = new DataItem1(i, i - 1);
            return dataSource;
        }
        static object GenerateDataSource2(int pointCount) {
            DataItem2[] dataSource = new DataItem2[pointCount];
            for(int i = 0; i < pointCount; i++)
                dataSource[i] = new DataItem2(i, i - 1, i);
            return dataSource;
        }
        static object GenerateDataSource3(int pointCount) {
            DataItem3[] dataSource = new DataItem3[pointCount];
            for(int i = 0; i < pointCount; i++)
                dataSource[i] = new DataItem3(i, i - 1, i, i + 1);
            return dataSource;
        }
        static object GenerateDataSource4(int pointCount) {
            DataItem4[] dataSource = new DataItem4[pointCount];
            for(int i = 0; i < pointCount; i++)
                dataSource[i] = new DataItem4(i, i - 1, i, i + 1, i + 2);
            return dataSource;
        }
        static object GenerateDataSource7(int pointCount) {
            DataItem7[] dataSource = new DataItem7[pointCount];
            for(int i = 0; i < pointCount; i++)
                dataSource[i] = new DataItem7(i, i - 1, i, i + 1, i + 2, i + 3, i + 4, i + 5);
            return dataSource;
        }
        public static object GenerateDataSource(int pointCount, int valuesCount) {
            switch(valuesCount) {
                case 1: return GenerateDataSource1(pointCount);
                case 2: return GenerateDataSource2(pointCount);
                case 3: return GenerateDataSource3(pointCount);
                case 4: return GenerateDataSource4(pointCount);
                case 7: return GenerateDataSource7(pointCount);
                default: throw new Exception();
            }
        }
        public static SeriesPoint[] CreateSeriesPoints(int pointsCount, int valuesCount) {
            SeriesPoint[] result = new SeriesPoint[pointsCount];
            for(int i = 0; i < pointsCount; i++)
                switch(valuesCount) {
                    case 1:
                        result[i] = new SeriesPoint(i, i - 1);
                        break;
                    case 2:
                        result[i] = new SeriesPoint(i, i - 1, i);
                        break;
                    case 3:
                        result[i] = new SeriesPoint(i, i - 1, i, i + 1);
                        break;
                    case 4:
                        result[i] = new SeriesPoint(i, i - 1, i, i + 1, i + 2);
                        break;
                    case 7:
                        result[i] = new SeriesPoint(i, i - 1, i, i + 1, i + 2, i + 3, i + 4, i + 5);
                        break;
                    default:
                        throw new Exception();
                }
            return result;
        }
        public static string[] GetValueMembers(int n) {
            string[] valueNames = new string[n];
            for(int i = 0; i < n; i++)
                valueNames[i] = "Value" + (i + 1).ToString();
            return valueNames;
        }
    }

    public struct DataItem1 {
        public double Argument { get; }
        public double Value1 { get; }

        public DataItem1(double argument, double value) {
            Argument = argument;
            Value1 = value;
        }
    }
    public struct DataItem2 {
        public double Argument { get; }
        public double Value1 { get; }
        public double Value2 { get; }

        public DataItem2(double argument, double value1, double value2) {
            Argument = argument;
            Value1 = value1;
            Value2 = value2;
        }
    }
    public struct DataItem3 {
        public double Argument { get; }
        public double Value1 { get; }
        public double Value2 { get; }
        public double Value3 { get; }

        public DataItem3(double argument, double value1, double value2, double value3) {
            Argument = argument;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }
    }
    public struct DataItem4 {
        public double Argument { get; }
        public double Value1 { get; }
        public double Value2 { get; }
        public double Value3 { get; }
        public double Value4 { get; }

        public DataItem4(double argument, double value1, double value2, double value3, double value4) {
            Argument = argument;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            Value4 = value4;
        }
    }
    public struct DataItem7 {
        public double Argument { get; }
        public double Value1 { get; }
        public double Value2 { get; }
        public double Value3 { get; }
        public double Value4 { get; }
        public double Value5 { get; }
        public double Value6 { get; }
        public double Value7 { get; }

        public DataItem7(double argument, double value1, double value2, double value3, double value4, double value5, double value6, double value7) {
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
