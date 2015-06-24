using BrightIdeasSoftware;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Linq;
using System.Dynamic;
using System.Linq.Expressions;

namespace HHR.HL7.Search
{
	public class ListWindow : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

        private ObjectListView listView;
        private System.ComponentModel.IContainer components = null;
        private ContextMenuStrip contextMenuStrip1;

		#endregion

		#region Constructor

		public ListWindow()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            ToolStripItem item = null;

            item = new ToolStripSeparator();
            this.listView.ContextMenuStrip.Items.Add(item);

            //item = new ToolStripMenuItem("Search on PatientId", null, menuItemSearchPatientId_Click);
            //item.Name = "menuItemSearchPatientId";
            //this.listView.ContextMenuStrip.Items.Add(item);

            //item = new ToolStripMenuItem("Search on VisitNumber", null, menuItemSearchVisitNumber_Click);
            //item.Name = "menuItemSearchVisitNumber";
            //this.listView.ContextMenuStrip.Items.Add(item);

            //item = new ToolStripMenuItem("Search on DoctorNumber", null, menuItemSearchDoctorNumber_Click);
            //item.Name = "menuItemSearchDoctorNumber";
            //this.listView.ContextMenuStrip.Items.Add(item);

            //item = new ToolStripMenuItem("Search InsuranceCompany", null, menuItemSearchInsuranceCompany_Click);
            //item.Name = "menuItemSearchInsuranceCompany";
            //this.listView.ContextMenuStrip.Items.Add(item);

            item = new ToolStripMenuItem("&Eport to HL7...", null, menuItemExport_Click);
            item.Name = "menuItemExport";
            this.listView.ContextMenuStrip.Items.Add(item);

            item = new ToolStripMenuItem("Copy as File", null, menuItemCopyFile_Click);
            item.Name = "menuItemCopyFile";
            this.listView.ContextMenuStrip.Items.Add(item);

            item = new ToolStripMenuItem("Copy as Tab Seperated (Excel)", null, menuItemCopyCommaSeperated_Click);
            item.Name = "menuItemCopyCommaSeperated";
            this.listView.ContextMenuStrip.Items.Add(item);

            this.listView.ContextMenuStrip.Opening += new CancelEventHandler(ContextMenuStrip_Opening);

            this.listView.CellToolTipGetter = delegate(OLVColumn col, Object x)
            {
                var value = (x as IDictionary<string, object>)[col.Name];
                return value != null ? value.ToString() : null;
            };
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListWindow));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.listView = new ObjectListView();
            ((System.ComponentModel.ISupportInitialize)(this.listView)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(146, 26);
            // 
            // listView
            // 
            this.listView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView.FullRowSelect = true;
            this.listView.HideSelection = false;
            this.listView.Location = new System.Drawing.Point(0, 0);
            this.listView.Name = "listView";
            this.listView.OwnerDraw = true;
            this.listView.ShowGroups = false;
            this.listView.Size = new System.Drawing.Size(296, 293);
            this.listView.TabIndex = 0;
            this.listView.UseCompatibleStateImageBehavior = false;
            this.listView.View = System.Windows.Forms.View.Details;
            this.listView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listView_KeyDown);
            this.listView.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.listView_ItemDrag);
            this.listView.SelectedIndexChanged += new System.EventHandler(this.listView_SelectedIndexChanged);
            this.listView.ContextMenuStrip = this.contextMenuStrip1;
            // 
            // ListWindow
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(296, 293);
            this.Controls.Add(this.listView);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop)
                        | WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ListWindow";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.Load += new System.EventHandler(this.ListWindow_Load);
            //((System.Configuration.IPersistComponentSettings)(this.listView)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.listView)).EndInit();
            this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		//		public override Cursor Cursor 
		//		{
		//			get{ return base.Cursor; }
		//			set
		//			{ 
		//				base.Cursor = value;
		//				this.listView.Cursor = value; 
		//			}
		//		}

		#endregion

		#region Methods

		#region Export Methods 

		private void ExportHL7(string path)
		{
            string fileText = null;
			string fullPath = null;
            StreamWriter sr = null;

            var query = (from dynamic item in listView.SelectedObjects
                         select item).OrderBy(x => x.MessageControlId);

			//Sort the messages by filename
			//hl7Set.Sort(new HL7FileNameComparer());

            foreach (var hl7 in query)
			{
				fullPath = path;
				if (!fullPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
					fullPath += Path.DirectorySeparatorChar;
				fullPath += hl7.FileName;

				if (File.Exists(fullPath)) 
					File.Delete(fullPath);

                using (sr = new StreamWriter(File.Create(fullPath), Encoding.GetEncoding(1252))) //Latin1_General_CI_AS = 1252 (default codepage SQL2000)
                {
                    fileText = hl7.Message; //.Replace("\r\n", "\r").Replace("\r\r", "\r").Replace("\r", "\r\n");
                    //byte[] buffer = Encoding.GetEncoding(1252).GetBytes(fileText); 
                    sr.Write(fileText);//Encoding.GetEncoding(1252).GetString(buffer));
                    sr.Flush();
                }
			}
		}

		#endregion 

		#region Display Methods

        public void DisplayResults(IList<dynamic> results) 
        {
			this.Cursor = Cursors.WaitCursor;

            listView.BeginUpdate();
            listView.Columns.Clear();
            if (results != null && results.Count > 0)
            {
                var first = results.First();
                var tFirst = first as IDynamicMetaObjectProvider;
                var memberNames = tFirst.GetMetaObject(Expression.Constant(tFirst)).GetDynamicMemberNames();
                for (int i = 0; i < memberNames.Count(); i++)
                {
                    var memberName = memberNames.ElementAt(i);
                    var column = new OLVColumn()
                        {
                            Name = memberName,
                            Text = memberName,
                            ToolTipText = memberName,
                            AspectGetter = x => 
                                {
                                    var dict = x as IDictionary<string, object>;
                                    return dict != null && dict.ContainsKey(memberName) ? dict[memberName] : null;
                                }          
                        };                 
                    listView.Columns.Add(column);                  
                }
            }

            listView.SetObjects(results);

            foreach (OLVColumn col in listView.Columns)
            {
                int colWidthBeforeAutoResize = col.Width;
                col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
                int colWidthAfterAutoResizeByHeader = col.Width;
                col.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                int colWidthAfterAutoResizeByContent = col.Width;

                if (colWidthAfterAutoResizeByHeader > colWidthAfterAutoResizeByContent)
                    col.AutoResize(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
            listView.EndUpdate();

            this.Cursor = Cursors.Default;

            OnList(new ListEventArgs(null));
		}

		private void DisplayPatientContextMenuItems()
		{
			if (this.listView.SelectedItems.Count > 0)
			{
				this.listView.ContextMenuStrip.Items["menuItemSearchPatientId"].Visible = true;
				this.listView.ContextMenuStrip.Items["menuItemSearchDoctorNumber"].Visible = true;			
			}

            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayVisitContextMenuItems()
		{
			if (this.listView.SelectedItems.Count > 0)
			{
				this.listView.ContextMenuStrip.Items["menuItemSearchPatientId"].Visible = true;		
				this.listView.ContextMenuStrip.Items["menuItemSearchVisitNumber"].Visible = true;		
			}

            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayInsuranceContextMenuItems()
		{
			if (this.listView.SelectedItems.Count > 0)
			{
				this.listView.ContextMenuStrip.Items["menuItemSearchInsuranceCompany"].Visible = true;			
			}

            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayVisitInsuranceContextMenuItems()
		{
           this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayVisitDetailContextMenuItems()
		{
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayAdmissionContextMenuItems()
		{
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayStayContextMenuItems()
		{
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayDoctorAttendationContextMenuItems()
		{
			if (this.listView.SelectedItems.Count > 0)
			{
				this.listView.ContextMenuStrip.Items["menuItemSearchDoctorNumber"].Visible = true;			
			}

            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayLeaveContextMenuItems()
		{
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayDoctorContextMenuItems()
		{
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayInsuranceCompanyContextMenuItems()
		{
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayADTContextMenuItems()
		{
			this.listView.ContextMenuStrip.Items["menuItemExport"].Visible = true;
            this.listView.ContextMenuStrip.Items["menuItemCopyFile"].Visible = true;
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void DisplayMFNContextMenuItems()
		{
			this.listView.ContextMenuStrip.Items["menuItemExport"].Visible = true;
            this.listView.ContextMenuStrip.Items["menuItemCopyFile"].Visible = true;
            this.listView.ContextMenuStrip.Items["menuItemCopyCommaSeperated"].Visible = true;
		}

		private void SelectAll()
		{
            this.Cursor = Cursors.WaitCursor;
            this.listView.BeginUpdate();

			this.listView.SelectedIndexChanged -= new EventHandler(listView_SelectedIndexChanged);

			// Select every item
			foreach (ListViewItem item in listView.Items)
			{
				item.Selected = true;
			}

            this.listView.EndUpdate();
            this.Cursor = Cursors.Default;

			this.listView.SelectedIndexChanged += new EventHandler(listView_SelectedIndexChanged);
		}

		#endregion

        #region Drag & Drop Methods

        private string[] GetSelection()
        {
            if (listView.SelectedItems.Count == 0)
                return null;

            string file = null;
            string fileText = null;
            StreamWriter sr = null;
            string[] files = new string[listView.SelectedItems.Count];
            int i = 0;
            foreach (dynamic item in listView.SelectedObjects)
            {
                file = Path.GetTempPath() + Path.DirectorySeparatorChar + item.FileName;
                files[i++] = file;

                using (sr = new StreamWriter(File.Create(file), Encoding.GetEncoding(1252))) //Latin1_General_CI_AS = 1252 (default codepage SQL2000)
                {
                    fileText = (item.Message as string); //.Replace("\r\n", "\r").Replace("\r\r", "\r").Replace("\r", "\r\n"); 
                    sr.Write(fileText);
                    sr.Flush();
                }
            }
            return files;
        }

        #endregion

        #endregion

        #region DockContent Overriden Methods

        protected override string GetPersistString()
		{
			return GetType().ToString() + "," + this.Text;
		}

		#endregion

		#region Events

		private void ListWindow_Load(object sender, System.EventArgs e)
		{
		}

		private void listView_SelectedIndexChanged(object sender, System.EventArgs e)
		{
            if (this.listView.SelectedIndex >= 0)
            {
                ListEventArgs eventArgs = new ListEventArgs((this.listView.SelectedItem as BrightIdeasSoftware.OLVListItem).RowObject);
                OnList(eventArgs);
            }
		}

        private void ContextMenuStrip_Opening(object sender, System.EventArgs e)
		{		
			// Hide all the menu items so that the menu will not appear
            bool afterSplitter = false;
            foreach (ToolStripItem item in this.listView.ContextMenuStrip.Items)
            {
                if (afterSplitter)
                    item.Visible = false;
                if (item is ToolStripSeparator)
                    afterSplitter = true;
            }

			if (this.listView.SelectedItems.Count == 0)
			{
				return;
			}
			else
			{
			}
		}

		private void menuItemExport_Click(object sender, System.EventArgs e)
		{
			FolderBrowserDialog folderBrowserDialog = null;
			DialogResult result;
			string folderName = null;

			folderBrowserDialog	= new FolderBrowserDialog();
			folderBrowserDialog.Description = "Select the directory that you want to export the HL7 files to.";
			folderBrowserDialog.ShowNewFolderButton = false;
			folderBrowserDialog.SelectedPath = ConfigurationManager.AppSettings["exportPath"];

			// Show the FolderBrowserDialog.
			result = folderBrowserDialog.ShowDialog();
			if( result == DialogResult.OK )
			{
				folderName = folderBrowserDialog.SelectedPath;
				//ConfigurationSettings.AppSettings["lastExportPath"] = folderName; //THROWS ERROR
				ExportHL7(folderName);
				MessageBox.Show("Exported " + this.listView.SelectedItems.Count + " HL7 " + 
					((this.listView.SelectedItems.Count > 1) ? "messages" : "message") + " to '" +
					folderName + "'", "Export done!", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}

		private void listView_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			// Control A pressed, for 'Select All'?
			if (e.KeyCode == Keys.A && e.Modifiers == Keys.Control)
			{
                SelectAll();

				e.Handled = true; // don't pass the event down
			}
		}

        private void listView_ItemDrag(object sender, System.Windows.Forms.ItemDragEventArgs e)
		{
			string[] files = GetSelection();
            if (files != null)
            {
                DoDragDrop(new DataObject(DataFormats.FileDrop, files), DragDropEffects.Move);
            }
		}

        private void menuItemCopyCommaSeperated_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (OLVListItem item in listView.SelectedItems)
            {
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems)
                {
                    sb.Append(subItem.Text);
                    sb.Append("\t");
                }
                sb.Remove(sb.Length - 1, 1); //remove last tab
                sb.Append(Environment.NewLine);
            }
            sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length); //remove last newline

            IDataObject data = new DataObject(DataFormats.CommaSeparatedValue);
            data.SetData(sb.ToString());
            Clipboard.SetDataObject(data);
        }

        private void menuItemCopyFile_Click(object sender, EventArgs e)
        {
            string[] files = GetSelection();
            if (files != null)
            {
                IDataObject data = new DataObject(DataFormats.FileDrop, files);
                MemoryStream memo = new MemoryStream(4);
                byte[] bytes = new byte[] { 2, 0, 0, 0 };
                memo.Write(bytes, 0, bytes.Length);
                data.SetData("Preferred DropEffect", memo);
                Clipboard.SetDataObject(data);
            }
        }

		#endregion

		#region Delegates

        private delegate void SetCursorDelegate(Cursor cursor);
        private delegate void SetEnabledDelegate(bool enabled);
		private delegate void AddColumnHeadersToListDelegate(ColumnHeader[] values);
		private delegate void AddItemToListDelegate(ListViewItem lvi);
		public delegate void ListEventHandler(object sender, ListEventArgs e);
		public delegate void SearchPatientIdEventHandler(object sender, PatientIdEventArgs e);
		public delegate void SearchVisitNumberEventHandler(object sender, VisitNumberEventArgs e);
		public delegate void SearchDoctorNumberEventHandler(object sender, DoctorNumberEventArgs e);
		//public delegate void SearchInsuranceCompanyEventHandler(object sender, InsuranceCompanyEventArgs e);
		public delegate void ImportPatientEventHandler(object sender, PatientIdEventArgs e);
		public delegate void ImportVisitEventHandler(object sender, VisitNumberEventArgs e);

		#endregion

		#region Event Declaration

		public event ListEventHandler List;

		protected virtual void OnList(ListEventArgs e)
		{
			if (List != null) 
				List(this, e);
		}

		public event SearchPatientIdEventHandler SearchPatientId;

		protected virtual void OnSearchPatientId(PatientIdEventArgs e)
		{
			if (SearchPatientId != null) 
				SearchPatientId(this, e);
		}

		public event SearchVisitNumberEventHandler SearchVisitNumber;

		protected virtual void OnSearchVisitNumber(VisitNumberEventArgs e)
		{
			if (SearchVisitNumber != null) 
				SearchVisitNumber(this, e);
		}

		public event SearchDoctorNumberEventHandler SearchDoctorNumber;

		protected virtual void OnSearchDoctorNumber(DoctorNumberEventArgs e)
		{
			if (SearchDoctorNumber != null)  
				SearchDoctorNumber(this, e);
		}
		public event ImportPatientEventHandler ImportPatient;

		protected virtual void OnImportPatient(PatientIdEventArgs e)
		{
			if (ImportPatient != null) 
				ImportPatient(this, e);
		}

		public event ImportVisitEventHandler ImportVisit;

		protected virtual void OnImportVisit(VisitNumberEventArgs e)
		{
			if (ImportVisit != null) 
				ImportVisit(this, e);
		}

		#endregion
    }

	#region EventArgs

	public class ListEventArgs : EventArgs 
	{  
		public ListEventArgs(dynamic entity) 
		{
			Entity = entity;
		}

        public dynamic Entity { get; private set; }
	}

	public class DoctorNumberEventArgs : EventArgs 
	{  
		public DoctorNumberEventArgs(string[] doctorNumbers) 
		{
			DoctorNumbers = doctorNumbers;
		}

        public string[] DoctorNumbers { get; private set; }
	}
    
	#endregion
}

