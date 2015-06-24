using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
    public partial class ExportHL7Form : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Fields

        //private Dictionary<string, IDalHL7> _Dals = null;

        #endregion

        #region Constructor

        public ExportHL7Form(IList<string> hl7Versions)
        {
            InitializeComponent();

            //_Dals = dals;

            ListViewItem item;
            listViewDals.Items.Clear();
            foreach (var version in hl7Versions)
            {
                item = new ListViewItem(version);
                //item.Checked = true;
                listViewDals.Items.Add(item);
            }

            this.textBoxExportFolder.Text = ConfigurationManager.AppSettings["exportPath"];

            //this.radioButtonSql.Checked = true;

            this.textBoxQuery.Text = @"SELECT *
-- h.MessageControlId, h.MessageTimeStamp, h.MessageType, h.EventType, h.Message, h.FileName, h.Creator, a.EventTimeStamp, a.PatientId, a.VisitNumber, a.AdmissionUnitNumber, a.CampusCode, a.NursingUnitNumber, a.RoomNumber, a.BedNumber, a.DoctorNumber
FROM HL7 h with (nolock)
INNER JOIN ADT a with (nolock) ON h.MessageControlId = a.MessageControlId
WHERE a.VisitNumber IN  --HL7 FULL SEQUENCE
(
    SELECT a2.VisitNumber
    FROM HL7 h2 with (nolock)
    INNER JOIN ADT a2 with (nolock) ON h2.MessageControlId = a2.MessageControlId
    WHERE h2.MessageTimeStamp >= CONVERT(DATETIME, '" + DateTime.Now.ToString("yyyy-MM-dd", new DateTimeFormatInfo()) + @"', 102) and h2.MessageTimeStamp < getdate()
)
ORDER BY h.MessageControlId ASC";
        }

        #endregion

        #region Events

        private void ExportHL7Form_Load(object sender, EventArgs e)
        {
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = this.textBoxExportFolder.Text;

            // Show the FolderBrowserDialog.
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.textBoxExportFolder.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void buttonExport_Click(object sender, EventArgs e)
        {
            if (!this.textBoxExportFolder.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
                this.textBoxExportFolder.Text += Path.DirectorySeparatorChar;

            if (!Directory.Exists(this.textBoxExportFolder.Text))
            {
                MessageBox.Show(string.Format("The export folder '{0}' doesn't exist!", this.textBoxExportFolder.Text), "Invalid export folder!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (this.listViewDals.CheckedItems.Count == 0)
            {
                MessageBox.Show(string.Format("You need to select a datalayer!", this.textBoxExportFolder.Text), "No datalayer selected!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (this.listViewDals.CheckedItems.Count > 1)
            {
                MessageBox.Show(string.Format("You can only select one datalayer!", this.textBoxExportFolder.Text), "Multiple datalayers selected!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            this.buttonExport.Enabled = false;

            backgroundWorker.RunWorkerAsync();
        }

        #endregion

        #region Invoke Methods

        private string GetCheckedListviewItem(int index)
        {
            if (InvokeRequired)
            {
                GetCheckedListviewItemDelegate del = delegate(int i) { return GetCheckedListviewItem(i); };
                return (string)EndInvoke(BeginInvoke(del, index));
            }
            else
                return this.listViewDals.CheckedItems[0].Text;
        }

        #endregion

        #region BackgroundWorker

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            StreamWriter sr = null;
            string fileText = null;

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings[GetCheckedListviewItem(0)].ConnectionString;
                using (IDbConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    IDbCommand command = connection.CreateCommand();
                    command.CommandText = textBoxQuery.Text;
                    command.CommandTimeout = 3600; //1 hour

                    IDataReader reader = command.ExecuteReader();

                    // Call Read before accessing data.
                    while (reader.Read())
                    {
                        if (File.Exists(this.textBoxExportFolder.Text + (string)reader["FileName"]))
                            File.Delete(this.textBoxExportFolder.Text + (string)reader["FileName"]);

                        using (sr = new StreamWriter(File.Create(this.textBoxExportFolder.Text + (string)reader["FileName"]), Encoding.GetEncoding(1252))) //Latin1_General_CI_AS = 1252 (default codepage SQL2000)
                        {
                            fileText = (reader["Message"] as string).Replace("\r\n", "\r").Replace("\r\r", "\r").Replace("\r", "\r\n");
                            sr.Write(fileText);
                            sr.Flush();
                        }
                    }

                    // Call Close when done reading.
                    reader.Close();
                }

            }
            catch (Exception ex)
            {
                e.Cancel = true;
                MessageBox.Show(ex.ToString(), "Export exception!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                MessageBox.Show("Export completed.", "Export done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.buttonExport.Enabled = true;
            this.Cursor = Cursors.Default;
        }

        #endregion

        #region delegates

        public delegate string GetCheckedListviewItemDelegate(int index);

        #endregion
    }
}

