using AsyncPoco;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
	/// <summary>
	/// Summary description for SearchHL7Window.
	/// </summary>
	public class SearchHL7ADTWindow : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

		private System.Windows.Forms.CheckBox checkBoxMessageTimeStamp;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.ComboBox comboBoxMessageTimeStamp;
		private System.Windows.Forms.CheckBox checkBoxEventTimeStamp;
		private System.Windows.Forms.CheckBox checkBoxContainingEventType;
		private System.Windows.Forms.DateTimePicker dateTimePickerMessageTimeStamp;
		private System.Windows.Forms.ComboBox comboBoxEventTimeStamp;
		private System.Windows.Forms.DateTimePicker dateTimePickerEventTimeStamp;
		private System.Windows.Forms.CheckedListBox checkedListBoxEventTypes;
		private System.Windows.Forms.TextBox textBoxContainingText;
		private System.Windows.Forms.CheckBox checkBoxContainingText;
		private System.Windows.Forms.CheckBox checkBoxVisitNumbers;
		private System.Windows.Forms.TextBox textBoxVisitNumbers;
        private System.Windows.Forms.CheckBox checkBoxFullSequence;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private ListView listViewDals;
        private CheckBox checkBoxEventTimeStamp2;
        private ComboBox comboBoxEventTimeStamp2;
        private DateTimePicker dateTimePickerEventTimeStamp2;
        private CheckBox checkBoxMessageTimeStamp2;
        private ComboBox comboBoxMessageTimeStamp2;
        private DateTimePicker dateTimePickerMessageTimeStamp2;
        private Label label1;
        private ComboBox cboBiztalkSendPorts;
        private CheckBox chkShowOnlyLastBiztalkHL7s;
        private IContainer components;

		#endregion

		#region Constructor

        public SearchHL7ADTWindow(IEnumerable<string> dals)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			this.dateTimePickerMessageTimeStamp.Value = DateTime.Now.Date;
            this.dateTimePickerEventTimeStamp.Value = DateTime.Now.Date;
            this.dateTimePickerMessageTimeStamp2.Value = DateTime.Now.Date.AddDays(1);
            this.dateTimePickerEventTimeStamp2.Value = DateTime.Now.Date.AddDays(1);

            ListViewItem item;
            listViewDals.Items.Clear();
            foreach (string dal in dals)
            {
                item = new ListViewItem(dal);
                item.Checked = true;
                listViewDals.Items.Add(item);
            }
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("test");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchHL7ADTWindow));
            this.checkedListBoxEventTypes = new System.Windows.Forms.CheckedListBox();
            this.dateTimePickerMessageTimeStamp = new System.Windows.Forms.DateTimePicker();
            this.checkBoxMessageTimeStamp = new System.Windows.Forms.CheckBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.comboBoxMessageTimeStamp = new System.Windows.Forms.ComboBox();
            this.comboBoxEventTimeStamp = new System.Windows.Forms.ComboBox();
            this.checkBoxEventTimeStamp = new System.Windows.Forms.CheckBox();
            this.dateTimePickerEventTimeStamp = new System.Windows.Forms.DateTimePicker();
            this.checkBoxContainingEventType = new System.Windows.Forms.CheckBox();
            this.textBoxContainingText = new System.Windows.Forms.TextBox();
            this.checkBoxContainingText = new System.Windows.Forms.CheckBox();
            this.textBoxVisitNumbers = new System.Windows.Forms.TextBox();
            this.checkBoxVisitNumbers = new System.Windows.Forms.CheckBox();
            this.checkBoxFullSequence = new System.Windows.Forms.CheckBox();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.listViewDals = new System.Windows.Forms.ListView();
            this.checkBoxMessageTimeStamp2 = new System.Windows.Forms.CheckBox();
            this.comboBoxMessageTimeStamp2 = new System.Windows.Forms.ComboBox();
            this.dateTimePickerMessageTimeStamp2 = new System.Windows.Forms.DateTimePicker();
            this.checkBoxEventTimeStamp2 = new System.Windows.Forms.CheckBox();
            this.comboBoxEventTimeStamp2 = new System.Windows.Forms.ComboBox();
            this.dateTimePickerEventTimeStamp2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.cboBiztalkSendPorts = new System.Windows.Forms.ComboBox();
            this.chkShowOnlyLastBiztalkHL7s = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // checkedListBoxEventTypes
            // 
            this.checkedListBoxEventTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxEventTypes.Enabled = false;
            this.checkedListBoxEventTypes.Items.AddRange(new object[] {
            "A01 - admit / visit notification",
            "A02 - transfer a patient",
            "A03 - discharge/end visit",
            "A04 - register a patient",
            "A05 - pre-admit a patient",
            "A06 - change an outpatient to an inpatient",
            "A07 - change an inpatient to an outpatient",
            "A08 - update patient information",
            "A09 - patient departing - tracking",
            "A10 - patient arriving - tracking",
            "A11 - cancel admit / visit notification",
            "A12 - cancel transfer",
            "A13 - cancel discharge / end visit",
            "A14 - pending admit",
            "A15 - pending transfer",
            "A16 - pending discharge",
            "A17 - swap patients",
            "A18 - merge patient information",
            "A19 - patient query",
            "A20 - bed status update",
            "A21 - patient goes on a \"leave of absence\"",
            "A22 - patient returns from a \"leave of absence\"",
            "A23 - delete a patient record",
            "A24 - link patient information",
            "A25 - cancel pending discharge",
            "A26 - cancel pending transfer",
            "A27 - cancel pending admit",
            "A28 - add person information",
            "A29 - delete person information",
            "A30 - merge person information",
            "A31 - update person information",
            "A32 - cancel patient arriving - tracking",
            "A33 - cancel patient departing - tracking",
            "A34 - merge patient information - patient ID only",
            "A35 - merge patient information - account number only",
            "A36 - merge patient information - patient ID & account number",
            "A37 - unlink patient information",
            "A38 - cancel pre-admit",
            "A39 - merge person - external ID",
            "A40 - merge patient - internal ID",
            "A41 - merge account - patient account number",
            "A42 - merge visit - visit number",
            "A43 - move patient information - internal ID",
            "A44 - move account information - patient account number",
            "A45 - move visit information - visit number",
            "A46 - change external ID",
            "A47 - change internal ID",
            "A48 - change alternate patient ID",
            "A49 - change patient account number",
            "A50 - change visit number",
            "A51 - change alternate visit ID"});
            this.checkedListBoxEventTypes.Location = new System.Drawing.Point(144, 390);
            this.checkedListBoxEventTypes.Name = "checkedListBoxEventTypes";
            this.checkedListBoxEventTypes.Size = new System.Drawing.Size(180, 379);
            this.checkedListBoxEventTypes.TabIndex = 0;
            // 
            // dateTimePickerMessageTimeStamp
            // 
            this.dateTimePickerMessageTimeStamp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerMessageTimeStamp.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePickerMessageTimeStamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMessageTimeStamp.Location = new System.Drawing.Point(192, 170);
            this.dateTimePickerMessageTimeStamp.Name = "dateTimePickerMessageTimeStamp";
            this.dateTimePickerMessageTimeStamp.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerMessageTimeStamp.TabIndex = 1;
            // 
            // checkBoxMessageTimeStamp
            // 
            this.checkBoxMessageTimeStamp.Checked = true;
            this.checkBoxMessageTimeStamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMessageTimeStamp.Location = new System.Drawing.Point(8, 170);
            this.checkBoxMessageTimeStamp.Name = "checkBoxMessageTimeStamp";
            this.checkBoxMessageTimeStamp.Size = new System.Drawing.Size(136, 24);
            this.checkBoxMessageTimeStamp.TabIndex = 3;
            this.checkBoxMessageTimeStamp.Text = "MessageTimeStamp:";
            this.checkBoxMessageTimeStamp.CheckedChanged += new System.EventHandler(this.checkBoxMessageTimeStamp_CheckedChanged);
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSearch.Location = new System.Drawing.Point(252, 791);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 6;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // comboBoxMessageTimeStamp
            // 
            this.comboBoxMessageTimeStamp.Items.AddRange(new object[] {
            ">",
            ">=",
            "=",
            "<=",
            "<"});
            this.comboBoxMessageTimeStamp.Location = new System.Drawing.Point(144, 170);
            this.comboBoxMessageTimeStamp.Name = "comboBoxMessageTimeStamp";
            this.comboBoxMessageTimeStamp.Size = new System.Drawing.Size(48, 21);
            this.comboBoxMessageTimeStamp.TabIndex = 7;
            this.comboBoxMessageTimeStamp.Text = ">=";
            // 
            // comboBoxEventTimeStamp
            // 
            this.comboBoxEventTimeStamp.Enabled = false;
            this.comboBoxEventTimeStamp.Items.AddRange(new object[] {
            ">",
            ">=",
            "=",
            "<=",
            "<"});
            this.comboBoxEventTimeStamp.Location = new System.Drawing.Point(144, 227);
            this.comboBoxEventTimeStamp.Name = "comboBoxEventTimeStamp";
            this.comboBoxEventTimeStamp.Size = new System.Drawing.Size(48, 21);
            this.comboBoxEventTimeStamp.TabIndex = 10;
            this.comboBoxEventTimeStamp.Text = ">=";
            // 
            // checkBoxEventTimeStamp
            // 
            this.checkBoxEventTimeStamp.Location = new System.Drawing.Point(8, 227);
            this.checkBoxEventTimeStamp.Name = "checkBoxEventTimeStamp";
            this.checkBoxEventTimeStamp.Size = new System.Drawing.Size(136, 24);
            this.checkBoxEventTimeStamp.TabIndex = 9;
            this.checkBoxEventTimeStamp.Text = "EventTimeStamp:";
            this.checkBoxEventTimeStamp.CheckedChanged += new System.EventHandler(this.checkBoxEventTimeStamp_CheckedChanged);
            // 
            // dateTimePickerEventTimeStamp
            // 
            this.dateTimePickerEventTimeStamp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerEventTimeStamp.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePickerEventTimeStamp.Enabled = false;
            this.dateTimePickerEventTimeStamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEventTimeStamp.Location = new System.Drawing.Point(192, 227);
            this.dateTimePickerEventTimeStamp.Name = "dateTimePickerEventTimeStamp";
            this.dateTimePickerEventTimeStamp.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerEventTimeStamp.TabIndex = 8;
            // 
            // checkBoxContainingEventType
            // 
            this.checkBoxContainingEventType.Location = new System.Drawing.Point(8, 390);
            this.checkBoxContainingEventType.Name = "checkBoxContainingEventType";
            this.checkBoxContainingEventType.Size = new System.Drawing.Size(144, 16);
            this.checkBoxContainingEventType.TabIndex = 11;
            this.checkBoxContainingEventType.Text = "Containing EventType:";
            this.checkBoxContainingEventType.CheckedChanged += new System.EventHandler(this.checkBoxContainingEventType_CheckedChanged);
            // 
            // textBoxContainingText
            // 
            this.textBoxContainingText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContainingText.Enabled = false;
            this.textBoxContainingText.Location = new System.Drawing.Point(144, 283);
            this.textBoxContainingText.Multiline = true;
            this.textBoxContainingText.Name = "textBoxContainingText";
            this.textBoxContainingText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxContainingText.Size = new System.Drawing.Size(180, 48);
            this.textBoxContainingText.TabIndex = 12;
            // 
            // checkBoxContainingText
            // 
            this.checkBoxContainingText.Location = new System.Drawing.Point(8, 283);
            this.checkBoxContainingText.Name = "checkBoxContainingText";
            this.checkBoxContainingText.Size = new System.Drawing.Size(128, 24);
            this.checkBoxContainingText.TabIndex = 13;
            this.checkBoxContainingText.Text = "Containing Text:";
            this.checkBoxContainingText.CheckedChanged += new System.EventHandler(this.checkBoxContainingText_CheckedChanged);
            // 
            // textBoxVisitNumbers
            // 
            this.textBoxVisitNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxVisitNumbers.Enabled = false;
            this.textBoxVisitNumbers.Location = new System.Drawing.Point(144, 339);
            this.textBoxVisitNumbers.Multiline = true;
            this.textBoxVisitNumbers.Name = "textBoxVisitNumbers";
            this.textBoxVisitNumbers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxVisitNumbers.Size = new System.Drawing.Size(180, 48);
            this.textBoxVisitNumbers.TabIndex = 14;
            // 
            // checkBoxVisitNumbers
            // 
            this.checkBoxVisitNumbers.Location = new System.Drawing.Point(8, 339);
            this.checkBoxVisitNumbers.Name = "checkBoxVisitNumbers";
            this.checkBoxVisitNumbers.Size = new System.Drawing.Size(128, 32);
            this.checkBoxVisitNumbers.TabIndex = 15;
            this.checkBoxVisitNumbers.Text = "VisitNumbers (comma seperated):";
            this.checkBoxVisitNumbers.CheckedChanged += new System.EventHandler(this.checkBoxVisitNumbers_CheckedChanged);
            // 
            // checkBoxFullSequence
            // 
            this.checkBoxFullSequence.Location = new System.Drawing.Point(8, 8);
            this.checkBoxFullSequence.Name = "checkBoxFullSequence";
            this.checkBoxFullSequence.Size = new System.Drawing.Size(304, 24);
            this.checkBoxFullSequence.TabIndex = 16;
            this.checkBoxFullSequence.Text = "Full HL7 visit sequence?";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // listViewDals
            // 
            this.listViewDals.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewDals.CheckBoxes = true;
            this.listViewDals.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            listViewItem1.StateImageIndex = 0;
            this.listViewDals.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listViewDals.Location = new System.Drawing.Point(8, 38);
            this.listViewDals.Name = "listViewDals";
            this.listViewDals.Size = new System.Drawing.Size(316, 65);
            this.listViewDals.TabIndex = 26;
            this.listViewDals.UseCompatibleStateImageBehavior = false;
            this.listViewDals.View = System.Windows.Forms.View.List;
            // 
            // checkBoxMessageTimeStamp2
            // 
            this.checkBoxMessageTimeStamp2.Checked = true;
            this.checkBoxMessageTimeStamp2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMessageTimeStamp2.Location = new System.Drawing.Point(8, 200);
            this.checkBoxMessageTimeStamp2.Name = "checkBoxMessageTimeStamp2";
            this.checkBoxMessageTimeStamp2.Size = new System.Drawing.Size(136, 24);
            this.checkBoxMessageTimeStamp2.TabIndex = 28;
            this.checkBoxMessageTimeStamp2.Text = "MessageTimeStamp:";
            this.checkBoxMessageTimeStamp2.CheckedChanged += new System.EventHandler(this.checkBoxMessageTimestamp2_CheckedChanged);
            // 
            // comboBoxMessageTimeStamp2
            // 
            this.comboBoxMessageTimeStamp2.Items.AddRange(new object[] {
            ">",
            ">=",
            "=",
            "<=",
            "<"});
            this.comboBoxMessageTimeStamp2.Location = new System.Drawing.Point(144, 200);
            this.comboBoxMessageTimeStamp2.Name = "comboBoxMessageTimeStamp2";
            this.comboBoxMessageTimeStamp2.Size = new System.Drawing.Size(48, 21);
            this.comboBoxMessageTimeStamp2.TabIndex = 29;
            this.comboBoxMessageTimeStamp2.Text = "<=";
            // 
            // dateTimePickerMessageTimeStamp2
            // 
            this.dateTimePickerMessageTimeStamp2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerMessageTimeStamp2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePickerMessageTimeStamp2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMessageTimeStamp2.Location = new System.Drawing.Point(192, 200);
            this.dateTimePickerMessageTimeStamp2.Name = "dateTimePickerMessageTimeStamp2";
            this.dateTimePickerMessageTimeStamp2.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerMessageTimeStamp2.TabIndex = 27;
            // 
            // checkBoxEventTimeStamp2
            // 
            this.checkBoxEventTimeStamp2.Location = new System.Drawing.Point(8, 257);
            this.checkBoxEventTimeStamp2.Name = "checkBoxEventTimeStamp2";
            this.checkBoxEventTimeStamp2.Size = new System.Drawing.Size(136, 24);
            this.checkBoxEventTimeStamp2.TabIndex = 31;
            this.checkBoxEventTimeStamp2.Text = "EventTimeStamp:";
            this.checkBoxEventTimeStamp2.CheckedChanged += new System.EventHandler(this.checkBoxEventTimeStamp2_CheckedChanged);
            // 
            // comboBoxEventTimeStamp2
            // 
            this.comboBoxEventTimeStamp2.Enabled = false;
            this.comboBoxEventTimeStamp2.Items.AddRange(new object[] {
            ">",
            ">=",
            "=",
            "<=",
            "<"});
            this.comboBoxEventTimeStamp2.Location = new System.Drawing.Point(144, 257);
            this.comboBoxEventTimeStamp2.Name = "comboBoxEventTimeStamp2";
            this.comboBoxEventTimeStamp2.Size = new System.Drawing.Size(48, 21);
            this.comboBoxEventTimeStamp2.TabIndex = 32;
            this.comboBoxEventTimeStamp2.Text = "<=";
            // 
            // dateTimePickerEventTimeStamp2
            // 
            this.dateTimePickerEventTimeStamp2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerEventTimeStamp2.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePickerEventTimeStamp2.Enabled = false;
            this.dateTimePickerEventTimeStamp2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerEventTimeStamp2.Location = new System.Drawing.Point(192, 257);
            this.dateTimePickerEventTimeStamp2.Name = "dateTimePickerEventTimeStamp2";
            this.dateTimePickerEventTimeStamp2.Size = new System.Drawing.Size(132, 20);
            this.dateTimePickerEventTimeStamp2.TabIndex = 30;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 16);
            this.label1.TabIndex = 34;
            this.label1.Text = "Biztalk HL7:";
            // 
            // cboBiztalkSendPorts
            // 
            this.cboBiztalkSendPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBiztalkSendPorts.FormattingEnabled = true;
            this.cboBiztalkSendPorts.Location = new System.Drawing.Point(144, 111);
            this.cboBiztalkSendPorts.Name = "cboBiztalkSendPorts";
            this.cboBiztalkSendPorts.Size = new System.Drawing.Size(180, 21);
            this.cboBiztalkSendPorts.TabIndex = 33;
            // 
            // chkShowOnlyLastBiztalkHL7s
            // 
            this.chkShowOnlyLastBiztalkHL7s.AutoSize = true;
            this.chkShowOnlyLastBiztalkHL7s.Location = new System.Drawing.Point(15, 138);
            this.chkShowOnlyLastBiztalkHL7s.Name = "chkShowOnlyLastBiztalkHL7s";
            this.chkShowOnlyLastBiztalkHL7s.Size = new System.Drawing.Size(179, 17);
            this.chkShowOnlyLastBiztalkHL7s.TabIndex = 35;
            this.chkShowOnlyLastBiztalkHL7s.Text = "Only show last 100 Biztalk HL7\'s";
            this.chkShowOnlyLastBiztalkHL7s.UseVisualStyleBackColor = true;
            // 
            // SearchHL7ADTWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(332, 828);
            this.Controls.Add(this.chkShowOnlyLastBiztalkHL7s);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboBiztalkSendPorts);
            this.Controls.Add(this.checkBoxEventTimeStamp2);
            this.Controls.Add(this.comboBoxEventTimeStamp2);
            this.Controls.Add(this.dateTimePickerEventTimeStamp2);
            this.Controls.Add(this.checkBoxMessageTimeStamp2);
            this.Controls.Add(this.comboBoxMessageTimeStamp2);
            this.Controls.Add(this.dateTimePickerMessageTimeStamp2);
            this.Controls.Add(this.listViewDals);
            this.Controls.Add(this.checkBoxMessageTimeStamp);
            this.Controls.Add(this.textBoxVisitNumbers);
            this.Controls.Add(this.textBoxContainingText);
            this.Controls.Add(this.checkBoxFullSequence);
            this.Controls.Add(this.checkBoxEventTimeStamp);
            this.Controls.Add(this.checkBoxVisitNumbers);
            this.Controls.Add(this.comboBoxEventTimeStamp);
            this.Controls.Add(this.dateTimePickerEventTimeStamp);
            this.Controls.Add(this.comboBoxMessageTimeStamp);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.checkedListBoxEventTypes);
            this.Controls.Add(this.dateTimePickerMessageTimeStamp);
            this.Controls.Add(this.checkBoxContainingText);
            this.Controls.Add(this.checkBoxContainingEventType);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchHL7ADTWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.TabText = "Search HL7 ADT";
            this.Text = "Search HL7 ADT visit sequences";
            this.Load += new System.EventHandler(this.SearchHL7ADTWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Events

		private void SearchHL7ADTWindow_Load(object sender, System.EventArgs e)
		{
            Task.Run(async () =>
            {
                var tasks = new Task<List<string>>[] 
                { 
                    Task.Run(async () =>
                        {
                            using (var db = new Database("Biztalk"))
                            {
                                return await db.FetchAsync<string>(@"SELECT nvcName
        FROM BizTalkMgmtDb..bts_sendport with (nolock)");
                            }
                        }),
                    Task.Run(async () =>
                         {
                             using (var db = new Database("Biztalk Test"))
                             {
                                 return await db.FetchAsync<string>(@"SELECT nvcName
        FROM BizTalkMgmtDb..bts_sendport with (nolock)");
                             }
                         })
                };

                await Task.WhenAll(tasks).ConfigureAwait(false);
                this.UIThread(() =>
                {
                    cboBiztalkSendPorts.BeginUpdate();
                    cboBiztalkSendPorts.Items.AddRange(tasks[0].Result.ToArray());
                    cboBiztalkSendPorts.Items.AddRange(tasks[1].Result.ToArray());
                    cboBiztalkSendPorts.EndUpdate();
                    cboBiztalkSendPorts.SelectedIndex = 0;
                });
            }).ConfigureAwait(false);
		}

		private void checkBoxMessageTimeStamp_CheckedChanged(object sender, System.EventArgs e)
		{
			this.comboBoxMessageTimeStamp.Enabled = checkBoxMessageTimeStamp.Checked;
			this.dateTimePickerMessageTimeStamp.Enabled = checkBoxMessageTimeStamp.Checked;
		}

		private void checkBoxEventTimeStamp_CheckedChanged(object sender, System.EventArgs e)
		{
			this.comboBoxEventTimeStamp.Enabled = checkBoxEventTimeStamp.Checked;
			this.dateTimePickerEventTimeStamp.Enabled = checkBoxEventTimeStamp.Checked;
		}

        private void checkBoxEventTimeStamp2_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBoxEventTimeStamp2.Enabled = checkBoxEventTimeStamp2.Checked;
            this.dateTimePickerEventTimeStamp2.Enabled = checkBoxEventTimeStamp2.Checked;
        }

        private void checkBoxMessageTimestamp2_CheckedChanged(object sender, EventArgs e)
        {
            this.comboBoxMessageTimeStamp2.Enabled = this.checkBoxMessageTimeStamp2.Checked;
            this.dateTimePickerMessageTimeStamp2.Enabled = this.checkBoxMessageTimeStamp2.Checked;
        }

		private void checkBoxContainingEventType_CheckedChanged(object sender, System.EventArgs e)
		{
			this.checkedListBoxEventTypes.Enabled = this.checkBoxContainingEventType.Checked;
		}

		private void checkBoxContainingText_CheckedChanged(object sender, System.EventArgs e)
		{
			this.textBoxContainingText.Enabled = this.checkBoxContainingText.Checked;
		}

		private void checkBoxVisitNumbers_CheckedChanged(object sender, System.EventArgs e)
		{
			this.textBoxVisitNumbers.Enabled = this.checkBoxVisitNumbers.Checked;
		}

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			ComparisonFilter messageTimeStampComparisonFilter = ComparisonFilter.None;
			DateTime messageTimeStamp = DateTime.Now;
            ComparisonFilter messageTimeStampComparisonFilter2 = ComparisonFilter.None;
            DateTime messageTimeStamp2 = DateTime.Now;
			ComparisonFilter eventTimeStampComparisonFilter = ComparisonFilter.None;
            DateTime eventTimeStamp = DateTime.Now;
            ComparisonFilter eventTimeStampComparisonFilter2 = ComparisonFilter.None;
            DateTime eventTimeStamp2 = DateTime.Now;
			string[] containingEventTypes = new string[0];
			string containingText = string.Empty;
			string[] arString = null;
			decimal[] arDecimal = null;
            IList<string> searchDals = new List<string>();

            foreach (ListViewItem item in listViewDals.CheckedItems)
                searchDals.Add(item.Text);

			if (this.checkBoxMessageTimeStamp.Checked)
			{
				messageTimeStampComparisonFilter = StringToComparisonFilter((string)this.comboBoxMessageTimeStamp.Text);
				messageTimeStamp = this.dateTimePickerMessageTimeStamp.Value;
			}
            if (this.checkBoxMessageTimeStamp2.Checked)
            {
                messageTimeStampComparisonFilter2 = StringToComparisonFilter((string)this.comboBoxMessageTimeStamp2.Text);
                messageTimeStamp2 = this.dateTimePickerMessageTimeStamp2.Value;
            }
			if (this.checkBoxEventTimeStamp.Checked)
			{
				eventTimeStampComparisonFilter = StringToComparisonFilter((string)this.comboBoxEventTimeStamp.Text);
				eventTimeStamp = this.dateTimePickerEventTimeStamp.Value;
			}
            if (this.checkBoxEventTimeStamp2.Checked)
            {
                eventTimeStampComparisonFilter2 = StringToComparisonFilter((string)this.comboBoxEventTimeStamp2.Text);
                eventTimeStamp2 = this.dateTimePickerEventTimeStamp2.Value;
            }
			if (this.checkBoxContainingEventType.Checked)
			{
				containingEventTypes = new string[this.checkedListBoxEventTypes.CheckedItems.Count];
				for (int i = 0; i < this.checkedListBoxEventTypes.CheckedItems.Count; i++)
				{
					containingEventTypes[i] = ((string)this.checkedListBoxEventTypes.CheckedItems[i]).Substring(0, 3);
				}
			}
			if (this.checkBoxContainingText.Checked)
			{
				containingText = this.textBoxContainingText.Text;
			}
			if (this.checkBoxVisitNumbers.Checked)
			{
				arString = this.textBoxVisitNumbers.Text.Split(',');
				arDecimal = new decimal[arString.Length];
				try 
				{
					for (int i = 0; i < arString.Length; i++)
						arDecimal[i] = Decimal.Parse(arString[i].Trim());
				}
				catch(FormatException)
				{
					errorProvider.SetError(this.textBoxVisitNumbers, "The VisitNumber(s) must be numeric!");
					return;
				}
			}

			//invoke event
			SearchHL7ADTEventArgs eventArgs = new SearchHL7ADTEventArgs(
				this.checkBoxFullSequence.Checked,
				messageTimeStampComparisonFilter,
				messageTimeStamp,
                messageTimeStampComparisonFilter2,
                messageTimeStamp2,
				eventTimeStampComparisonFilter,
				eventTimeStamp,
                eventTimeStampComparisonFilter2,
                eventTimeStamp2,
				containingEventTypes,
				containingText,
				arDecimal,
                searchDals,
                cboBiztalkSendPorts.SelectedItem as string,
                chkShowOnlyLastBiztalkHL7s.Checked);
			OnSearch(eventArgs);
		}

		#endregion

		#region Methods

		private ComparisonFilter StringToComparisonFilter(string comparisonFilter)
		{
			switch(comparisonFilter)
			{
				case "<":
					return ComparisonFilter.LessThen;
				case "<=":
					return ComparisonFilter.LessThenEquals;
				case "=":
					return ComparisonFilter.Equals;
				case ">=":
					return ComparisonFilter.GreatherThenEquals;
				case ">":
					return ComparisonFilter.GreatherThen;
				default:
					return ComparisonFilter.None;
			}
		}

		#endregion

		#region Delegates

		public delegate void SearchEventHandler(object sender, SearchHL7ADTEventArgs e);

		#endregion

		#region Event Declaration

		public event SearchEventHandler Search;

		protected virtual void OnSearch(SearchHL7ADTEventArgs e)
		{
			if (Search != null) 
				Search(this, e);
		}

		#endregion
	}

	#region EventArgs

	public class SearchHL7ADTEventArgs : SearchBaseEventArgs 
	{      
		public SearchHL7ADTEventArgs(bool fullSequences,
			ComparisonFilter messageTimeStampComparisonFilter,
			DateTime messageTimeStamp,
            ComparisonFilter messageTimeStampComparisonFilter2,
            DateTime messageTimeStamp2,
			ComparisonFilter eventTimeStampComparisonFilter,
			DateTime eventTimeStamp,
            ComparisonFilter eventTimeStampComparisonFilter2,
            DateTime eventTimeStamp2,
			string[] containingEventTypes,
			string containingText,
			decimal[] visitNumbers,
            IList<string> searchDals,
            string biztalk,
            bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
		{
			FullSequences = fullSequences;
			MessageTimeStampComparisonFilter = messageTimeStampComparisonFilter;
			MessageTimeStamp = messageTimeStamp;
            MessageTimeStampComparisonFilter2 = messageTimeStampComparisonFilter2;
            MessageTimeStamp2 = messageTimeStamp2;
			EventTimeStampComparisonFilter = eventTimeStampComparisonFilter;
			EventTimeStamp = eventTimeStamp;
            EventTimeStampComparisonFilter2 = eventTimeStampComparisonFilter2;
            EventTimeStamp2 = eventTimeStamp2;
			ContainingEventTypes = containingEventTypes;
			ContainingText = containingText;
			VisitNumbers = visitNumbers;
		}

		public bool FullSequences {get; protected set; }

        public ComparisonFilter MessageTimeStampComparisonFilter { get; protected set; }

        public DateTime MessageTimeStamp { get; protected set; }

        public ComparisonFilter MessageTimeStampComparisonFilter2 { get; protected set; }

        public DateTime MessageTimeStamp2 { get; protected set; }

        public ComparisonFilter EventTimeStampComparisonFilter { get; protected set; }

        public DateTime EventTimeStamp { get; protected set; }

        public ComparisonFilter EventTimeStampComparisonFilter2 { get; protected set; }

        public DateTime EventTimeStamp2 { get; protected set; }

        public string[] ContainingEventTypes { get; protected set; }

        public string ContainingText { get; protected set; }

        public decimal[] VisitNumbers { get; protected set; }
	}

	#endregion
}
