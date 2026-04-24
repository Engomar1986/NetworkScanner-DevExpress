
namespace NetworkScannerDevExpress
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.SimpleButton btnScan;
        private DevExpress.XtraEditors.SimpleButton btnStop;
        private DevExpress.XtraEditors.ProgressBarControl progressBarControl1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.btnScan = new DevExpress.XtraEditors.SimpleButton();
            this.btnStop = new DevExpress.XtraEditors.SimpleButton();
            this.progressBarControl1 = new DevExpress.XtraEditors.ProgressBarControl();

            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).BeginInit();

            this.SuspendLayout();

            // gridControl1
            this.gridControl1.Location = new System.Drawing.Point(12, 12);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Size = new System.Drawing.Size(760, 350);
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});

            // gridView1
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";

            // btnScan
            this.btnScan.Location = new System.Drawing.Point(12, 370);
            this.btnScan.Size = new System.Drawing.Size(120, 30);
            this.btnScan.Text = "Start Scan";
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);

            // btnStop
            this.btnStop.Location = new System.Drawing.Point(140, 370);
            this.btnStop.Size = new System.Drawing.Size(120, 30);
            this.btnStop.Text = "Stop";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // progressBarControl1
            this.progressBarControl1.Location = new System.Drawing.Point(270, 370);
            this.progressBarControl1.Size = new System.Drawing.Size(500, 30);

            // Form1
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.progressBarControl1);
            this.Name = "Form1";
            this.Text = "Network Scanner DevExpress";

            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.progressBarControl1.Properties)).EndInit();

            this.ResumeLayout(false);
        }
    }
}
