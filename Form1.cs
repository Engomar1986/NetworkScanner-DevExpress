
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace NetworkScannerDevExpress
{
    public partial class Form1 : XtraForm
    {
        private BindingList<NetworkDevice> devices;
        private CancellationTokenSource cts;

        public Form1()
        {
            InitializeComponent();
            InitGrid();
        }

        private void InitGrid()
        {
            devices = new BindingList<NetworkDevice>();
            gridControl1.DataSource = devices;

            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ShowAutoFilterRow = true;
            gridView1.OptionsFind.AlwaysVisible = true;

            gridView1.PopulateColumns();

            // ترتيب الأعمدة
            if (gridView1.Columns["IP"] != null) gridView1.Columns["IP"].VisibleIndex = 0;
            if (gridView1.Columns["HostName"] != null) gridView1.Columns["HostName"].VisibleIndex = 1;
            if (gridView1.Columns["MAC"] != null) gridView1.Columns["MAC"].VisibleIndex = 2;
            if (gridView1.Columns["DeviceType"] != null) gridView1.Columns["DeviceType"].VisibleIndex = 3;
            if (gridView1.Columns["Status"] != null) gridView1.Columns["Status"].VisibleIndex = 4;

            gridView1.BestFitColumns();

            // تلوين حسب نوع الجهاز
            gridView1.RowCellStyle += (s, e) =>
            {
                if (e.Column.FieldName == "DeviceType")
                {
                    string val = e.CellValue?.ToString();

                    if (val == "Router")
                        e.Appearance.ForeColor = System.Drawing.Color.Red;
                    else if (val == "Printer")
                        e.Appearance.ForeColor = System.Drawing.Color.Blue;
                    else if (val == "Mobile")
                        e.Appearance.ForeColor = System.Drawing.Color.Green;
                }
            };
        }

        private async void btnScan_Click(object sender, EventArgs e)
        {
            devices.Clear();
            progressBarControl1.Position = 0;

            btnScan.Enabled = false;
            btnStop.Enabled = true;

            cts = new CancellationTokenSource();

            try
            {
                await NetworkScanner.ScanAsync(
                    device =>
                    {
                        // تحديث UI
                        if (!IsDisposed)
                        {
                            Invoke(new Action(() =>
                            {
                                devices.Add(device);
                            }));
                        }
                    },
                    progress =>
                    {
                        if (!IsDisposed)
                        {
                            Invoke(new Action(() =>
                            {
                                progressBarControl1.Position = progress;
                            }));
                        }
                    },
                    cts.Token);
            }
            catch (OperationCanceledException)
            {
                XtraMessageBox.Show("تم إيقاف البحث ⛔");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("خطأ: " + ex.Message);
            }

            btnScan.Enabled = true;
            btnStop.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cts?.Cancel();
        }
    }
}
