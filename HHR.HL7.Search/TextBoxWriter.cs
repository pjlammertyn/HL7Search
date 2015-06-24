using System;
using System.Windows.Forms;
using System.Text; 

namespace HHR.HL7.Search
{
	/// <summary>
	/// Summary description for TextBoxWriter.
	/// </summary>
	public class TextBoxWriter : System.IO.TextWriter
	{
		#region Fields

		private TextBoxBase control;
		private StringBuilder Builder;

		#endregion

		#region Constructor

		public TextBoxWriter(TextBoxBase control)
		{
			this.control = control;
			control.HandleCreated += new EventHandler(new EventHandler(OnHandleCreated));
		}

		#endregion

		#region TextWriter methods

		public override void Write(char ch)
		{
			Write(ch.ToString());
		}

		public override void Write(string s)
		{
			if ((control.IsHandleCreated)) 
			{
				AppendText(s);
			} 
			else 
			{
				BufferText(s);
			}
		}

		public override void WriteLine(string s)
		{
			Write(s + Environment.NewLine);
		}

		public override System.Text.Encoding Encoding 
		{
			get 
			{
				return Encoding.Default;
			}
		}

		#endregion

		#region Methods

		private void BufferText(string s)
		{
			if ((Builder == null)) 
			{
				Builder = new StringBuilder();
			}
			Builder.Append(s);
		}

		private void AppendText(string s)
		{
            if (control.InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                control.BeginInvoke(new StringParameterDelegate(AppendText), new object[] { s });
                return;
            }

			if ((Builder == null == false)) 
			{
				control.AppendText(Builder.ToString());
				Builder = null;
			}
			control.AppendText(s);
			control.ScrollToCaret();
		}

		private void OnHandleCreated(object sender, EventArgs e)
		{
			if ((Builder == null == false)) 
			{
				control.AppendText(Builder.ToString());
				Builder = null;
			}
		}

		#endregion

        #region delegate

        private delegate void StringParameterDelegate(string value);

        #endregion
    }
}
