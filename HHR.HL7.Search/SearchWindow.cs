using AsyncPoco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
    public class SearchWindow : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        #region Fields

        System.Windows.Forms.Label labelPatientId;
        System.Windows.Forms.TextBox textBoxPatientId;
        System.Windows.Forms.Button buttonSearch;
        System.Windows.Forms.ErrorProvider errorProvider;
        System.ComponentModel.IContainer components = null;
        System.Windows.Forms.TextBox textBoxFamilyName;
        System.Windows.Forms.TextBox textBoxGivenName;
        System.Windows.Forms.Label labelFamilyName;
        System.Windows.Forms.Label labelGivenName;
        System.Windows.Forms.TabControl tabControlPatient;
        System.Windows.Forms.TabPage tabPagePatientId;
        System.Windows.Forms.TabPage tabPagePatientName;
        System.Windows.Forms.TabPage tabPageVisitNumber;
        System.Windows.Forms.TabPage tabPageDoctorNumber;
        System.Windows.Forms.TextBox textBoxVisitNumbers;
        System.Windows.Forms.Label labelVisitNumber;
        System.Windows.Forms.TextBox textBoxDoctorNumbers;
        System.Windows.Forms.Label labelDoctorNumber;
        System.Windows.Forms.RadioButton radioButtonVisitNumberVisits;
        System.Windows.Forms.RadioButton radioButtonVisitNumberPatients;
        System.Windows.Forms.RadioButton radioButtonPatientIdInsurances;
        System.Windows.Forms.RadioButton radioButtonPatientIdVisits;
        System.Windows.Forms.RadioButton radioButtonPatientIdPatient;
        System.Windows.Forms.RadioButton radioButtonVisitNumberVisitDetails;
        public ListView listViewDals;
        ShowState visitShowState = ShowState.VisitDetails;
        ContextMenuStrip contextMenuStrip1;
        ToolStripMenuItem unSelectAllToolStripMenuItem;
        Label label1;
        ComboBox cboBiztalkSendPorts;
        CheckBox chkShowOnlyLastBiztalkHL7s;
        TabPage tabPageBedOccupation;
        TextBox textBoxWardIds;
        Label label2;
        ShowState patientShowState = ShowState.Visists;

        #endregion

        #region Constructor

        public SearchWindow(IList<string> dals)
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            ListViewItem item;
            listViewDals.Items.Clear();
            foreach (string dal in dals)
            {
                item = new ListViewItem(dal);
                item.Checked = (new string[] { "Oazis", /*"ZISv23",*/ /*"HL7v21",*/ "HL7v23"/*, "Biztalk"*/ }).Contains(dal);
                listViewDals.Items.Add(item);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("test");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchWindow));
            this.labelPatientId = new System.Windows.Forms.Label();
            this.textBoxPatientId = new System.Windows.Forms.TextBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.textBoxFamilyName = new System.Windows.Forms.TextBox();
            this.textBoxGivenName = new System.Windows.Forms.TextBox();
            this.labelFamilyName = new System.Windows.Forms.Label();
            this.labelGivenName = new System.Windows.Forms.Label();
            this.tabControlPatient = new System.Windows.Forms.TabControl();
            this.tabPagePatientId = new System.Windows.Forms.TabPage();
            this.radioButtonPatientIdInsurances = new System.Windows.Forms.RadioButton();
            this.radioButtonPatientIdVisits = new System.Windows.Forms.RadioButton();
            this.radioButtonPatientIdPatient = new System.Windows.Forms.RadioButton();
            this.tabPagePatientName = new System.Windows.Forms.TabPage();
            this.tabPageVisitNumber = new System.Windows.Forms.TabPage();
            this.radioButtonVisitNumberVisitDetails = new System.Windows.Forms.RadioButton();
            this.radioButtonVisitNumberVisits = new System.Windows.Forms.RadioButton();
            this.radioButtonVisitNumberPatients = new System.Windows.Forms.RadioButton();
            this.textBoxVisitNumbers = new System.Windows.Forms.TextBox();
            this.labelVisitNumber = new System.Windows.Forms.Label();
            this.tabPageDoctorNumber = new System.Windows.Forms.TabPage();
            this.textBoxDoctorNumbers = new System.Windows.Forms.TextBox();
            this.labelDoctorNumber = new System.Windows.Forms.Label();
            this.tabPageBedOccupation = new System.Windows.Forms.TabPage();
            this.textBoxWardIds = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.listViewDals = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.unSelectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cboBiztalkSendPorts = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkShowOnlyLastBiztalkHL7s = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.tabControlPatient.SuspendLayout();
            this.tabPagePatientId.SuspendLayout();
            this.tabPagePatientName.SuspendLayout();
            this.tabPageVisitNumber.SuspendLayout();
            this.tabPageDoctorNumber.SuspendLayout();
            this.tabPageBedOccupation.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelPatientId
            // 
            this.labelPatientId.Location = new System.Drawing.Point(8, 8);
            this.labelPatientId.Name = "labelPatientId";
            this.labelPatientId.Size = new System.Drawing.Size(90, 16);
            this.labelPatientId.TabIndex = 0;
            this.labelPatientId.Text = "PatientId:";
            // 
            // textBoxPatientId
            // 
            this.textBoxPatientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPatientId.Location = new System.Drawing.Point(100, 8);
            this.textBoxPatientId.MaxLength = 10;
            this.textBoxPatientId.Multiline = true;
            this.textBoxPatientId.Name = "textBoxPatientId";
            this.textBoxPatientId.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxPatientId.Size = new System.Drawing.Size(400, 66);
            this.textBoxPatientId.TabIndex = 0;
            this.textBoxPatientId.TextChanged += new System.EventHandler(this.InputValidating);
            this.textBoxPatientId.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxPatientId_KeyDown);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSearch.Location = new System.Drawing.Point(400, 556);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(128, 23);
            this.buttonSearch.TabIndex = 5;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // textBoxFamilyName
            // 
            this.textBoxFamilyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFamilyName.Location = new System.Drawing.Point(104, 32);
            this.textBoxFamilyName.MaxLength = 50;
            this.textBoxFamilyName.Name = "textBoxFamilyName";
            this.textBoxFamilyName.Size = new System.Drawing.Size(394, 20);
            this.textBoxFamilyName.TabIndex = 19;
            // 
            // textBoxGivenName
            // 
            this.textBoxGivenName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGivenName.Location = new System.Drawing.Point(104, 8);
            this.textBoxGivenName.MaxLength = 50;
            this.textBoxGivenName.Name = "textBoxGivenName";
            this.textBoxGivenName.Size = new System.Drawing.Size(394, 20);
            this.textBoxGivenName.TabIndex = 18;
            // 
            // labelFamilyName
            // 
            this.labelFamilyName.Location = new System.Drawing.Point(8, 32);
            this.labelFamilyName.Name = "labelFamilyName";
            this.labelFamilyName.Size = new System.Drawing.Size(96, 16);
            this.labelFamilyName.TabIndex = 20;
            this.labelFamilyName.Text = "FamilyName:";
            // 
            // labelGivenName
            // 
            this.labelGivenName.Location = new System.Drawing.Point(8, 8);
            this.labelGivenName.Name = "labelGivenName";
            this.labelGivenName.Size = new System.Drawing.Size(96, 16);
            this.labelGivenName.TabIndex = 17;
            this.labelGivenName.Text = "GivenName:";
            // 
            // tabControlPatient
            // 
            this.tabControlPatient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlPatient.Controls.Add(this.tabPagePatientId);
            this.tabControlPatient.Controls.Add(this.tabPagePatientName);
            this.tabControlPatient.Controls.Add(this.tabPageVisitNumber);
            this.tabControlPatient.Controls.Add(this.tabPageDoctorNumber);
            this.tabControlPatient.Controls.Add(this.tabPageBedOccupation);
            this.tabControlPatient.Location = new System.Drawing.Point(12, 8);
            this.tabControlPatient.Name = "tabControlPatient";
            this.tabControlPatient.SelectedIndex = 0;
            this.tabControlPatient.Size = new System.Drawing.Size(516, 179);
            this.tabControlPatient.TabIndex = 21;
            this.tabControlPatient.SelectedIndexChanged += new System.EventHandler(this.tabControlPatient_SelectedIndexChanged);
            // 
            // tabPagePatientId
            // 
            this.tabPagePatientId.Controls.Add(this.radioButtonPatientIdInsurances);
            this.tabPagePatientId.Controls.Add(this.radioButtonPatientIdVisits);
            this.tabPagePatientId.Controls.Add(this.radioButtonPatientIdPatient);
            this.tabPagePatientId.Controls.Add(this.labelPatientId);
            this.tabPagePatientId.Controls.Add(this.textBoxPatientId);
            this.tabPagePatientId.Location = new System.Drawing.Point(4, 22);
            this.tabPagePatientId.Name = "tabPagePatientId";
            this.tabPagePatientId.Size = new System.Drawing.Size(508, 153);
            this.tabPagePatientId.TabIndex = 0;
            this.tabPagePatientId.Text = "PatientId";
            // 
            // radioButtonPatientIdInsurances
            // 
            this.radioButtonPatientIdInsurances.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonPatientIdInsurances.Location = new System.Drawing.Point(104, 112);
            this.radioButtonPatientIdInsurances.Name = "radioButtonPatientIdInsurances";
            this.radioButtonPatientIdInsurances.Size = new System.Drawing.Size(394, 16);
            this.radioButtonPatientIdInsurances.TabIndex = 33;
            this.radioButtonPatientIdInsurances.Text = "Insurances";
            this.radioButtonPatientIdInsurances.Visible = false;
            this.radioButtonPatientIdInsurances.CheckedChanged += new System.EventHandler(this.radioButtonPatientId_CheckedChanged);
            // 
            // radioButtonPatientIdVisits
            // 
            this.radioButtonPatientIdVisits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonPatientIdVisits.Checked = true;
            this.radioButtonPatientIdVisits.Location = new System.Drawing.Point(104, 96);
            this.radioButtonPatientIdVisits.Name = "radioButtonPatientIdVisits";
            this.radioButtonPatientIdVisits.Size = new System.Drawing.Size(394, 16);
            this.radioButtonPatientIdVisits.TabIndex = 32;
            this.radioButtonPatientIdVisits.TabStop = true;
            this.radioButtonPatientIdVisits.Text = "Visits";
            this.radioButtonPatientIdVisits.CheckedChanged += new System.EventHandler(this.radioButtonPatientId_CheckedChanged);
            // 
            // radioButtonPatientIdPatient
            // 
            this.radioButtonPatientIdPatient.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonPatientIdPatient.Location = new System.Drawing.Point(104, 80);
            this.radioButtonPatientIdPatient.Name = "radioButtonPatientIdPatient";
            this.radioButtonPatientIdPatient.Size = new System.Drawing.Size(394, 16);
            this.radioButtonPatientIdPatient.TabIndex = 31;
            this.radioButtonPatientIdPatient.Text = "Patient";
            this.radioButtonPatientIdPatient.CheckedChanged += new System.EventHandler(this.radioButtonPatientId_CheckedChanged);
            // 
            // tabPagePatientName
            // 
            this.tabPagePatientName.Controls.Add(this.textBoxFamilyName);
            this.tabPagePatientName.Controls.Add(this.textBoxGivenName);
            this.tabPagePatientName.Controls.Add(this.labelFamilyName);
            this.tabPagePatientName.Controls.Add(this.labelGivenName);
            this.tabPagePatientName.Location = new System.Drawing.Point(4, 22);
            this.tabPagePatientName.Name = "tabPagePatientName";
            this.tabPagePatientName.Size = new System.Drawing.Size(508, 153);
            this.tabPagePatientName.TabIndex = 1;
            this.tabPagePatientName.Text = "PatientName";
            // 
            // tabPageVisitNumber
            // 
            this.tabPageVisitNumber.Controls.Add(this.radioButtonVisitNumberVisitDetails);
            this.tabPageVisitNumber.Controls.Add(this.radioButtonVisitNumberVisits);
            this.tabPageVisitNumber.Controls.Add(this.radioButtonVisitNumberPatients);
            this.tabPageVisitNumber.Controls.Add(this.textBoxVisitNumbers);
            this.tabPageVisitNumber.Controls.Add(this.labelVisitNumber);
            this.tabPageVisitNumber.Location = new System.Drawing.Point(4, 22);
            this.tabPageVisitNumber.Name = "tabPageVisitNumber";
            this.tabPageVisitNumber.Size = new System.Drawing.Size(508, 153);
            this.tabPageVisitNumber.TabIndex = 2;
            this.tabPageVisitNumber.Text = "VisitNumber";
            // 
            // radioButtonVisitNumberVisitDetails
            // 
            this.radioButtonVisitNumberVisitDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisitNumberVisitDetails.Checked = true;
            this.radioButtonVisitNumberVisitDetails.Location = new System.Drawing.Point(104, 124);
            this.radioButtonVisitNumberVisitDetails.Name = "radioButtonVisitNumberVisitDetails";
            this.radioButtonVisitNumberVisitDetails.Size = new System.Drawing.Size(394, 16);
            this.radioButtonVisitNumberVisitDetails.TabIndex = 32;
            this.radioButtonVisitNumberVisitDetails.TabStop = true;
            this.radioButtonVisitNumberVisitDetails.Text = "VisitDetails";
            this.radioButtonVisitNumberVisitDetails.CheckedChanged += new System.EventHandler(this.radioButtonVisitNumber_CheckedChanged);
            // 
            // radioButtonVisitNumberVisits
            // 
            this.radioButtonVisitNumberVisits.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisitNumberVisits.Location = new System.Drawing.Point(104, 102);
            this.radioButtonVisitNumberVisits.Name = "radioButtonVisitNumberVisits";
            this.radioButtonVisitNumberVisits.Size = new System.Drawing.Size(394, 16);
            this.radioButtonVisitNumberVisits.TabIndex = 26;
            this.radioButtonVisitNumberVisits.Text = "Visits";
            this.radioButtonVisitNumberVisits.CheckedChanged += new System.EventHandler(this.radioButtonVisitNumber_CheckedChanged);
            // 
            // radioButtonVisitNumberPatients
            // 
            this.radioButtonVisitNumberPatients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonVisitNumberPatients.Location = new System.Drawing.Point(104, 80);
            this.radioButtonVisitNumberPatients.Name = "radioButtonVisitNumberPatients";
            this.radioButtonVisitNumberPatients.Size = new System.Drawing.Size(394, 16);
            this.radioButtonVisitNumberPatients.TabIndex = 25;
            this.radioButtonVisitNumberPatients.Text = "Patient";
            this.radioButtonVisitNumberPatients.CheckedChanged += new System.EventHandler(this.radioButtonVisitNumber_CheckedChanged);
            // 
            // textBoxVisitNumbers
            // 
            this.textBoxVisitNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVisitNumbers.Location = new System.Drawing.Point(102, 8);
            this.textBoxVisitNumbers.Multiline = true;
            this.textBoxVisitNumbers.Name = "textBoxVisitNumbers";
            this.textBoxVisitNumbers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxVisitNumbers.Size = new System.Drawing.Size(396, 58);
            this.textBoxVisitNumbers.TabIndex = 24;
            this.textBoxVisitNumbers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxVisitNumbers_KeyDown);
            // 
            // labelVisitNumber
            // 
            this.labelVisitNumber.Location = new System.Drawing.Point(6, 8);
            this.labelVisitNumber.Name = "labelVisitNumber";
            this.labelVisitNumber.Size = new System.Drawing.Size(88, 16);
            this.labelVisitNumber.TabIndex = 23;
            this.labelVisitNumber.Text = "VisitNumber(s):";
            // 
            // tabPageDoctorNumber
            // 
            this.tabPageDoctorNumber.Controls.Add(this.textBoxDoctorNumbers);
            this.tabPageDoctorNumber.Controls.Add(this.labelDoctorNumber);
            this.tabPageDoctorNumber.Location = new System.Drawing.Point(4, 22);
            this.tabPageDoctorNumber.Name = "tabPageDoctorNumber";
            this.tabPageDoctorNumber.Size = new System.Drawing.Size(508, 153);
            this.tabPageDoctorNumber.TabIndex = 3;
            this.tabPageDoctorNumber.Text = "DoctorNumber";
            // 
            // textBoxDoctorNumbers
            // 
            this.textBoxDoctorNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDoctorNumbers.Location = new System.Drawing.Point(102, 8);
            this.textBoxDoctorNumbers.Multiline = true;
            this.textBoxDoctorNumbers.Name = "textBoxDoctorNumbers";
            this.textBoxDoctorNumbers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDoctorNumbers.Size = new System.Drawing.Size(396, 58);
            this.textBoxDoctorNumbers.TabIndex = 26;
            this.textBoxDoctorNumbers.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxDoctorNumbers_KeyDown);
            // 
            // labelDoctorNumber
            // 
            this.labelDoctorNumber.Location = new System.Drawing.Point(6, 8);
            this.labelDoctorNumber.Name = "labelDoctorNumber";
            this.labelDoctorNumber.Size = new System.Drawing.Size(98, 16);
            this.labelDoctorNumber.TabIndex = 25;
            this.labelDoctorNumber.Text = "DoctorNumber(s):";
            // 
            // tabPageBedOccupation
            // 
            this.tabPageBedOccupation.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageBedOccupation.Controls.Add(this.textBoxWardIds);
            this.tabPageBedOccupation.Controls.Add(this.label2);
            this.tabPageBedOccupation.Location = new System.Drawing.Point(4, 22);
            this.tabPageBedOccupation.Name = "tabPageBedOccupation";
            this.tabPageBedOccupation.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageBedOccupation.Size = new System.Drawing.Size(508, 153);
            this.tabPageBedOccupation.TabIndex = 4;
            this.tabPageBedOccupation.Text = "BedOccupation";
            // 
            // textBoxWardIds
            // 
            this.textBoxWardIds.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxWardIds.Location = new System.Drawing.Point(106, 6);
            this.textBoxWardIds.Multiline = true;
            this.textBoxWardIds.Name = "textBoxWardIds";
            this.textBoxWardIds.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxWardIds.Size = new System.Drawing.Size(396, 58);
            this.textBoxWardIds.TabIndex = 28;
            this.textBoxWardIds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxWardIds_KeyDown);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(10, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 16);
            this.label2.TabIndex = 27;
            this.label2.Text = "WardId(s):";
            // 
            // listViewDals
            // 
            this.listViewDals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDals.CheckBoxes = true;
            this.listViewDals.ContextMenuStrip = this.contextMenuStrip1;
            this.listViewDals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem1.StateImageIndex = 0;
            this.listViewDals.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewDals.Location = new System.Drawing.Point(114, 244);
            this.listViewDals.Name = "listViewDals";
            this.listViewDals.Size = new System.Drawing.Size(414, 306);
            this.listViewDals.TabIndex = 24;
            this.listViewDals.UseCompatibleStateImageBehavior = false;
            this.listViewDals.View = System.Windows.Forms.View.List;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unSelectAllToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(146, 26);
            // 
            // unSelectAllToolStripMenuItem
            // 
            this.unSelectAllToolStripMenuItem.Name = "unSelectAllToolStripMenuItem";
            this.unSelectAllToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.unSelectAllToolStripMenuItem.Text = "(Un)Select All";
            this.unSelectAllToolStripMenuItem.Click += new System.EventHandler(this.unSelectAllToolStripMenuItem_Click);
            // 
            // cboBiztalkSendPorts
            // 
            this.cboBiztalkSendPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBiztalkSendPorts.FormattingEnabled = true;
            this.cboBiztalkSendPorts.Location = new System.Drawing.Point(84, 193);
            this.cboBiztalkSendPorts.Name = "cboBiztalkSendPorts";
            this.cboBiztalkSendPorts.Size = new System.Drawing.Size(440, 21);
            this.cboBiztalkSendPorts.TabIndex = 26;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 196);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 27;
            this.label1.Text = "Biztalk HL7:";
            // 
            // chkShowOnlyLastBiztalkHL7s
            // 
            this.chkShowOnlyLastBiztalkHL7s.AutoSize = true;
            this.chkShowOnlyLastBiztalkHL7s.Location = new System.Drawing.Point(12, 220);
            this.chkShowOnlyLastBiztalkHL7s.Name = "chkShowOnlyLastBiztalkHL7s";
            this.chkShowOnlyLastBiztalkHL7s.Size = new System.Drawing.Size(179, 17);
            this.chkShowOnlyLastBiztalkHL7s.TabIndex = 28;
            this.chkShowOnlyLastBiztalkHL7s.Text = "Only show last 100 Biztalk HL7\'s";
            this.chkShowOnlyLastBiztalkHL7s.UseVisualStyleBackColor = true;
            // 
            // SearchWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(536, 591);
            this.Controls.Add(this.chkShowOnlyLastBiztalkHL7s);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboBiztalkSendPorts);
            this.Controls.Add(this.listViewDals);
            this.Controls.Add(this.tabControlPatient);
            this.Controls.Add(this.buttonSearch);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeftAutoHide;
            this.TabText = "Search";
            this.Text = "Search by PatientId/VisitNumber";
            this.Load += new System.EventHandler(this.SearchWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.tabControlPatient.ResumeLayout(false);
            this.tabPagePatientId.ResumeLayout(false);
            this.tabPagePatientId.PerformLayout();
            this.tabPagePatientName.ResumeLayout(false);
            this.tabPagePatientName.PerformLayout();
            this.tabPageVisitNumber.ResumeLayout(false);
            this.tabPageVisitNumber.PerformLayout();
            this.tabPageDoctorNumber.ResumeLayout(false);
            this.tabPageDoctorNumber.PerformLayout();
            this.tabPageBedOccupation.ResumeLayout(false);
            this.tabPageBedOccupation.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        #region Properties

        public string PatientId
        {
            get
            {
                return this.textBoxPatientId.Text;
            }
            set
            {
                this.tabControlPatient.SelectedIndex = 0;
                this.textBoxPatientId.Text = value;
            }
        }

        public string[] VisitNumbers
        {
            get
            {
                return this.textBoxVisitNumbers.Text.Split(',');
            }
            set
            {
                this.textBoxVisitNumbers.Text = string.Join(", ", value);
                this.tabControlPatient.SelectedIndex = 2;
            }
        }

        public string[] DoctorNumbers
        {
            get
            {
                return this.textBoxDoctorNumbers.Text.Split(',');
            }
            set
            {
                this.textBoxDoctorNumbers.Text = string.Join(", ", value);
                this.tabControlPatient.SelectedIndex = 3;
            }
        }

        public string[] WardIds
        {
            get
            {
                return this.textBoxWardIds.Text.Split(',');
            }
            set
            {
                this.textBoxWardIds.Text = string.Join(", ", value);
                this.tabControlPatient.SelectedIndex = 4;
            }
        }

        #endregion

        #region Methods

        public void DoSearch()
        {
            buttonSearch_Click(null, null);
        }

        #endregion

        #region Events

        void SearchWindow_Load(object sender, System.EventArgs e)
        {
            this.tabControlPatient.SelectedIndex = 2;

//            Task.Run(async () =>
//            {
//                var tasks = new Task<List<string>>[] 
//                { 
//                    Task.Run(async () =>
//                        {
//                            using (var db = new Database("Biztalk"))
//                            {
//                                return await db.FetchAsync<string>(@"SELECT nvcName
//        FROM BizTalkMgmtDb..bts_sendport with (nolock)");
//                            }
//                        }),
//                    Task.Run(async () =>
//                         {
//                             using (var db = new Database("Biztalk Test"))
//                             {
//                                 return await db.FetchAsync<string>(@"SELECT nvcName
//        FROM BizTalkMgmtDb..bts_sendport with (nolock)");
//                             }
//                         })
//                };

//                await Task.WhenAll(tasks).ConfigureAwait(false);
//                this.UIThread(() =>
//                {
//                    cboBiztalkSendPorts.BeginUpdate();
//                    cboBiztalkSendPorts.Items.AddRange(tasks[0].Result.ToArray());
//                    cboBiztalkSendPorts.Items.AddRange(tasks[1].Result.ToArray());
//                    cboBiztalkSendPorts.EndUpdate();
//                    cboBiztalkSendPorts.SelectedIndex = 0;

//                    this.listViewDals.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewDals_ItemChecked);
//                });
//            }).ConfigureAwait(false);

            this.listViewDals.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewDals_ItemChecked);
        }

        void buttonSearch_Click(object sender, System.EventArgs e)
        {
            errorProvider.SetError(this.textBoxPatientId, string.Empty);
            errorProvider.SetError(this.textBoxGivenName, string.Empty);
            errorProvider.SetError(this.textBoxFamilyName, string.Empty);
            errorProvider.SetError(this.textBoxVisitNumbers, string.Empty);
            errorProvider.SetError(this.textBoxDoctorNumbers, string.Empty);
            errorProvider.SetError(this.textBoxWardIds, string.Empty);

            var searchDals = (from ListViewItem lvi in listViewDals.CheckedItems
                              select lvi.Text).ToList();

            //input validation
            switch (tabControlPatient.SelectedIndex)
            {
                case 0: //PatientId
                    OnSearchByPatientId(new SearchByPatientIdEventArgs(
                        this.textBoxPatientId.Text.Trim(),
                        patientShowState,
                        searchDals,
                        cboBiztalkSendPorts.SelectedItem as string,
                        chkShowOnlyLastBiztalkHL7s.Checked));
                    break;
                case 1: //PatientName		
                    if (this.textBoxGivenName.Text.Trim().Length == 0 &&
                        this.textBoxFamilyName.Text.Trim().Length == 0)
                    {
                        errorProvider.SetError(this.textBoxGivenName, "You need to enter a GivenName and/or a FamilyName!");
                        errorProvider.SetError(this.textBoxFamilyName, "You need to enter a GivenName and/or a FamilyName!");
                        return;
                    }
                    OnSearchByPatientName(new SearchByPatientNameEventArgs(
                        this.textBoxGivenName.Text.Trim(),
                        this.textBoxFamilyName.Text.Trim(),
                        searchDals,
                        cboBiztalkSendPorts.SelectedItem as string,
                        chkShowOnlyLastBiztalkHL7s.Checked));
                    break;
                case 2: //VisitNumber
                    OnSearchByVisitNumbers(new SearchByVisitNumbersEventArgs(
                        (from visitNumber
                               in this.textBoxVisitNumbers.Text.Split(',')
                         select visitNumber.Trim()).ToArray(),
                        visitShowState,
                        searchDals,
                        cboBiztalkSendPorts.SelectedItem as string,
                        chkShowOnlyLastBiztalkHL7s.Checked));
                    break;
                case 3: //DoctorNumber
                    OnSearchByDoctorNumbers(new SearchByDoctorNumbersEventArgs(
                        (from doctorNumber
                               in this.textBoxDoctorNumbers.Text.Split(',')
                         select doctorNumber.Trim()).ToArray(),
                        searchDals,
                        cboBiztalkSendPorts.SelectedItem as string,
                        chkShowOnlyLastBiztalkHL7s.Checked));
                    break;
                case 4: //WardIds
                    OnSearchByWardIds(new SearchByWardIdsEventArgs(
                        (from wardId
                               in this.textBoxWardIds.Text.Split(',')
                         select wardId.Trim()).ToArray(),
                        searchDals,
                        cboBiztalkSendPorts.SelectedItem as string,
                        chkShowOnlyLastBiztalkHL7s.Checked));
                    break;
            }
        }

        void InputValidating(object sender, System.EventArgs e)
        {

        }

        void radioButtonPatientId_CheckedChanged(object sender, System.EventArgs e)
        {
            if (sender.Equals(this.radioButtonPatientIdPatient))
                patientShowState = ShowState.Patients;
            else if (sender.Equals(this.radioButtonPatientIdVisits))
                patientShowState = ShowState.Visists;
        }

        void radioButtonVisitNumber_CheckedChanged(object sender, System.EventArgs e)
        {
            if (sender.Equals(this.radioButtonVisitNumberPatients))
                visitShowState = ShowState.Patients;
            else if (sender.Equals(this.radioButtonVisitNumberVisits))
                visitShowState = ShowState.Visists;
            else if (sender.Equals(this.radioButtonVisitNumberVisitDetails))
                visitShowState = ShowState.VisitDetails;
        }

        void radioButton_CheckedChanged(object sender, System.EventArgs e)
        {

        }

        void textBoxPatientId_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Enter pressed
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                buttonSearch_Click(null, null);
            }
        }

        void textBoxVisitNumbers_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            // Enter pressed
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                buttonSearch_Click(null, null);
                e.SuppressKeyPress = true;
            }
        }

        void textBoxWardIds_KeyDown(object sender, KeyEventArgs e)
        {
            // Enter pressed
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
            {
                buttonSearch_Click(null, null);
                e.SuppressKeyPress = true;
            }
        }

        void tabControlPatient_SelectedIndexChanged(object sender, System.EventArgs e)
        {
        }

        void textBoxDoctorNumbers_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && e.Modifiers == Keys.None)
                buttonSearch_Click(null, null);
        }

        void unSelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var check = (listViewDals.CheckedItems.Count > 0);

            foreach (ListViewItem lvi in listViewDals.Items)
                lvi.Checked = !check;
        }

        void listViewDals_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            OnDalChecked(e);
        }

        #endregion

        #region Delegates

        public delegate void SearchByPatientNameEventHandler(object sender, SearchByPatientNameEventArgs e);
        public delegate void SearchByPatientIdEventHandler(object sender, SearchByPatientIdEventArgs e);
        public delegate void SearchByVisitNumbersEventHandler(object sender, SearchByVisitNumbersEventArgs e);
        public delegate void SearchByDoctorNumbersEventHandler(object sender, SearchByDoctorNumbersEventArgs e);
        public delegate void SearchByWardIdsEventHandler(object sender, SearchByWardIdsEventArgs e);

        #endregion

        #region Event Declaration

        public event SearchByPatientNameEventHandler SearchByPatientName;
        public event SearchByPatientIdEventHandler SearchByPatientId;
        public event SearchByVisitNumbersEventHandler SearchByVisitNumbers;
        public event SearchByDoctorNumbersEventHandler SearchByDoctorNumbers;
        public event SearchByWardIdsEventHandler SearchByWardIds;
        public event ItemCheckedEventHandler DalChecked;

        protected virtual void OnDalChecked(ItemCheckedEventArgs e)
        {
            if (DalChecked != null)
                DalChecked(this, e);
        }

        protected virtual void OnSearchByPatientName(SearchByPatientNameEventArgs e)
        {
            if (SearchByPatientName != null)
                SearchByPatientName(this, e);
        }

        protected virtual void OnSearchByPatientId(SearchByPatientIdEventArgs e)
        {
            if (SearchByPatientId != null)
                SearchByPatientId(this, e);
        }

        protected virtual void OnSearchByVisitNumbers(SearchByVisitNumbersEventArgs e)
        {
            if (SearchByVisitNumbers != null)
                SearchByVisitNumbers(this, e);
        }

        protected virtual void OnSearchByDoctorNumbers(SearchByDoctorNumbersEventArgs e)
        {
            if (SearchByDoctorNumbers != null)
                SearchByDoctorNumbers(this, e);
        }

        protected virtual void OnSearchByWardIds(SearchByWardIdsEventArgs e)
        {
            if (SearchByWardIds != null)
                SearchByWardIds(this, e);
        }

        #endregion
    }

    #region Enums

    public enum ShowState
    {
        Patients,
        Visists,
        VisitDetails
    }

    #endregion

    #region EventArgs

    public abstract class SearchBaseEventArgs : EventArgs
    {
        public SearchBaseEventArgs(IList<string> searchDals, string biztalk, bool biztalkShowOnlyLast100HL7s)
        {
            SearchDals = searchDals;
            Biztalk = biztalk;
            BiztalkShowOnlyLast100HL7s = biztalkShowOnlyLast100HL7s;
        }

        public IList<string> SearchDals { get; set; }
        public string Biztalk { get; set; }
        public bool BiztalkShowOnlyLast100HL7s { get; set; }
    }

    public class SearchByPatientNameEventArgs : SearchBaseEventArgs
    {
        public SearchByPatientNameEventArgs(string patientGivenName, string patientFamilyName, IList<string> searchDals, string biztalk, bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
        {
            PatientGivenName = patientGivenName;
            PatientFamilyName = patientFamilyName;
        }

        public string PatientGivenName { get; set; }
        public string PatientFamilyName { get; set; }
    }

    public class SearchByPatientIdEventArgs : SearchBaseEventArgs
    {
        public SearchByPatientIdEventArgs(string patientId, ShowState showState, IList<string> searchDals, string biztalk, bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
        {
            PatientId = patientId;
            ShowState = showState;
        }

        public string PatientId { get; set; }
        public ShowState ShowState { get; set; }
    }

    public class SearchByVisitNumbersEventArgs : SearchBaseEventArgs
    {
        public SearchByVisitNumbersEventArgs(string[] visitNumbers, ShowState showState, IList<string> searchDals, string biztalk, bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
        {
            VisitNumbers = visitNumbers;
            ShowState = showState;
        }

        public string[] VisitNumbers { get; set; }
        public ShowState ShowState { get; set; }
    }

    public class SearchByDoctorNumbersEventArgs : SearchBaseEventArgs
    {
        public SearchByDoctorNumbersEventArgs(string[] doctorNumbers, IList<string> searchDals, string biztalk, bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
        {
            DoctorNumbers = doctorNumbers;
        }

        public string[] DoctorNumbers { get; set; }
    }

    public class SearchByWardIdsEventArgs : SearchBaseEventArgs
    {
        public SearchByWardIdsEventArgs(string[] wardIds, IList<string> searchDals, string biztalk, bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
        {
            WardIds = wardIds;
        }

        public string[] WardIds { get; set; }
    }

    #endregion
}

