using AsyncPoco;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
	public class SearchHL7MFNWindow : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

        private System.Windows.Forms.CheckBox checkBoxMessageTimeStamp;
		private System.Windows.Forms.ComboBox comboBoxMessageTimeStamp;
		private System.Windows.Forms.Button buttonSearch;
		private System.Windows.Forms.DateTimePicker dateTimePickerMessageTimeStamp;
		private System.Windows.Forms.TextBox textBoxDoctorNumbers;
		private System.Windows.Forms.CheckBox checkBoxDoctorNumbers;
		private System.Windows.Forms.ErrorProvider errorProvider;
        private ListView listViewDals;
        private Label label1;
        private ComboBox cboBiztalkSendPorts;
        private CheckBox chkShowOnlyLastBiztalkHL7s;
		private System.ComponentModel.IContainer components = null;

		#endregion

		#region Constructor

        public SearchHL7MFNWindow(IEnumerable<string> dals)
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

			this.dateTimePickerMessageTimeStamp.Value = DateTime.Now.Date;

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
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#endregion

		#region Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("test");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchHL7MFNWindow));
            this.checkBoxMessageTimeStamp = new System.Windows.Forms.CheckBox();
            this.textBoxDoctorNumbers = new System.Windows.Forms.TextBox();
            this.checkBoxDoctorNumbers = new System.Windows.Forms.CheckBox();
            this.comboBoxMessageTimeStamp = new System.Windows.Forms.ComboBox();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.dateTimePickerMessageTimeStamp = new System.Windows.Forms.DateTimePicker();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.listViewDals = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.cboBiztalkSendPorts = new System.Windows.Forms.ComboBox();
            this.chkShowOnlyLastBiztalkHL7s = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // checkBoxMessageTimeStamp
            // 
            this.checkBoxMessageTimeStamp.Checked = true;
            this.checkBoxMessageTimeStamp.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxMessageTimeStamp.Location = new System.Drawing.Point(6, 140);
            this.checkBoxMessageTimeStamp.Name = "checkBoxMessageTimeStamp";
            this.checkBoxMessageTimeStamp.Size = new System.Drawing.Size(136, 24);
            this.checkBoxMessageTimeStamp.TabIndex = 35;
            this.checkBoxMessageTimeStamp.Text = "MessageTimeStamp:";
            this.checkBoxMessageTimeStamp.CheckedChanged += new System.EventHandler(this.checkBoxMessageTimeStamp_CheckedChanged);
            // 
            // textBoxDoctorNumbers
            // 
            this.textBoxDoctorNumbers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDoctorNumbers.Enabled = false;
            this.textBoxDoctorNumbers.Location = new System.Drawing.Point(140, 170);
            this.textBoxDoctorNumbers.Multiline = true;
            this.textBoxDoctorNumbers.Name = "textBoxDoctorNumbers";
            this.textBoxDoctorNumbers.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDoctorNumbers.Size = new System.Drawing.Size(202, 272);
            this.textBoxDoctorNumbers.TabIndex = 38;
            // 
            // checkBoxDoctorNumbers
            // 
            this.checkBoxDoctorNumbers.Location = new System.Drawing.Point(6, 170);
            this.checkBoxDoctorNumbers.Name = "checkBoxDoctorNumbers";
            this.checkBoxDoctorNumbers.Size = new System.Drawing.Size(128, 32);
            this.checkBoxDoctorNumbers.TabIndex = 39;
            this.checkBoxDoctorNumbers.Text = "DoctorNumbers (comma seperated):";
            this.checkBoxDoctorNumbers.CheckedChanged += new System.EventHandler(this.checkBoxDoctorNumbers_CheckedChanged);
            // 
            // comboBoxMessageTimeStamp
            // 
            this.comboBoxMessageTimeStamp.Items.AddRange(new object[] {
            ">",
            ">=",
            "=",
            "<=",
            "<"});
            this.comboBoxMessageTimeStamp.Location = new System.Drawing.Point(142, 140);
            this.comboBoxMessageTimeStamp.Name = "comboBoxMessageTimeStamp";
            this.comboBoxMessageTimeStamp.Size = new System.Drawing.Size(48, 21);
            this.comboBoxMessageTimeStamp.TabIndex = 37;
            this.comboBoxMessageTimeStamp.Text = ">=";
            // 
            // buttonSearch
            // 
            this.buttonSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSearch.Location = new System.Drawing.Point(267, 455);
            this.buttonSearch.Name = "buttonSearch";
            this.buttonSearch.Size = new System.Drawing.Size(75, 23);
            this.buttonSearch.TabIndex = 36;
            this.buttonSearch.Text = "Search";
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);
            // 
            // dateTimePickerMessageTimeStamp
            // 
            this.dateTimePickerMessageTimeStamp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerMessageTimeStamp.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dateTimePickerMessageTimeStamp.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerMessageTimeStamp.Location = new System.Drawing.Point(190, 140);
            this.dateTimePickerMessageTimeStamp.Name = "dateTimePickerMessageTimeStamp";
            this.dateTimePickerMessageTimeStamp.Size = new System.Drawing.Size(154, 20);
            this.dateTimePickerMessageTimeStamp.TabIndex = 34;
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
            this.listViewDals.Location = new System.Drawing.Point(4, 12);
            this.listViewDals.Name = "listViewDals";
            this.listViewDals.Size = new System.Drawing.Size(338, 65);
            this.listViewDals.TabIndex = 42;
            this.listViewDals.UseCompatibleStateImageBehavior = false;
            this.listViewDals.View = System.Windows.Forms.View.List;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 16);
            this.label1.TabIndex = 44;
            this.label1.Text = "Biztalk HL7:";
            // 
            // cboBiztalkSendPorts
            // 
            this.cboBiztalkSendPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboBiztalkSendPorts.FormattingEnabled = true;
            this.cboBiztalkSendPorts.Location = new System.Drawing.Point(140, 82);
            this.cboBiztalkSendPorts.Name = "cboBiztalkSendPorts";
            this.cboBiztalkSendPorts.Size = new System.Drawing.Size(196, 21);
            this.cboBiztalkSendPorts.TabIndex = 43;
            // 
            // chkShowOnlyLastBiztalkHL7s
            // 
            this.chkShowOnlyLastBiztalkHL7s.AutoSize = true;
            this.chkShowOnlyLastBiztalkHL7s.Location = new System.Drawing.Point(13, 106);
            this.chkShowOnlyLastBiztalkHL7s.Name = "chkShowOnlyLastBiztalkHL7s";
            this.chkShowOnlyLastBiztalkHL7s.Size = new System.Drawing.Size(179, 17);
            this.chkShowOnlyLastBiztalkHL7s.TabIndex = 45;
            this.chkShowOnlyLastBiztalkHL7s.Text = "Only show last 100 Biztalk HL7\'s";
            this.chkShowOnlyLastBiztalkHL7s.UseVisualStyleBackColor = true;
            // 
            // SearchHL7MFNWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(348, 490);
            this.Controls.Add(this.chkShowOnlyLastBiztalkHL7s);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboBiztalkSendPorts);
            this.Controls.Add(this.listViewDals);
            this.Controls.Add(this.checkBoxMessageTimeStamp);
            this.Controls.Add(this.textBoxDoctorNumbers);
            this.Controls.Add(this.checkBoxDoctorNumbers);
            this.Controls.Add(this.comboBoxMessageTimeStamp);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.dateTimePickerMessageTimeStamp);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SearchHL7MFNWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.TabText = "Search HL7 MFN";
            this.Text = "Search HL7 MFN";
            this.Load += new System.EventHandler(this.SearchHL7MFNWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Events

		private void buttonSearch_Click(object sender, System.EventArgs e)
		{
			ComparisonFilter messageTimeStampComparisonFilter = ComparisonFilter.None;
            DateTime messageTimeStamp = DateTime.Now;
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
			if (this.checkBoxDoctorNumbers.Checked)
			{
				arString = this.textBoxDoctorNumbers.Text.Split(',');
				arDecimal = new decimal[arString.Length];
				try 
				{
					for (int i = 0; i < arString.Length; i++)
						arDecimal[i] = Decimal.Parse(arString[i].Trim());
				}
				catch(FormatException)
				{
					errorProvider.SetError(this.textBoxDoctorNumbers, "The DoctorNumber(s) must be numeric!");
					return;
				}
			}

			//invoke event
			SearchHL7MFNEventArgs eventArgs = new SearchHL7MFNEventArgs(
				messageTimeStampComparisonFilter,
				messageTimeStamp,
				arDecimal,
                searchDals,
                cboBiztalkSendPorts.SelectedItem as string,
                chkShowOnlyLastBiztalkHL7s.Checked);
			OnSearch(eventArgs);
		}

		private void checkBoxMessageTimeStamp_CheckedChanged(object sender, System.EventArgs e)
		{
			this.comboBoxMessageTimeStamp.Enabled = checkBoxMessageTimeStamp.Checked;
			this.dateTimePickerMessageTimeStamp.Enabled = checkBoxMessageTimeStamp.Checked;
		}

		private void checkBoxDoctorNumbers_CheckedChanged(object sender, System.EventArgs e)
		{
			this.textBoxDoctorNumbers.Enabled = this.checkBoxDoctorNumbers.Checked;
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
					return ComparisonFilter.GreatherThenEquals;
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

		public delegate void SearchEventHandler(object sender, SearchHL7MFNEventArgs e);

		#endregion

		#region Event Declaration

		public event SearchEventHandler Search;

		protected virtual void OnSearch(SearchHL7MFNEventArgs e)
		{
			if (Search != null) 
			{
				// Invokes the delegates. 
				Search(this, e);
			}
		}

		#endregion

        private void SearchHL7MFNWindow_Load(object sender, EventArgs e)
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
	}

	#region EventArgs

    public class SearchHL7MFNEventArgs : SearchBaseEventArgs 
	{  
		public SearchHL7MFNEventArgs(ComparisonFilter messageTimeStampComparisonFilter,
			DateTime messageTimeStamp,
			decimal[] doctorNumbers,
            IList<string> searchDals,
            string biztalk,
            bool biztalkShowOnlyLast100HL7s)
            : base(searchDals, biztalk, biztalkShowOnlyLast100HL7s)
		{
			MessageTimeStampComparisonFilter = messageTimeStampComparisonFilter;
			MessageTimeStamp = messageTimeStamp;
			DoctorNumbers = doctorNumbers;
		}

        public ComparisonFilter MessageTimeStampComparisonFilter { get; private set; }
        public DateTime MessageTimeStamp { get; private set; }
        public decimal[] DoctorNumbers { get; private set; }
	}

	#endregion

    #region Enums

    public enum ComparisonFilter
    {
        None,
        LessThen,
        LessThenEquals,
        Equals,
        GreatherThenEquals,
        GreatherThen
    }

    #endregion
}

