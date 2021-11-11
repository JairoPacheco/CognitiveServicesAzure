
namespace CognitiveServicesSO
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chartAdult = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartMaleFemale = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dgvMaleFemale = new System.Windows.Forms.DataGridView();
            this.TimeMF = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Gender = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvAdult = new System.Windows.Forms.DataGridView();
            this.TimeA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblParallel = new System.Windows.Forms.Label();
            this.lblSecuential = new System.Windows.Forms.Label();
            this.btnRun = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblFile = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.chartAdult)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMaleFemale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaleFemale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdult)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chartAdult
            // 
            chartArea3.Name = "ChartArea1";
            this.chartAdult.ChartAreas.Add(chartArea3);
            legend3.Name = "Legend1";
            this.chartAdult.Legends.Add(legend3);
            this.chartAdult.Location = new System.Drawing.Point(271, 12);
            this.chartAdult.Name = "chartAdult";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            this.chartAdult.Series.Add(series3);
            this.chartAdult.Size = new System.Drawing.Size(240, 171);
            this.chartAdult.TabIndex = 0;
            this.chartAdult.Text = "chartAdult";
            // 
            // chartMaleFemale
            // 
            chartArea4.Name = "ChartArea1";
            this.chartMaleFemale.ChartAreas.Add(chartArea4);
            legend4.Name = "Legend1";
            this.chartMaleFemale.Legends.Add(legend4);
            this.chartMaleFemale.Location = new System.Drawing.Point(12, 12);
            this.chartMaleFemale.Name = "chartMaleFemale";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Pie;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chartMaleFemale.Series.Add(series4);
            this.chartMaleFemale.Size = new System.Drawing.Size(240, 172);
            this.chartMaleFemale.TabIndex = 1;
            this.chartMaleFemale.Text = "chartMaleFemale";
            // 
            // dgvMaleFemale
            // 
            this.dgvMaleFemale.AllowUserToAddRows = false;
            this.dgvMaleFemale.AllowUserToDeleteRows = false;
            this.dgvMaleFemale.AllowUserToResizeColumns = false;
            this.dgvMaleFemale.AllowUserToResizeRows = false;
            this.dgvMaleFemale.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMaleFemale.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeMF,
            this.Gender});
            this.dgvMaleFemale.Location = new System.Drawing.Point(12, 199);
            this.dgvMaleFemale.Name = "dgvMaleFemale";
            this.dgvMaleFemale.Size = new System.Drawing.Size(240, 150);
            this.dgvMaleFemale.TabIndex = 2;
            // 
            // TimeMF
            // 
            this.TimeMF.HeaderText = "Tiempo";
            this.TimeMF.Name = "TimeMF";
            this.TimeMF.Width = 118;
            // 
            // Gender
            // 
            this.Gender.HeaderText = "Género";
            this.Gender.Name = "Gender";
            this.Gender.Width = 118;
            // 
            // dgvAdult
            // 
            this.dgvAdult.AllowUserToAddRows = false;
            this.dgvAdult.AllowUserToDeleteRows = false;
            this.dgvAdult.AllowUserToResizeColumns = false;
            this.dgvAdult.AllowUserToResizeRows = false;
            this.dgvAdult.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAdult.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TimeA});
            this.dgvAdult.Location = new System.Drawing.Point(271, 199);
            this.dgvAdult.Name = "dgvAdult";
            this.dgvAdult.Size = new System.Drawing.Size(240, 150);
            this.dgvAdult.TabIndex = 3;
            // 
            // TimeA
            // 
            this.TimeA.HeaderText = "Tiempo";
            this.TimeA.Name = "TimeA";
            this.TimeA.Width = 237;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Secuencial:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Paralelismo:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblParallel);
            this.groupBox1.Controls.Add(this.lblSecuential);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(526, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(157, 171);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Resultados";
            // 
            // lblParallel
            // 
            this.lblParallel.AutoSize = true;
            this.lblParallel.Location = new System.Drawing.Point(86, 29);
            this.lblParallel.Name = "lblParallel";
            this.lblParallel.Size = new System.Drawing.Size(0, 13);
            this.lblParallel.TabIndex = 8;
            // 
            // lblSecuential
            // 
            this.lblSecuential.AutoSize = true;
            this.lblSecuential.Location = new System.Drawing.Point(86, 16);
            this.lblSecuential.Name = "lblSecuential";
            this.lblSecuential.Size = new System.Drawing.Size(0, 13);
            this.lblSecuential.TabIndex = 7;
            // 
            // btnRun
            // 
            this.btnRun.Location = new System.Drawing.Point(76, 121);
            this.btnRun.Name = "btnRun";
            this.btnRun.Size = new System.Drawing.Size(75, 23);
            this.btnRun.TabIndex = 6;
            this.btnRun.Text = "Ejecutar";
            this.btnRun.UseVisualStyleBackColor = true;
            this.btnRun.Click += new System.EventHandler(this.btnRun_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnLoad);
            this.groupBox2.Controls.Add(this.lblFile);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.btnRun);
            this.groupBox2.Location = new System.Drawing.Point(526, 199);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(157, 150);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Ejecución";
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(66, 36);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 9;
            this.btnLoad.Text = "Cargar";
            this.btnLoad.UseVisualStyleBackColor = true;
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            // 
            // lblFile
            // 
            this.lblFile.AutoSize = true;
            this.lblFile.Location = new System.Drawing.Point(63, 20);
            this.lblFile.Name = "lblFile";
            this.lblFile.Size = new System.Drawing.Size(82, 13);
            this.lblFile.TabIndex = 8;
            this.lblFile.Text = "No hay archivo.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Video:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Video File (*.mp4)|*.mp4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 361);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dgvAdult);
            this.Controls.Add(this.dgvMaleFemale);
            this.Controls.Add(this.chartMaleFemale);
            this.Controls.Add(this.chartAdult);
            this.Name = "Form1";
            this.Text = "Cognitive Services";
            ((System.ComponentModel.ISupportInitialize)(this.chartAdult)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartMaleFemale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMaleFemale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAdult)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chartAdult;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMaleFemale;
        private System.Windows.Forms.DataGridView dgvMaleFemale;
        private System.Windows.Forms.DataGridView dgvAdult;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblParallel;
        private System.Windows.Forms.Label lblSecuential;
        private System.Windows.Forms.Button btnRun;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Label lblFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeA;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeMF;
        private System.Windows.Forms.DataGridViewTextBoxColumn Gender;
    }
}

