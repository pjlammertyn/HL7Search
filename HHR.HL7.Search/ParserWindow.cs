using System;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HHR.HL7.Search
{
	public class ParserWindow : WeifenLuo.WinFormsUI.Docking.DockContent
	{
		#region Fields

		private System.ComponentModel.IContainer components = null;
		private dynamic _Entity = null;
		private SortedList _EventTypes = null;
        private string _HL7PackageVersion = string.Empty;
		private string m_ValidationClass = string.Empty;
        //private BackgroundWorker backgroundWorker = null;

		#endregion

		#region Constructor

		public ParserWindow()
		{
			// This call is required by the Windows Form Designer.
			InitializeComponent();

            //backgroundWorker = new BackgroundWorker();
            //backgroundWorker.WorkerSupportsCancellation = false;
            //backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            //backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);

			//loading the eventtypes
			_EventTypes = new SortedList();
			_EventTypes.Add("A01", "admit / visit notification");
			_EventTypes.Add("A02", "transfer a patient");
			_EventTypes.Add("A03", "discharge/end visit");
			_EventTypes.Add("A04", "register a patient");
			_EventTypes.Add("A05", "pre-admit a patient");
			_EventTypes.Add("A06", "change an outpatient to an inpatient");
			_EventTypes.Add("A07", "change an inpatient to an outpatient");
			_EventTypes.Add("A08", "update patient information");
			_EventTypes.Add("A09", "patient departing - tracking");
			_EventTypes.Add("A10", "patient arriving - tracking");
			_EventTypes.Add("A11", "cancel admit / visit notification");
			_EventTypes.Add("A12", "cancel transfer");
			_EventTypes.Add("A13", "cancel discharge / end visit");
			_EventTypes.Add("A14", "pending admit");
			_EventTypes.Add("A15", "pending transfer");
			_EventTypes.Add("A16", "pending discharge");
			_EventTypes.Add("A17", "swap patients");
			_EventTypes.Add("A18", "merge patient information");
			_EventTypes.Add("A19", "patient query");
			_EventTypes.Add("A20", "bed status update");
			_EventTypes.Add("A21", "patient goes on a \"leave of absence\"");
			_EventTypes.Add("A22", "patient returns from a \"leave of absence\"");
			_EventTypes.Add("A23", "delete a patient record");
			_EventTypes.Add("A24", "link patient information");
			_EventTypes.Add("A25", " cancel pending discharge");
			_EventTypes.Add("A26", "cancel pending transfer");
			_EventTypes.Add("A27", "cancel pending admit");
			_EventTypes.Add("A28", "add person information");
			_EventTypes.Add("A29", "delete person information");
			_EventTypes.Add("A30", "merge person information");
			_EventTypes.Add("A31", "update person information");
			_EventTypes.Add("A32", "cancel patient arriving - tracking");
			_EventTypes.Add("A33", "cancel patient departing - tracking");
			_EventTypes.Add("A34", "merge patient information - patient ID only");
			_EventTypes.Add("A35", "merge patient information - account number only");
			_EventTypes.Add("A36", "merge patient information - patient ID & account number");
			_EventTypes.Add("A37", "unlink patient information");
			_EventTypes.Add("A38", "cancel pre-admit");
			_EventTypes.Add("A39", "merge person - external ID");
			_EventTypes.Add("A40", "merge patient - internal ID");
			_EventTypes.Add("A41", "merge account - patient account number");
			_EventTypes.Add("A42", "merge visit - visit number");
			_EventTypes.Add("A43", "move patient information - internal ID");
			_EventTypes.Add("A44", "move account information - patient account number");
			_EventTypes.Add("A45", "move visit information - visit number");
			_EventTypes.Add("A46", "change external ID");
			_EventTypes.Add("A47", "change internal ID");
			_EventTypes.Add("A48", "change alternate patient ID");
			_EventTypes.Add("A49", "change patient account number");
			_EventTypes.Add("A50", "change visit number");
			_EventTypes.Add("A51", "change alternate visit ID");
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
			components = new System.ComponentModel.Container();
		}
		#endregion

		#region Properties

		public dynamic Entity
		{
			get{ return _Entity; }
			set
			{ 
				_Entity = value;

                //if (!backgroundWorker.IsBusy)
                //    backgroundWorker.RunWorkerAsync();
                Task.Run(() => ProcessEntity()).ConfigureAwait(false);
			}
		}
        
		protected SortedList EventTypes
		{
			get { return _EventTypes; }
		}

		#endregion

		#region Methods

		protected virtual void ProcessEntity()
		{
		}

        #endregion 

        #region Events

        //void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //}

        //private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    ProcessEntity();
        //}

        #endregion

        #region Invoke Methods

        //protected void SetCursor(Cursor cursor)
        //{


        //    if (InvokeRequired)
        //    {
        //        // We're not in the UI thread, so we need to call BeginInvoke
        //        BeginInvoke(new SetCursorDelegate(SetCursor), new object[] { cursor });
        //        return;
        //    }
        //    // Must be on the UI thread if we've got this far
        //    this.Cursor = cursor;
        //}

        //protected void SetText(string text)
        //{
        //    if (InvokeRequired)
        //    {
        //        // We're not in the UI thread, so we need to call BeginInvoke
        //        BeginInvoke(new StringParameterDelegate(SetText), new object[] { text });
        //        return;
        //    }
        //    // Must be on the UI thread if we've got this far
        //    this.Text = text;
        //}

		#endregion

		#region Delegates

        protected delegate void StringParameterDelegate(string value);
        protected delegate void SetCursorDelegate(Cursor cursor);
		public delegate void SearchPatientIdEventHandler(object sender, PatientIdEventArgs e);
		public delegate void SearchVisitNumberEventHandler(object sender, VisitNumberEventArgs e);

		#endregion

		#region Event Declaration

		public event SearchPatientIdEventHandler SearchPatientId;

		protected virtual void OnSearchPatientId(PatientIdEventArgs e)
		{
			if (SearchPatientId != null) 
			{
				// Invokes the delegates. 
				SearchPatientId(this, e);
			}
		}

		public event SearchVisitNumberEventHandler SearchVisitNumber;

		protected virtual void OnSearchVisitNumber(VisitNumberEventArgs e)
		{
			if (SearchVisitNumber != null) 
			{
				// Invokes the delegates. 
				SearchVisitNumber(this, e);
			}
		}

		#endregion
	}
}

