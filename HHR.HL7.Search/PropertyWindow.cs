using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
	public class PropertyWindow : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.ComponentModel.IContainer components = null;
		
		#endregion

		#region Constructor

		public PropertyWindow()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(PropertyWindow));
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.SuspendLayout();
			// 
			// propertyGrid
			// 
			this.propertyGrid.CommandsVisibleIfAvailable = true;
			this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid.HelpVisible = false;
			this.propertyGrid.LargeButtons = false;
			this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(392, 253);
			this.propertyGrid.TabIndex = 0;
			this.propertyGrid.Text = "PropertyGrid";
			this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// PropertyWindow
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(392, 253);
			this.Controls.Add(this.propertyGrid);
			this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
				| WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight) 
				| WeifenLuo.WinFormsUI.Docking.DockAreas.DockTop) 
				| WeifenLuo.WinFormsUI.Docking.DockAreas.DockBottom)));
			this.HideOnClose = true;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "PropertyWindow";
			this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockRightAutoHide;
			this.ResumeLayout(false);

		}
		#endregion

		#region Properties

		public object Entity
		{
			get{ return this.propertyGrid.SelectedObject; }
			set{ this.propertyGrid.SelectedObject = value; }
		}

		#endregion

		#region DockContent Overriden Methods

		protected override string GetPersistString()
		{
			return GetType().ToString() + "," + this.Text;
		}

		#endregion
	}
}

