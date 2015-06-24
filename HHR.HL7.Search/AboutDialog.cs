using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Reflection;

namespace HHR.HL7.Search
{
	/// <summary>
	/// Summary description for AboutDialog.
	/// </summary>
	public class AboutDialog : System.Windows.Forms.Form
	{
		#region Fields

		private System.Windows.Forms.Label labelVersion;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Label labelCopyright;
		private System.Windows.Forms.PictureBox pictureBox1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		#endregion

		#region Constructor

		public AboutDialog()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(AboutDialog));
			this.labelVersion = new System.Windows.Forms.Label();
			this.buttonOK = new System.Windows.Forms.Button();
			this.labelCopyright = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// labelVersion
			// 
			this.labelVersion.Location = new System.Drawing.Point(35, 136);
			this.labelVersion.Name = "labelVersion";
			this.labelVersion.Size = new System.Drawing.Size(272, 16);
			this.labelVersion.TabIndex = 4;
			this.labelVersion.Text = "Version";
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonOK.Location = new System.Drawing.Point(251, 232);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.TabIndex = 3;
			this.buttonOK.Text = "OK";
			// 
			// labelCopyright
			// 
			this.labelCopyright.Location = new System.Drawing.Point(35, 168);
			this.labelCopyright.Name = "labelCopyright";
			this.labelCopyright.Size = new System.Drawing.Size(272, 32);
			this.labelCopyright.TabIndex = 5;
			this.labelCopyright.Text = "Copyright 2006-2007, HHR";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
			this.pictureBox1.Location = new System.Drawing.Point(112, 8);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(120, 112);
			this.pictureBox1.TabIndex = 6;
			this.pictureBox1.TabStop = false;
			// 
			// AboutDialog
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(336, 263);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.labelCopyright);
			this.Controls.Add(this.labelVersion);
			this.Controls.Add(this.buttonOK);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "AboutDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About";
			this.Load += new System.EventHandler(this.AboutDialog_Load);
			this.ResumeLayout(false);

		}
		#endregion

		#region Events

		private void AboutDialog_Load(object sender, System.EventArgs e)
		{
			labelVersion.Text = Assembly.GetExecutingAssembly().GetName().Name + ", Version " 
				+ Assembly.GetExecutingAssembly().GetName().Version;
		}

		#endregion
	}
}
