namespace TestMemoryConsumption {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.tablePanel1 = new DevExpress.Utils.Layout.TablePanel();
            this.stackPanel2 = new DevExpress.Utils.Layout.StackPanel();
            this.stackPanel1 = new DevExpress.Utils.Layout.StackPanel();
            this.seriesTypeComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.pointsCountComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.chartControl1 = new DevExpress.XtraCharts.ChartControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.seriesCreationTypeComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.resultLabel = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).BeginInit();
            this.tablePanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel2)).BeginInit();
            this.stackPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).BeginInit();
            this.stackPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.seriesTypeComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsCountComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seriesCreationTypeComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tablePanel1
            // 
            this.tablePanel1.Columns.AddRange(new DevExpress.Utils.Layout.TablePanelColumn[] {
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 50F),
            new DevExpress.Utils.Layout.TablePanelColumn(DevExpress.Utils.Layout.TablePanelEntityStyle.Relative, 10F)});
            this.tablePanel1.Controls.Add(this.stackPanel2);
            this.tablePanel1.Controls.Add(this.stackPanel1);
            this.tablePanel1.Controls.Add(this.chartControl1);
            this.tablePanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePanel1.Location = new System.Drawing.Point(0, 0);
            this.tablePanel1.Name = "tablePanel1";
            this.tablePanel1.Rows.AddRange(new DevExpress.Utils.Layout.TablePanelRow[] {
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F),
            new DevExpress.Utils.Layout.TablePanelRow(DevExpress.Utils.Layout.TablePanelEntityStyle.Absolute, 26F)});
            this.tablePanel1.Size = new System.Drawing.Size(800, 450);
            this.tablePanel1.TabIndex = 0;
            // 
            // stackPanel2
            // 
            this.tablePanel1.SetColumn(this.stackPanel2, 0);
            this.stackPanel2.Controls.Add(this.resultLabel);
            this.stackPanel2.Location = new System.Drawing.Point(3, 3);
            this.stackPanel2.Name = "stackPanel2";
            this.tablePanel1.SetRow(this.stackPanel2, 0);
            this.stackPanel2.Size = new System.Drawing.Size(661, 20);
            this.stackPanel2.TabIndex = 2;
            // 
            // stackPanel1
            // 
            this.tablePanel1.SetColumn(this.stackPanel1, 1);
            this.stackPanel1.Controls.Add(this.seriesTypeComboBox);
            this.stackPanel1.Controls.Add(this.pointsCountComboBox);
            this.stackPanel1.Controls.Add(this.seriesCreationTypeComboBox);
            this.stackPanel1.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.stackPanel1.LayoutDirection = DevExpress.Utils.Layout.StackPanelLayoutDirection.TopDown;
            this.stackPanel1.Location = new System.Drawing.Point(667, 3);
            this.stackPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.stackPanel1.Name = "stackPanel1";
            this.tablePanel1.SetRow(this.stackPanel1, 0);
            this.tablePanel1.SetRowSpan(this.stackPanel1, 2);
            this.stackPanel1.Size = new System.Drawing.Size(133, 444);
            this.stackPanel1.TabIndex = 1;
            // 
            // seriesTypeComboBox
            // 
            this.seriesTypeComboBox.Location = new System.Drawing.Point(1, 3);
            this.seriesTypeComboBox.Name = "seriesTypeComboBox";
            this.seriesTypeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seriesTypeComboBox.Size = new System.Drawing.Size(130, 20);
            this.seriesTypeComboBox.TabIndex = 4;
            this.seriesTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.SeriesTypeComboBox_SelectedIndexChanged);
            // 
            // pointsCountComboBox
            // 
            this.pointsCountComboBox.Location = new System.Drawing.Point(1, 29);
            this.pointsCountComboBox.Name = "pointsCountComboBox";
            this.pointsCountComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.pointsCountComboBox.Size = new System.Drawing.Size(130, 20);
            this.pointsCountComboBox.TabIndex = 5;
            this.pointsCountComboBox.SelectedIndexChanged += new System.EventHandler(this.PointsCountComboBox_SelectedIndexChanged_1);
            // 
            // chartControl1
            // 
            this.tablePanel1.SetColumn(this.chartControl1, 0);
            this.chartControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartControl1.Location = new System.Drawing.Point(3, 29);
            this.chartControl1.Name = "chartControl1";
            this.tablePanel1.SetRow(this.chartControl1, 1);
            this.chartControl1.SeriesSerializable = new DevExpress.XtraCharts.Series[0];
            this.chartControl1.Size = new System.Drawing.Size(661, 418);
            this.chartControl1.TabIndex = 0;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // seriesCreationTypeComboBox
            // 
            this.seriesCreationTypeComboBox.Location = new System.Drawing.Point(1, 55);
            this.seriesCreationTypeComboBox.Name = "seriesCreationTypeComboBox";
            this.seriesCreationTypeComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seriesCreationTypeComboBox.Size = new System.Drawing.Size(130, 20);
            this.seriesCreationTypeComboBox.TabIndex = 6;
            this.seriesCreationTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.SeriesCreationTypeComboBox_SelectedIndexChanged_1);
            // 
            // resultLabel
            // 
            this.resultLabel.Location = new System.Drawing.Point(3, 3);
            this.resultLabel.Name = "resultLabel";
            this.resultLabel.Size = new System.Drawing.Size(0, 13);
            this.resultLabel.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tablePanel1);
            this.Name = "Form1";
            this.Text = "ChartControl Memory Consumption";
            ((System.ComponentModel.ISupportInitialize)(this.tablePanel1)).EndInit();
            this.tablePanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel2)).EndInit();
            this.stackPanel2.ResumeLayout(false);
            this.stackPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.stackPanel1)).EndInit();
            this.stackPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.seriesTypeComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pointsCountComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seriesCreationTypeComboBox.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.Utils.Layout.TablePanel tablePanel1;
        private DevExpress.Utils.Layout.StackPanel stackPanel1;
        private DevExpress.XtraCharts.ChartControl chartControl1;
        private DevExpress.Utils.Layout.StackPanel stackPanel2;
        private System.Windows.Forms.Timer timer1;
        private DevExpress.XtraEditors.ComboBoxEdit seriesTypeComboBox;
        private DevExpress.XtraEditors.ComboBoxEdit pointsCountComboBox;
        private DevExpress.XtraEditors.ComboBoxEdit seriesCreationTypeComboBox;
        private DevExpress.XtraEditors.LabelControl resultLabel;
    }
}

