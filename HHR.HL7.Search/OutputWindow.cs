using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
	/// <summary>
	/// Summary description for OutputWindow.
	/// </summary>
	public class OutputWindow : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

		private System.Windows.Forms.RichTextBox richTextBox;
		private System.Windows.Forms.ContextMenu contextMenu;
		private System.Windows.Forms.MenuItem menuItemClearAll;
		private TextBoxWriter writer = null;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor

		public OutputWindow()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(OutputWindow));
			this.richTextBox = new System.Windows.Forms.RichTextBox();
			this.contextMenu = new System.Windows.Forms.ContextMenu();
			this.menuItemClearAll = new System.Windows.Forms.MenuItem();
			this.SuspendLayout();
			// 
			// richTextBox
			// 
			this.richTextBox.ContextMenu = this.contextMenu;
			this.richTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.richTextBox.Location = new System.Drawing.Point(0, 0);
			this.richTextBox.Name = "richTextBox";
			this.richTextBox.Size = new System.Drawing.Size(292, 273);
			this.richTextBox.TabIndex = 0;
			this.richTextBox.Text = "";
			// 
			// contextMenu
			// 
			this.contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						this.menuItemClearAll});
			// 
			// menuItemClearAll
			// 
			this.menuItemClearAll.Index = 0;
			this.menuItemClearAll.Text = "Clear Al&l";
			this.menuItemClearAll.Click += new System.EventHandler(this.menuItemClearAll_Click);
			// 
			// OutputWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(292, 273);
			this.Controls.Add(this.richTextBox);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
				| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
				| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "OutputWindow";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockBottomAutoHide;
			this.TabText = "Output";
			this.Text = "Output";
			this.Load += new System.EventHandler(this.OutputWindow_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Menu Events

		private void menuItemClearAll_Click(object sender, System.EventArgs e)
		{
			this.richTextBox.Clear();
		}

		#endregion

		#region Form Events

		private void OutputWindow_Load(object sender, System.EventArgs e)
		{
			writer = new TextBoxWriter(richTextBox);
			Console.SetOut(writer);
		}

		#endregion
	}
}
